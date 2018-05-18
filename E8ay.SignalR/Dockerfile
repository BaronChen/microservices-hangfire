FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY E8ay.SignalR/E8ay.SignalR.csproj E8ay.SignalR/
RUN dotnet restore E8ay.SignalR/E8ay.SignalR.csproj
COPY . .
WORKDIR /src/E8ay.SignalR
RUN dotnet build E8ay.SignalR.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish E8ay.SignalR.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "E8ay.SignalR.dll"]
