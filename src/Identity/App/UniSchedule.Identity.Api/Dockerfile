FROM mcr.microsoft.com/dotnet/sdk:8.0 AS prepare-restore

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global --no-cache dotnet-subset

WORKDIR /restore
COPY ./src /restore/
RUN dotnet subset restore Identity/App/UniSchedule.Identity.Api/UniSchedule.Identity.Api.csproj --root-directory /restore --output restore_subset/

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish

WORKDIR /src
COPY --from=prepare-restore /restore/restore_subset .
RUN dotnet restore Identity/App/UniSchedule.Identity.Api/UniSchedule.Identity.Api.csproj

COPY ./src /src/
RUN dotnet publish Identity/App/UniSchedule.Identity.Api/UniSchedule.Identity.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
COPY --from=publish /app /app
ENV ASPNETCORE_ENVIRONMENT $ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_URLS="http://*:80"

EXPOSE 80
ENTRYPOINT ["dotnet", "UniSchedule.Identity.Api.dll"]
