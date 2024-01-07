using Domain.Core.Primitives;


namespace Domain.Enumeration
{
    public static class CommonErrors
    {
        public static Error Unknown = new(500, "Произошла неизвестная ошибка");
        public static class User
        {
            public static Error NotFound = new(404, "Указанный пользователь не обнаружен!");
            public static Error InvalidEmail = new(400, "Указанный email не прошел валидацию!");
        }
    }
}
