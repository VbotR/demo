namespace demo.Models
{
    public class UserVote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SurveyId { get; set; }
        public bool VotedYes { get; set; }
    }
}
