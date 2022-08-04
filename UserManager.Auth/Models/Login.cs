using System.ComponentModel.DataAnnotations;

namespace UserManager.Auth.Models
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
        
        [Required]
        [MinLength(5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}