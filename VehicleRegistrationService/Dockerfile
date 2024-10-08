ARG RUNTIMEVERSION=8.0-azurelinux3.0-distroless
ARG SDKVERSION=8.0-azurelinux3.0

FROM mcr.microsoft.com/dotnet/sdk:$SDKVERSION  AS build
WORKDIR /src

COPY ["VehicleRegistrationService.csproj", "VehicleRegistrationService.csproj"]

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "VehicleRegistrationService.csproj"
COPY . .

WORKDIR /src
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet build "VehicleRegistrationService.csproj" -c Release --runtime linux-x64 --self-contained true --no-restore -p:PublishSingleFile=true -o /app/build

FROM build AS publish

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish \
  "VehicleRegistrationService.csproj" \
  --runtime linux-x64 \
  --self-contained true \
  -c Release \
  /p:PublishSingleFile=true \
  -o /app/publish

# /p:PublishSingleFile=true -p:PublishTrimmed=True -p:TrimMode=partial \

FROM  mcr.microsoft.com/dotnet/runtime-deps:$RUNTIMEVERSION AS final

USER $APP_UID
WORKDIR /app
COPY --from=publish /app/publish .
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true \
  DOTNET_CLI_TELEMETRY_OPTOUT=1 \
  DOTNET_RUNNING_IN_CONTAINER=true \
  ASPNETCORE_HTTP_PORTS="8080" \
  ASPNETCORE_ENVIRONMENT="Production"

EXPOSE 8080
ENTRYPOINT ["./VehicleRegistrationService"]
