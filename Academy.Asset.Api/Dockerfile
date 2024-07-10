FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Academy.Asset.Api/Academy.Asset.Api.csproj", "Academy.Asset.Api/"]
RUN dotnet restore "Academy.Asset.Api/Academy.Asset.Api.csproj"
COPY . .
WORKDIR "/src/Academy.Asset.Api"
RUN dotnet build "Academy.Asset.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Academy.Asset.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Academy.Asset.Api.dll"]
