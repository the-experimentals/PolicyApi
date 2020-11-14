FROM mcr.microsoft.com/dotnet/aspnet:5.0.0-buster-slim AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:5.0.100-buster-slim AS build
WORKDIR /src
COPY PolicyApi.csproj /src
RUN dotnet restore "PolicyApi.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "PolicyApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PolicyApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PolicyApi.dll"]
