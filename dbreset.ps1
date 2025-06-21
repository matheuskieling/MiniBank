docker compose stop postgres
docker compose rm -f postgres

# Remove the Docker volume
docker volume rm minibancovirtual_postgres_data
docker compose up -d postgres