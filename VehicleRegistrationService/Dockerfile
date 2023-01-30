ARG VERSION=7.0-alpine

FROM mcr.microsoft.com/dotnet/sdk:$VERSION  AS build
WORKDIR /src

COPY ["VehicleRegistrationService.csproj", "VehicleRegistrationService.csproj"]

RUN dotnet restore "VehicleRegistrationService.csproj"
COPY . .

WORKDIR /src
RUN dotnet build "VehicleRegistrationService.csproj" -c Release --runtime alpine-x64 --self-contained true --no-restore -p:PublishSingleFile=true -o /app/build

FROM build AS publish
RUN dotnet publish \
  "VehicleRegistrationService.csproj" \
  --runtime alpine-x64 \
  --self-contained true \
  -c Release \
  /p:PublishSingleFile=true \
  -o /app/publish

# /p:PublishSingleFile=true -p:PublishTrimmed=True -p:TrimMode=partial \

FROM  mcr.microsoft.com/dotnet/runtime-deps:$VERSION
RUN addgroup -g 2000 app\
  && adduser \
  --disabled-password \
  --home /app \
  --uid 1000 --ingroup app \
  --gecos '' app \
  && chown -R app /app \
  && apk add --upgrade --no-cache apk-tools openssl zlib krb5-libs busybox \
  && apk add --no-cache icu-libs

USER app
WORKDIR /app
COPY --from=publish /app/publish .
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
  DOTNET_CLI_TELEMETRY_OPTOUT=1 \
  DOTNET_RUNNING_IN_CONTAINER=true \
  ASPNETCORE_URLS=http://+:8080 \
  ASPNETCORE_ENVIRONMENT="Production"

EXPOSE 8080
ENTRYPOINT ["./VehicleRegistrationService"]
