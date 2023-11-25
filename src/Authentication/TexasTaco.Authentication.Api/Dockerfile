#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Authentication/TexasTaco.Authentication.Api/TexasTaco.Authentication.Api.csproj", "src/Authentication/TexasTaco.Authentication.Api/"]
COPY ["src/Authentication/TexasTaco.Authentication.Core/TexasTaco.Authentication.Core.csproj", "src/Authentication/TexasTaco.Authentication.Core/"]
RUN dotnet restore "./src/Authentication/TexasTaco.Authentication.Api/./TexasTaco.Authentication.Api.csproj"
COPY . .
WORKDIR "/src/src/Authentication/TexasTaco.Authentication.Api"
RUN dotnet build "./TexasTaco.Authentication.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TexasTaco.Authentication.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TexasTaco.Authentication.Api.dll"]