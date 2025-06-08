# Etapa base para runtime com SDK (precisa do SDK para rodar `dotnet ef`)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia arquivos de projeto e restaura dependências
COPY ["ProjectManager.API/ProjectManager.API.csproj", "ProjectManager.API/"]
COPY ["ProjectManager.Application/ProjectManager.Application.csproj", "ProjectManager.Application/"]
COPY ["ProjectManager.Domain/ProjectManager.Domain.csproj", "ProjectManager.Domain/"]
COPY ["ProjectManager.Infrastructure/ProjectManager.Infrastructure.csproj", "ProjectManager.Infrastructure/"]

RUN dotnet restore "ProjectManager.API/ProjectManager.API.csproj"

# Copia o restante do código
COPY . .

WORKDIR "/src/ProjectManager.API"
RUN dotnet build "ProjectManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProjectManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final (runtime + EF CLI)
FROM base AS final
WORKDIR /app

# Copia artefatos publicados
COPY --from=publish /app/publish .

# Instala netcat (para entrypoint.sh)
RUN apt-get update && apt-get install -y netcat-openbsd

# Instala dotnet-ef globalmente
RUN dotnet tool install --global dotnet-ef \
    && export PATH="$PATH:/root/.dotnet/tools"

COPY . .

# Copia e dá permissão ao entrypoint
COPY ProjectManager.API/entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]
