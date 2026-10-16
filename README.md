# Team Meeting & Remote Support Scaffold

This repository contains a minimal working scaffold for a team meeting + remote support app built with ASP.NET Core 8, SignalR, WebRTC and SQL Server.

Features included:
- Room creation/join with max 10 users
- SignalR-based signaling for WebRTC
- Voice/video and screen share flow
- Chat
- Mute/unmute
- Remote control request/grant flow (explicit permission required)
- SQL Server + Docker Compose setup

Important note:
- Browser WebRTC can share video/audio/screen but cannot directly control the target machine's mouse/keyboard without a native helper (Windows/Electron agent). This scaffold includes the permission flow and control-event channel, which is the correct architecture for a real implementation.

Quick start:

1. Build and run locally:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

2. Or with Docker:
   ```bash
   docker compose up --build
   ```

3. Open a browser to:
   ```text
   http://localhost:5000
   ```

4. Use the same room id in multiple browser tabs/windows to simulate users in one room.

Architecture summary:
- Backend: ASP.NET Core 8 + SignalR
- Database: SQL Server
- Real-time media: WebRTC
- Local LAN deployment: Docker Compose

This is a starter scaffold for building a full production system.
