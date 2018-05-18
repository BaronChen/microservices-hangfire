FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY E8ay.Bid.Api/E8ay.Bid.Api.csproj E8ay.Bid.Api/
RUN dotnet restore E8ay.Bid.Api/E8ay.Bid.Api.csproj
COPY . .
WORKDIR /src/E8ay.Bid.Api
RUN dotnet build E8ay.Bid.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish E8ay.Bid.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "E8ay.Bid.Api.dll"]
