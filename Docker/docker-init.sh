cd ../Backend
docker build . --tag ads-backend --no-cache

cd ../Frontend
npm install
npm run build
docker build . --tag ads-frontend --no-cache

cd ../Docker

docker save -o ads-backend.tar ads-backend
docker save -o ads-frontend.tar ads-frontend

touch nginx-database.env
echo MYSQL_DATABASE="string" >> nginx-database.env
echo MYSQL_USER="string" >> nginx-database.env
echo MYSQL_PASSWORD="string" >> nginx-database.env
echo MYSQL_RANDOM_ROOT_PASSWORD="yes" >> nginx-database.env

touch nginx-config.env
echo DB_MYSQL_HOST="nginx-database" >> nginx-config.env
echo DB_MYSQL_PORT=3306 >> nginx-config.env
echo DB_MYSQL_USER="string" >> nginx-config.env
echo DB_MYSQL_PASSWORD="string" >> nginx-config.env
echo DB_MYSQL_NAME="string" >> nginx-config.env

touch backend-database.env
echo POSTGRES_DB="postgres" >> backend-database.env
echo POSTGRES_EFCORE_DB="string" >>backend-database.env 
echo POSTGRES_USER="postgres" >> backend-database.env
echo POSTGRES_PASSWORD="string" >> backend-database.env

touch JWT-config.env
echo JWT_SECRET="string" >> JWT-config.env
echo JWT_ISSUER="string" >> JWT-config.env
echo JWT_AUDIENCE="string" >> JWT-config.env
echo JWT_REFRESH_EXPIRY_HOURS=number >> JWT-config.env
echo JWT_ACCESS_EXPIRY_MINUTES=number >> JWT-config.env

touch SMTP-config.env
echo SMTP_HOST="string" >> SMTP-config.env
echo SMTP_PORT=number >> SMTP-config.env
echo SMTP_LOGIN="string" >> SMTP-config.env
echo SMTP_PASSWORD="string" >> SMTP-config.env

touch app-config.env
echo POSTGRES_HOST="ADS-database" >> app-config.env
echo POSTGRES_PORT=5432 >> app-config.env
echo FRONTEND_URL="string" >> app-config.env
echo BACKEND_URL="string" >> app-config.env
<<<<<<< HEAD

rm -- "$0"
=======
>>>>>>> 2883e5a5e955d19681ae8dcb97e3c4b3162ec5e5
