using Microsoft.AspNetCore.Identity;

namespace Group10_WebAPI.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
    }
}
