FROM mcr.microsoft.com/dotnet/sdk:8.0 AS prepare-restore

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global --no-cache dotnet-subset

WORKDIR /restore
COPY ./src /restore/
RUN dotnet subset restore Events/App/UniSchedule.Events.Api/UniSchedule.Events.Api.csproj --root-directory /restore --output restore_subset/

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish

WORKDIR /src
COPY --from=prepare-restore /restore/restore_subset .
RUN dotnet restore Events/App/UniSchedule.Events.Api/UniSchedule.Events.Api.csproj

COPY ./src /src/
RUN dotnet publish Events/App/UniSchedule.Events.Api/UniSchedule.Events.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
COPY --from=publish /app /app
ENV ASPNETCORE_ENVIRONMENT $ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_URLS="http://*:80"

EXPOSE 80
ENTRYPOINT ["dotnet", "UniSchedule.Events.Api.dll"]
