FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
EXPOSE 8080
WORKDIR /App

RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY ./bin/release/net6.0/ .


ENTRYPOINT ["dotnet", "PolicyApi.dll"]
