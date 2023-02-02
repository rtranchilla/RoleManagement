#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RoleService/RoleManager.WebService/RoleManager.WebService.csproj", "RoleService/RoleManager.WebService/"]
COPY ["RoleService/RoleManager.Commands/RoleManager.Commands.csproj", "RoleService/RoleManager.Commands/"]
COPY ["RoleService/RoleManager.DataPersistence/RoleManager.DataPersistence.csproj", "RoleService/RoleManager.DataPersistence/"]
COPY ["RoleService/RoleManager/RoleManager.csproj", "RoleService/RoleManager/"]
COPY ["RoleService/RoleManager.Dto/RoleManager.Dto.csproj", "RoleService/RoleManager.Dto/"]
COPY ["RoleService/RoleManager.Events/RoleManager.Events.csproj", "RoleService/RoleManager.Events/"]
COPY ["RoleService/RoleManager.Events.Dto/RoleManager.Events.Payloads.csproj", "RoleService/RoleManager.Events.Dto/"]
COPY ["RoleService/RoleManager.Mapper/RoleManager.Mapper.csproj", "RoleService/RoleManager.Mapper/"]
COPY ["RoleService/RoleManager.Queries/RoleManager.Queries.csproj", "RoleService/RoleManager.Queries/"]
RUN dotnet restore "RoleService/RoleManager.WebService/RoleManager.WebService.csproj"
COPY . .
WORKDIR "/src/RoleService/RoleManager.WebService"
RUN dotnet build "RoleManager.WebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RoleManager.WebService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RoleManager.WebService.dll"]