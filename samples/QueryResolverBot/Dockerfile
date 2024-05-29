FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["../../samples/QueryResolverBot/QueryResolverBot.csproj", "../../samples/QueryResolverBot/"]
COPY ["TgBotFramework.Core/TgBotFramework.Core.csproj", "TgBotFramework.Core/"]
RUN dotnet restore "../../samples/QueryResolverBot/QueryResolverBot.csproj"
COPY . .
WORKDIR "/src/../../samples/QueryResolverBot"
RUN dotnet build "QueryResolverBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "QueryResolverBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QueryResolverBot.dll"]
