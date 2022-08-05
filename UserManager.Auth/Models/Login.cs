using System.ComponentModel.DataAnnotations;

namespace UserManager.Auth.Models
{
    public class Login
    {
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
        
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}