public class Progress
{
    public int ProgressId { get; set; }
    public int UserId { get; set; }
    public int SignsLearned { get; set; }
    public int BadgesEarned { get; set; }
    public double PracticeHours { get; set; }

    public User User { get; set; }
}