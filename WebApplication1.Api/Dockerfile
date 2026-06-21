FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["WebApplication1.Api/WebApplication1.Api.csproj", "WebApplication1.Api/"]
COPY ["WebApplication1.Shared/WebApplication1.Shared.csproj", "WebApplication1.Shared/"]

RUN dotnet restore "WebApplication1.Api/WebApplication1.Api.csproj"

COPY . .
WORKDIR "/src/WebApplication1.Api"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApplication1.Api.dll"]