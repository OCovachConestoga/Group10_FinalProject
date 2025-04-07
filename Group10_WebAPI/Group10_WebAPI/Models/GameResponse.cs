namespace Group10_WebAPI.Models
{
    public class GameResponse
    {
        public int ResponseId { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public DateTime? JoinedAt { get; set; }
    }
}
