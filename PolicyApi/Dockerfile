FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
EXPOSE 8080

RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY . App/
WORKDIR /App

ENTRYPOINT ["dotnet", "PolicyApi.dll"]
