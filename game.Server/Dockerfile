# Stage 1: Build the React client
FROM node:22-alpine AS client-build
WORKDIR /app/client

# Only copy package files first to cache 'npm ci'
COPY game.client/package*.json ./
RUN npm ci

# Copy source and build
COPY game.client/ ./
RUN npm run build 

# Stage 2: Build the .NET server
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS server-build
WORKDIR /app/server

# Copy solution and csproj files for better caching
COPY *.sln ./
COPY game.Server/*.csproj ./game.Server/
RUN dotnet restore ./game.Server/game.Server.csproj

# Copy server source
COPY game.Server/ ./game.Server/
WORKDIR /app/server/game.Server

# Copy built client files to the server's web root
COPY --from=client-build /app/client/dist ./wwwroot

# Publish with ReadyToRun for faster startup
RUN dotnet publish -c Release -o /app/publish /p:PublishReadyToRun=true

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app

# Use a non-root user for security (and often better performance in restricted environments)
USER $APP_UID

COPY --from=server-build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080
ENTRYPOINT ["dotnet", "game.Server.dll"]
