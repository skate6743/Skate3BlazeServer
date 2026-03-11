FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet publish "Skate Custom Server/Skate Custom Server.csproj" \
    -c Release -o /app/publish && \
    cp -r "Skate Custom Server/bin/Debug/net8.0/wwwroot" /app/publish/wwwroot && \
    cp "Skate Custom Server/bin/Debug/net8.0/spoofed_usernames.json" /app/publish/spoofed_usernames.json

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 42100/tcp
EXPOSE 42100/udp
EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "Skate Custom Server.dll"]
