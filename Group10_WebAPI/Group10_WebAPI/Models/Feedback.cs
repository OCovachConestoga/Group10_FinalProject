namespace Group10_WebAPI.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string? UserEmail { get; set; }
        public string? Message { get; set; }
        public DateTime? SubmittedAt { get; set; }
    }
}
