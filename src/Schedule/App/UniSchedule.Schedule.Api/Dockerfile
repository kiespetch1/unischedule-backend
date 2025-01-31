FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UniSchedule.Api/UniSchedule.Api.csproj", "UniSchedule.Api/"]
RUN dotnet restore "UniSchedule.Api/UniSchedule.Api.csproj"
COPY . .
WORKDIR "/src/UniSchedule.Api"
RUN dotnet build "UniSchedule.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UniSchedule.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSchedule.Api.dll"]
