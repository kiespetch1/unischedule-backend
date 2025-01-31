FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Identity/App/UniSchedule.Identity.Api/UniSchedule.Identity.Api.csproj", "Identity/App/UniSchedule.Identity.Api/"]
RUN dotnet restore "Identity/App/UniSchedule.Identity.Api/UniSchedule.Identity.Api.csproj"
COPY . .
WORKDIR "/src/Identity/App/UniSchedule.Identity.Api"
RUN dotnet build "UniSchedule.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UniSchedule.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSchedule.Identity.Api.dll"]
