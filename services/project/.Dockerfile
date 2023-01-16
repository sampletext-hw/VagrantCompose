FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ./WebAPI ./WebAPI
COPY ./Infrastructure ./Infrastructure
COPY ./Models ./Models
COPY ./Services ./Services

# restore only main project, it references everything that is required
RUN dotnet restore ./WebAPI/WebAPI.csproj

RUN dotnet publish ./WebAPI/WebAPI.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out /app
ENTRYPOINT ["dotnet", "/app/WebAPI.dll"]