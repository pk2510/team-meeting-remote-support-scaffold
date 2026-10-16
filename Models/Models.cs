namespace TeamMeetingRemoteSupport.Models;

public class UserAccount
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class MeetingRoom
{
    public int Id { get; set; }
    public string RoomCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int MaxUsers { get; set; } = 10;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class RoomMember
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int UserId { get; set; }
    public string ConnectionId { get; set; } = string.Empty;
    public bool IsPresenter { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}

public class ControlGrant
{
    public int Id { get; set; }
    public string RoomCode { get; set; } = string.Empty;
    public string RequesterConnectionId { get; set; } = string.Empty;
    public string TargetConnectionId { get; set; } = string.Empty;
    public bool IsGranted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
