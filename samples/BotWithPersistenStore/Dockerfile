FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["../../samples/BotWithPersistenStore/BotWithPersistenStore.csproj", "../../samples/BotWithPersistenStore/"]
COPY ["TgBotFramework.Core/TgBotFramework.Core.csproj", "TgBotFramework.Core/"]
COPY ["../TgBotFramework.Persistent/TgBotFramework.Persistent.csproj", "../TgBotFramework.Persistent/"]
RUN dotnet restore "../../samples/BotWithPersistenStore/BotWithPersistenStore.csproj"
COPY . .
WORKDIR "/src/../../samples/BotWithPersistenStore"
RUN dotnet build "BotWithPersistenStore.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BotWithPersistenStore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BotWithPersistenStore.dll"]
