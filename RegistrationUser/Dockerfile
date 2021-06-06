FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RegistrationUser/RegistrationUser.csproj", "RegistrationUser/"]
RUN dotnet restore "RegistrationUser/RegistrationUser.csproj"
COPY . .
WORKDIR "/src/RegistrationUser"
RUN dotnet build "RegistrationUser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RegistrationUser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RegistrationUser.dll"]
