namespace demo.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int VotesYes { get; set; } = 0;
        public int VotesNo { get; set; } = 0;

        // Хранение ID проголосовавших пользователей
        public string VotedUserIds { get; set; } = ""; // Список ID пользователей в формате "1,2,3"
    }
}
