using System.ComponentModel.DataAnnotations;

namespace Group10_WebAPI.Models
{
    public class GameResponse
    {
        [Key]
        public int ResponseId { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public DateTime? JoinedAt { get; set; }
    }
}
