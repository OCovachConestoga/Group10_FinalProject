namespace Group10_WebAPI.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public string? Location { get; set; }
        public DateTime? Date { get; set; }
        public int MaxPlayers { get; set; }
        public string? Organizer { get; set; }
        public bool IsFull { get; set; } = false;
    }
}
