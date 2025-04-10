using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Group10_WebAPI.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
    }
}
