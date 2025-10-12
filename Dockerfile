# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln ./
COPY aLMS.API/*.csproj ./aLMS.API/
COPY aLMS.Domain/*.csproj ./aLMS.Domain/
COPY aLMS.Application/*.csproj ./aLMS.Application/
COPY aLMS.Infrastructure/*.csproj ./aLMS.Infrastructure/

RUN dotnet restore

COPY . .
WORKDIR /src/aLMS.API
RUN dotnet publish -c Release -o /app /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 5000
ENTRYPOINT ["dotnet", "aLMS.API.dll"]
