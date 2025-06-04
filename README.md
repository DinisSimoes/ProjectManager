# ProjectManager

## Pré-requisitos
Instalar o **Podman** (gerenciador de containers).

### Passos para subir o banco de dados

1. Abra o terminal e navegue até a pasta `infrastructure/docker` do projeto:

   ```bash
   cd infrastructure/docker
   ```
2. Execute o comando para subir os containers
    ```bash
    podman-compose up -d
    ```
3. Para verificar se o container está rodando, execute
    ```bash
    podman ps
    ```

### Aplicar as Migrations
Após subir os containers, abra o Package Manager Console (PMC) no seu ambiente de desenvolvimento, escolha o default project o Infrastructure e rode o comando:
```bash
Update-Database
```
