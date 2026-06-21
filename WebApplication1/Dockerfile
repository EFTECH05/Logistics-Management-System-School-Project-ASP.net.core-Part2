FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first (important for restore cache)
COPY ["WebApplication1/WebApplication1.csproj", "WebApplication1/"]
COPY ["WebApplication1.Shared/WebApplication1.Shared.csproj", "WebApplication1.Shared/"]

RUN dotnet restore "WebApplication1/WebApplication1.csproj"

# Copy everything else
COPY . .

# IMPORTANT: publish the csproj explicitly (THIS FIXES YOUR ERROR)
RUN dotnet publish "WebApplication1/WebApplication1.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApplication1.dll"]