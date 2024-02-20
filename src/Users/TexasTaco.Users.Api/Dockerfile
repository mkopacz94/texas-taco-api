#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Users/TexasTaco.Users.Api/TexasTaco.Users.Api.csproj", "src/Users/TexasTaco.Users.Api/"]
RUN dotnet restore "./src/Users/TexasTaco.Users.Api/./TexasTaco.Users.Api.csproj"
COPY . .
WORKDIR "/src/src/Users/TexasTaco.Users.Api"
RUN dotnet build "./TexasTaco.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TexasTaco.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TexasTaco.Users.Api.dll"]