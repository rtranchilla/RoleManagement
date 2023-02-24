#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RoleConfiguration/RoleConfiguration.WebService/RoleConfiguration.WebService.csproj", "RoleConfiguration/RoleConfiguration.WebService/"]
COPY ["RoleConfiguration/RoleConfiguration.Commands/RoleConfiguration.Commands.csproj", "RoleConfiguration/RoleConfiguration.Commands/"]
COPY ["RoleConfiguration/RoleConfiguration.DataPersistence/RoleConfiguration.DataPersistence.csproj", "RoleConfiguration/RoleConfiguration.DataPersistence/"]
COPY ["RoleConfiguration/RoleConfiguration/RoleConfiguration.csproj", "RoleConfiguration/RoleConfiguration/"]
COPY ["RoleConfiguration/RoleConfiguration.Yaml.Serialization/RoleConfiguration.Yaml.Serialization.csproj", "RoleConfiguration/RoleConfiguration.Yaml.Serialization/"]
COPY ["RoleConfiguration/RoleConfiguration.Yaml/RoleConfiguration.Yaml.csproj", "RoleConfiguration/RoleConfiguration.Yaml/"]
COPY ["RoleConfiguration/RoleConfiguration.Mapper/RoleConfiguration.Mapper.csproj", "RoleConfiguration/RoleConfiguration.Mapper/"]
COPY ["RoleService/RoleManager.Dto/RoleManager.Dto.csproj", "RoleService/RoleManager.Dto/"]
RUN dotnet restore "RoleConfiguration/RoleConfiguration.WebService/RoleConfiguration.WebService.csproj"
COPY . .
WORKDIR "/src/RoleConfiguration/RoleConfiguration.WebService"
RUN dotnet build "RoleConfiguration.WebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RoleConfiguration.WebService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RoleConfiguration.WebService.dll"]