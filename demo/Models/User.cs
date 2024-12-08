namespace demo.Models
{
    public class User
    {
        public int Id { get; set; } // Уникальный идентификатор
        public string Username { get; set; } // Имя пользователя
        public string Password { get; set; } // Пароль (рекомендуется хэшировать)
        public string Email { get; set; } // Электронная почта
    }
}
