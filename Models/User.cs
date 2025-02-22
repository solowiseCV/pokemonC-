using System.ComponentModel.DataAnnotations;

namespace PokesMan.Models
{
    public class User
    {
     
        [Key]
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpiry { get; set; }
    
}
}
