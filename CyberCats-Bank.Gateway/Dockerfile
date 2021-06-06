FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CyberCats-Bank/CyberCats-Bank.csproj", "CyberCats-Bank/"]
RUN dotnet restore "CyberCats-Bank/CyberCats-Bank.csproj"
COPY . .
WORKDIR "/src/CyberCats-Bank"
RUN dotnet build "CyberCats-Bank.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CyberCats-Bank.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CyberCats-Bank.dll"]
