FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["TimeClockApi.csproj", "./"]
RUN dotnet restore "TimeClockApi.csproj"

# Copy everything except tests
COPY ["Controllers/", "Controllers/"]
COPY ["Models/", "Models/"]
COPY ["Dal/", "Dal/"]
COPY ["DataAccess/", "DataAccess/"]
COPY ["Program.cs", "./"]
COPY ["appsettings.json", "./"]

RUN dotnet publish "TimeClockApi.csproj" -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TimeClockApi.dll"] 