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

ENV DOCKERIZE_VERSION v0.9.3
RUN apt-get update \
    && apt-get install -y wget \
    && wget -O - https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz | tar xzf - -C /usr/local/bin \
    && apt-get autoremove -yqq --purge wget && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_ENVIRONMENT $ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_URLS="http://*:80"

EXPOSE 80
ENTRYPOINT ["dockerize","-wait","tcp://postgres:5432","-wait","tcp://rabbitmq:5672","-timeout","30s","dotnet", "UniSchedule.Events.Api.dll"]
