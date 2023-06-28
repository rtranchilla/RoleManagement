# RoleManagement

## Setup

### Prereqs
- `winget install Docker.DockerDesktop`
- `winget install Microsoft.DotNet.SDK.6`

### Download nuget dependencies 
`dotnet restore D:\Repos\RoleManagement\src\RoleManager.sln`

### Build the application
`msbuild /p:SolutionPath=D:\Repos\RoleManagement\src\RoleManager.sln /p:Configuration=Release D:\Repos\RoleManagement\src\docker-compose.dcproj`

### Resources
[For more info](https://github.com/MicrosoftDocs/visualstudio-docs/blob/main/docs/containers/container-build.md#msbuild)
