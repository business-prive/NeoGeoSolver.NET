# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
EXPOSE 80

# copy csproj and restore as distinct layers
COPY *.sln .
COPY NeoGeoSolver.NET/NeoGeoSolver.NET.csproj ./NeoGeoSolver.NET/NeoGeoSolver.NET.csproj
COPY NeoGeoSolver.NET.Tests/NeoGeoSolver.NET.Tests.csproj ./NeoGeoSolver.NET.Tests/NeoGeoSolver.NET.Tests.csproj
COPY NeoGeoSolver.NET.UI.Web/NeoGeoSolver.NET.UI.Web.csproj ./NeoGeoSolver.NET.UI.Web/NeoGeoSolver.NET.UI.Web.csproj
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /source/NeoGeoSolver.NET.UI.Web
RUN pwd
RUN ls
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "NeoGeoSolver.NET.UI.Web.dll"]
