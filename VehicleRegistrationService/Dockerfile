ARG RUNTIMEVERSION=10.0-noble-chiseled-amd64
ARG SDKVERSION=10.0-noble-amd64


FROM mcr.microsoft.com/dotnet/sdk:$SDKVERSION  AS build
ENV HUSKY=0

WORKDIR /src

COPY ["VehicleRegistrationService.csproj", "VehicleRegistrationService.csproj"]

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "VehicleRegistrationService.csproj" -r linux-x64 /p:PublishReadyToRun=true

COPY . .

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish \
  "VehicleRegistrationService.csproj" \
  -r linux-x64 \
  --self-contained true \
  -c Release \
  /p:PublishTrimmed=true \
  /p:PublishSingleFile=true \
  /p:IncludeNativeLibrariesForSelfExtract=true \
  /p:InvariantGlobalization=true \
  /p:PublishReadyToRun=true \
  /p:ProfileGuidedOptimization=pgodata \
  --no-restore \
  -o /app/publish

# /p:PublishSingleFile=true -p:PublishTrimmed=True -p:TrimMode=partial \

FROM  mcr.microsoft.com/dotnet/runtime-deps:$RUNTIMEVERSION AS final

WORKDIR /app
COPY --from=build /app/publish .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true \
  DOTNET_CLI_TELEMETRY_OPTOUT=1 \
  DOTNET_RUNNING_IN_CONTAINER=true \
  ASPNETCORE_HTTP_PORTS="8080" \
  ASPNETCORE_ENVIRONMENT="Production" \
  TZ=Etc/UTC

USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["./VehicleRegistrationService"]
