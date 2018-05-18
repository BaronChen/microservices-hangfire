FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY E8ay.HangFire/E8ay.HangFire.csproj E8ay.HangFire/
RUN dotnet restore E8ay.HangFire/E8ay.HangFire.csproj
COPY . .
WORKDIR /src/E8ay.HangFire
RUN dotnet build E8ay.HangFire.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish E8ay.HangFire.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "E8ay.HangFire.dll"]
