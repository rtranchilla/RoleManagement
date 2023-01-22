#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RoleManager.WebService/RoleManager.WebService.csproj", "RoleManager.WebService/"]
COPY ["RoleManager.Commands/RoleManager.Commands.csproj", "RoleManager.Commands/"]
COPY ["RoleManager.DataPersistence/RoleManager.DataPersistence.csproj", "RoleManager.DataPersistence/"]
COPY ["RoleManager/RoleManager.csproj", "RoleManager/"]
COPY ["RoleManager.Dto/RoleManager.Dto.csproj", "RoleManager.Dto/"]
COPY ["RoleManager.Events/RoleManager.Events.csproj", "RoleManager.Events/"]
COPY ["RoleManager.Events.Dto/RoleManager.Events.Payloads.csproj", "RoleManager.Events.Dto/"]
COPY ["RoleManager.Mapper/RoleManager.Mapper.csproj", "RoleManager.Mapper/"]
COPY ["RoleManager.Queries/RoleManager.Queries.csproj", "RoleManager.Queries/"]
RUN dotnet restore "RoleManager.WebService/RoleManager.WebService.csproj"
COPY . .
WORKDIR "/src/RoleManager.WebService"
RUN dotnet build "RoleManager.WebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RoleManager.WebService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RoleManager.WebService.dll"]