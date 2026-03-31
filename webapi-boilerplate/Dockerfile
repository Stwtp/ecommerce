# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers for better caching
COPY ["webapi-boilerplate.csproj", "./"]
RUN dotnet restore "webapi-boilerplate.csproj"

# Copy the remaining source code
COPY . .
RUN dotnet publish "webapi-boilerplate.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Environment settings
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5000

# Start the application
ENTRYPOINT ["dotnet", "webapi-boilerplate.dll"]
