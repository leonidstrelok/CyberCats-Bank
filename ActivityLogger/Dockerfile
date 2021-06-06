FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ActivityLogger/ActivityLogger.csproj", "ActivityLogger/"]
RUN dotnet restore "ActivityLogger/ActivityLogger.csproj"
COPY . .
WORKDIR "/src/ActivityLogger"
RUN dotnet build "ActivityLogger.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ActivityLogger.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ActivityLogger.dll"]
