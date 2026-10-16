using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace TeamMeetingRemoteSupport.Hubs;

public class MeetingHub : Hub
{
    private const int MaxUsersPerRoom = 10;

    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> RoomMembers = new();
    private static readonly ConcurrentDictionary<string, string> ConnectionToRoom = new();

    public async Task JoinRoom(string roomCode, string userName)
    {
        var room = RoomMembers.GetOrAdd(roomCode, _ => new ConcurrentDictionary<string, string>());

        if (room.Count >= MaxUsersPerRoom)
        {
            await Clients.Caller.SendAsync("RoomFull");
            return;
        }

        room[Context.ConnectionId] = userName;
        ConnectionToRoom[Context.ConnectionId] = roomCode;

        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
        await Clients.Group(roomCode).SendAsync("UserJoined", userName, Context.ConnectionId);
    }

    public async Task LeaveRoom(string roomCode)
    {
        if (RoomMembers.TryGetValue(roomCode, out var room))
        {
            room.TryRemove(Context.ConnectionId, out _);
            ConnectionToRoom.TryRemove(Context.ConnectionId, out _);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);
            await Clients.Group(roomCode).SendAsync("UserLeft", Context.ConnectionId);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roomCode = ConnectionToRoom.TryGetValue(Context.ConnectionId, out var room) ? room : null;

        if (!string.IsNullOrWhiteSpace(roomCode) && RoomMembers.TryGetValue(roomCode, out var roomMembers))
        {
            roomMembers.TryRemove(Context.ConnectionId, out _);
            await Clients.Group(roomCode).SendAsync("UserLeft", Context.ConnectionId);
        }

        ConnectionToRoom.TryRemove(Context.ConnectionId, out _);
        await base.OnDisconnectedAsync(exception);
    }

    public Task SendOffer(string roomCode, string toConnectionId, string sdp)
        => Clients.Client(toConnectionId).SendAsync("ReceiveOffer", Context.ConnectionId, sdp, roomCode);

    public Task SendAnswer(string roomCode, string toConnectionId, string sdp)
        => Clients.Client(toConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, sdp, roomCode);

    public Task SendIceCandidate(string roomCode, string toConnectionId, string candidate)
        => Clients.Client(toConnectionId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate, roomCode);

    public Task SendChat(string roomCode, string userName, string message)
        => Clients.Group(roomCode).SendAsync("ChatMessage", userName, message);

    public Task ToggleMute(string roomCode, string userName, bool muted)
        => Clients.Group(roomCode).SendAsync("MuteChanged", userName, muted);

    public Task RequestControl(string roomCode, string requesterName, string targetConnectionId)
        => Clients.Client(targetConnectionId).SendAsync("ControlRequested", requesterName, Context.ConnectionId, roomCode);

    public Task GrantControl(string roomCode, string targetConnectionId, string grantedBy)
        => Clients.Client(targetConnectionId).SendAsync("ControlGranted", grantedBy, roomCode);

    public Task RevokeControl(string roomCode, string targetConnectionId)
        => Clients.Client(targetConnectionId).SendAsync("ControlRevoked", roomCode);

    public Task SendControlEvent(string roomCode, string toConnectionId, string payloadJson)
        => Clients.Client(toConnectionId).SendAsync("ReceiveControlEvent", Context.ConnectionId, payloadJson, roomCode);
}
