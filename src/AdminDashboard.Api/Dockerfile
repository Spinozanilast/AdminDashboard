FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AdminDashboard.Api/AdminDashboard.Api.csproj", "src/AdminDashboard.Api/"]
COPY ["src/AdminDashboard.Domain/AdminDashboard.Domain.csproj", "src/AdminDashboard.Domain/"]
COPY ["src/AdminDashboard.Infrastructure/AdminDashboard.Infrastructure.csproj", "src/AdminDashboard.Infrastructure/"]
COPY ["src/AdminDashboard.Abstractions/AdminDashboard.Abstractions.csproj", "src/AdminDashboard.Abstractions/"]
COPY ["src/AdminDashboard.Contracts/AdminDashboard.Contracts.csproj", "src/AdminDashboard.Contracts/"]
COPY ["src/AdminDashboard.Application/AdminDashboard.Application.csproj", "src/AdminDashboard.Application/"]
COPY . .
RUN dotnet restore "src/AdminDashboard.Api/AdminDashboard.Api.csproj"
WORKDIR "/src/src/AdminDashboard.Api"
RUN dotnet build "./AdminDashboard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AdminDashboard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminDashboard.Api.dll"]
