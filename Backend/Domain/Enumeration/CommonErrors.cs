using Domain.Core.Primitives;
using System.Net;


namespace Domain.Enumeration
{
    public static class CommonErrors
    {
        public static Error Unknown = new(HttpStatusCode.InternalServerError, "Произошла неизвестная ошибка.");
        public static class User
        {
            public static Error NotFound = new(HttpStatusCode.NotFound, "Указанный пользователь не обнаружен!");
            public static Error InvalidCredentials = new(HttpStatusCode.BadRequest, "Неверный логин или пароль!");
            public static Error NonUniqueEmail = new(HttpStatusCode.BadRequest, "Пользователь с указанным email уже существует!");
            public static Error InvalidEmail = new(HttpStatusCode.BadRequest, "Указанный email не прошел валидацию!");
            public static Error InvalidPassword = new(HttpStatusCode.BadRequest, "Указанный пароль не прошел валидацию!");
            public static Error InvalidUserType = new(HttpStatusCode.BadRequest, "Неизвестный тип учетной записи!");
        }
    }
}
