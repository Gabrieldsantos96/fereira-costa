FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Fereira.Costa.Server/Fereira.Costa.Server.csproj", "Fereira.Costa.Server/"]
COPY ["Fereira.Costa.Application/Fereira.Costa.Application.csproj", "Fereira.Costa.Application/"]
COPY ["Fereira.Costa.Domain/Fereira.Costa.Domain.csproj", "Fereira.Costa.Domain/"]
COPY ["Fereira.Costa.Shared/Fereira.Costa.Shared.csproj", "Fereira.Costa.Shared/"]
COPY ["Fereira.Costa.Infra/Fereira.Costa.Infra.csproj", "Fereira.Costa.Infra/"]
RUN dotnet restore "./Fereira.Costa.Server/Fereira.Costa.Server.csproj"
COPY . .
WORKDIR "/src/Fereira.Costa.Server"
RUN dotnet build "./Fereira.Costa.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fereira.Costa.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fereira.Costa.Server.dll"]