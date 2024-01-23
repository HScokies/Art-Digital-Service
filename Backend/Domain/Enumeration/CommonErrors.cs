using Domain.Core.Primitives;
using System.Net;


namespace Domain.Enumeration
{
    public static class CommonErrors
    {
        public static Error Unknown = new(HttpStatusCode.InternalServerError, "Произошла неизвестная ошибка.", "unknown error");
        public static class User
        {
            public static Error NotFound = new(HttpStatusCode.NotFound, "Указанный пользователь не обнаружен!", "User not found");
            public static Error InvalidCredentials = new(HttpStatusCode.BadRequest, "Неверный логин или пароль!", "Invalid credentials");
            public static Error NonUniqueEmail = new(HttpStatusCode.BadRequest, "Пользователь с указанным email уже существует!", "Non-unique email");
            public static Error InvalidEmail = new(HttpStatusCode.BadRequest, "Указанный email не прошел валидацию!", "Invalid email");
            public static Error InvalidPhone = new(HttpStatusCode.BadRequest, "Указанный номер телефона не прошел валидацию!", "Invalid phone");
            public static Error InvalidPassword = new(HttpStatusCode.BadRequest, "Указанный пароль не прошел валидацию!", "Invalid password");
            public static Error InvalidUserType = new(HttpStatusCode.BadRequest, "Неизвестный тип учетной записи!", "Invalid user type");
            public static Error InvalidStatus = new(HttpStatusCode.BadRequest, "Неизвестный статус участника!", "Invalid user status");
            public static Error InvalidToken = new(HttpStatusCode.Unauthorized, "Неверный формат токена!", "Invalid token format");
        }
        public static class Case
        {
            public static Error NotFound = new(HttpStatusCode.NotFound, "Указанное направление не обнаружено!", "Case not found");
        }
        public static class Staff
        {
            public static Error InvalidRole = new(HttpStatusCode.BadRequest, "Неизвестная роль!", "Invalid staff role");
        }
        public static class File
        {
            public static Error NotFound = new(HttpStatusCode.NotFound, "Указанный файл не обнаружен!", "File not found");
            public static Error UnknownExtension = new (HttpStatusCode.UnsupportedMediaType, "Незвестное расширение файла!", "Unknown file extension");
            public static Error UnknownMimeType = new(HttpStatusCode.UnsupportedMediaType, "Неизвестный MIME – тип файла!", "Unknown file MIME - type");
            public static Error UnsupportedMediaType = new(HttpStatusCode.UnsupportedMediaType, "Неподдерживаемый формат файла!", "Unsupported file format");
            public static Error LargeFile = new(HttpStatusCode.RequestEntityTooLarge, "Превышен лимит на размер файла!", "File is too large");
        }
    }
}
