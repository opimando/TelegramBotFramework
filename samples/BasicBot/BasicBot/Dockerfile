FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["../../samples/BasicBot/BasicBot/BasicBot.csproj", "../../samples/BasicBot/BasicBot/"]
COPY ["TgBotFramework.Core/TgBotFramework.Core.csproj", "TgBotFramework.Core/"]
RUN dotnet restore "../../samples/BasicBot/BasicBot/BasicBot.csproj"
COPY . .
WORKDIR "/src/../../samples/BasicBot/BasicBot"
RUN dotnet build "BasicBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BasicBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasicBot.dll"]
