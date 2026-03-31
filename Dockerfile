# Build from repository root (directory containing gir.net.sln):
#   docker build -t gir-net .
#
# Run (pass secrets via environment — do not bake .env into the image):
#   docker run --rm -e DOTNET_ENVIRONMENT=Production \
#     -e DISCORD_TOKEN=... -e DATABASE_CONNECTION_STRING=... \
#     -e R2_ACCOUNT_ID=... -e R2_ACCESS_KEY=... -e R2_SECRET_KEY=... \
#     -e R2_PUBLIC_URL_PREFIX=... -e R2_BUCKET_NAME=... \
#     gir-net

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY gir.net.Domain/gir.net.Domain.csproj gir.net.Domain/
COPY gir.net.Application/gir.net.Application.csproj gir.net.Application/
COPY gir.net.Infrastructure/gir.net.Infrastructure.csproj gir.net.Infrastructure/
COPY gir.net/gir.net.csproj gir.net/

RUN dotnet restore gir.net/gir.net.csproj

COPY gir.net.Domain/ gir.net.Domain/
COPY gir.net.Application/ gir.net.Application/
COPY gir.net.Infrastructure/ gir.net.Infrastructure/
COPY gir.net/ gir.net/

RUN dotnet publish gir.net/gir.net.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "gir.net.dll"]
