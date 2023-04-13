FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Fabillio.Ordering.API/Fabillio.Ordering.API.csproj", "Fabillio.Ordering.API/"]
RUN dotnet restore "Fabillio.Ordering.API/Fabillio.Ordering.API.csproj"
COPY . .
WORKDIR "/src/Fabillio.Ordering.API"
RUN dotnet build "Fabillio.Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fabillio.Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fabillio.Ordering.API.dll"]
