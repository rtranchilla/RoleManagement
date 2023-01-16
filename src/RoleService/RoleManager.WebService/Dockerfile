#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RoleManagement.RoleManagementService.Web/RoleManagement.RoleManagementService.Web.csproj", "RoleManagement.RoleManagementService.Web/"]
COPY ["RoleManagement.RoleManagementService.Commands/RoleManagement.RoleManagementService.Commands.csproj", "RoleManagement.RoleManagementService.Commands/"]
COPY ["RoleManagement.RoleManagementService.DataPersistence/RoleManagement.RoleManagementService.DataPersistence.csproj", "RoleManagement.RoleManagementService.DataPersistence/"]
COPY ["RoleManagement.RoleManagementService/RoleManagement.RoleManagementService.csproj", "RoleManagement.RoleManagementService/"]
COPY ["RoleManagement.RoleManagementService.Dto/RoleManagement.RoleManagementService.Dto.csproj", "RoleManagement.RoleManagementService.Dto/"]
COPY ["RoleManagement.RoleManagementService.MapperConfig/RoleManagement.RoleManagementService.MapperConfig.csproj", "RoleManagement.RoleManagementService.MapperConfig/"]
COPY ["RoleManagement.RoleManagementService.Queries/RoleManagement.RoleManagementService.Queries.csproj", "RoleManagement.RoleManagementService.Queries/"]
RUN dotnet restore "RoleManagement.RoleManagementService.Web/RoleManagement.RoleManagementService.Web.csproj"
COPY . .
WORKDIR "/src/RoleManagement.RoleManagementService.Web"
RUN dotnet build "RoleManagement.RoleManagementService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RoleManagement.RoleManagementService.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RoleManagement.RoleManagementService.Web.dll"]