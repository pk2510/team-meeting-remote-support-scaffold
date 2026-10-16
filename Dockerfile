version: '3.8'

services:
  app:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:8080"
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__DefaultConnection: "Server=db,1433;Database=TeamMeetingDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
    depends_on:
      - db
    restart: unless-stopped
    networks:
      - meeting-net

  db:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "YourStrong!Passw0rd"
    ports:
      - "1433:1433"
    restart: unless-stopped
    networks:
      - meeting-net

networks:
  meeting-net:
    driver: bridge
