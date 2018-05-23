FROM microsoft/aspnetcore:2.0.8 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY E8ay.User.Api/E8ay.User.Api.csproj E8ay.User.Api/
RUN dotnet restore E8ay.User.Api/E8ay.User.Api.csproj
COPY . .
WORKDIR /src/E8ay.User.Api
RUN dotnet build E8ay.User.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish E8ay.User.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "E8ay.User.Api.dll"]
