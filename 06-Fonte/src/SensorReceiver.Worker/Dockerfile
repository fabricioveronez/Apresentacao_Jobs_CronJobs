FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS buildAndTest
WORKDIR /src
COPY src/SensorReceiver.Worker/SensorReceiver.Worker.csproj src/SensorReceiver.Worker/
RUN dotnet restore src/SensorReceiver.Worker/SensorReceiver.Worker.csproj
COPY . .
WORKDIR /src/src/SensorReceiver.Worker
RUN dotnet build SensorReceiver.Worker.csproj -c Release -o /app
WORKDIR /src/src/SensorReceiver.Worker.Test.Unit
RUN dotnet test SensorReceiver.Worker.Test.Unit.csproj
WORKDIR /src/src/SensorReceiver.Worker

FROM buildAndTest AS publish
RUN dotnet publish SensorReceiver.Worker.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SensorReceiver.Worker.dll"]
