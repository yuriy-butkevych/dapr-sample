FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Fabillio.Inventory.API/Fabillio.Inventory.API.csproj", "Fabillio.Inventory.API/"]
COPY ["nuget.config", "./"]
RUN dotnet restore "Fabillio.Inventory.API/Fabillio.Inventory.API.csproj"
COPY . .
WORKDIR "/src/Fabillio.Inventory.API"
RUN dotnet build "Fabillio.Inventory.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fabillio.Inventory.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 6012/tcp
ENV ASPNETCORE_URLS http://*:6012
ENTRYPOINT ["dotnet", "Fabillio.Inventory.API.dll"]