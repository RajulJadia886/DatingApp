using System.ComponentModel.DataAnnotations;

namespace DatingAppAPI.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Username {get;set;}
        [Required]
        public string Password {get;set;}
    }
}