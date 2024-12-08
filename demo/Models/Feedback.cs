namespace demo.Models;

public class Feedback
{
    public int Id { get; set; } // Уникальный идентификатор
    public string Username { get; set; } // Имя пользователя
    public string Content { get; set; } // Текст отзыва
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Дата и время создания отзыва
}
