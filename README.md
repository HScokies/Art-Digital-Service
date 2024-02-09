# Art Digital Service

## Описание
Данный проект был разработан в рамках дипломной работ и представляет собой дэшборд для локальной олимпиады «Цифра. Дизайн. Сервис»

P.S. Для создания изображений и .env файлов можно воспользоваться Docker/docker-init.sh

## Переменные окружения

### nginx-database.env
Содержит информацию для создания контейнера с **MySQL**, используемого **Nginx**
```
MYSQL_DATABASE="string"
MYSQL_USER="string"
MYSQL_PASSWORD="string"
MYSQL_RANDOM_ROOT_PASSWORD="yes"
```

### nginx-config.env
Содержит информацию для подключения **Nginx** к **MySQL**
```
DB_MYSQL_HOST="string" # Имя контейнера с MySQL
DB_MYSQL_PORT=3306
DB_MYSQL_USER="string"
DB_MYSQL_PASSWORD="string"
DB_MYSQL_NAME="string"
```
### backend-database.env
Содержит информацию для создания контейнера с **PostgreSQL**, используемого **Backend**'ом, а также для подключения к нему
```
POSTGRES_DB="string"
POSTGRES_EFCORE_DB="string" # Имя БД, создаваемой с помощью Entity Framework Core
POSTGRES_USER="string"
POSTGRES_PASSWORD="string"
```

### JWT-config.env
Содержит информацию для генерации **JSON Web Tokens**
```
JWT_SECRET="string" # 384-битная строка
JWT_ISSUER="string"
JWT_AUDIENCE="string"
JWT_REFRESH_EXPIRY_HOURS=number
JWT_ACCESS_EXPIRY_MINUTES=number
```

### SMTP-config.env
Содержит информацию для авторизации и отправки писем через **Simple Mail Transfer Protocol**
```
SMTP_HOST="string"
SMTP_PORT=number
SMTP_LOGIN="string"
SMTP_PASSWORD="string"
```

### app-config.env
Содержит информация для контейнеров с **Frontend**, **Backend**
```
POSTGRES_HOST="string" # Имя контейнера с PostgreSQL
POSTGRES_PORT=5432
FRONTEND_URL="string" # Для CORS
BACKEND_URL="string" # Для HTTP Запросов
```

