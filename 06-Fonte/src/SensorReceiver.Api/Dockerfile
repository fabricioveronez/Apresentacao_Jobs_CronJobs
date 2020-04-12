FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS buildAndTest
WORKDIR /src
COPY src/SensorReceiver.Api/SensorReceiver.Api.csproj src/SensorReceiver.Api/
RUN dotnet restore src/SensorReceiver.Api/SensorReceiver.Api.csproj
COPY . .
WORKDIR /src/src/SensorReceiver.Api
RUN dotnet build SensorReceiver.Api.csproj -c Release -o /app
WORKDIR /src/src/SensorReceiver.Api.Test.Unit
RUN dotnet test SensorReceiver.Api.Test.Unit.csproj
WORKDIR /src/src/SensorReceiver.Api



FROM buildAndTest AS publish
RUN dotnet publish SensorReceiver.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SensorReceiver.Api.dll"]
