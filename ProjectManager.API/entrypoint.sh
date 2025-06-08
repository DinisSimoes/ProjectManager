#!/bin/sh
set -e

export PATH="$PATH:/root/.dotnet/tools"

host="projectmanager_db"
port=5432

echo "Aguardando o banco de dados em $host:$port..."

while ! nc -z $host $port; do
  echo "Banco de dados indisponível - tentando novamente em 1s..."
  sleep 1
done

echo "Banco de dados disponível! Aplicando migrations..."

cd /app/ProjectManager.Infrastructure
dotnet ef database update

echo "Iniciando a aplicação..."
cd /app
exec dotnet ProjectManager.API.dll
