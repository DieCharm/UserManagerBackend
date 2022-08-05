using System.ComponentModel.DataAnnotations;

namespace UserManager.Auth.Models
{
    public class Register
    {
        [Required] 
        [MinLength(2)]
        [MaxLength(30)]
        public string UserName { get; set; }
        
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
        
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        
        [Required]
        [DataType(DataType.Url)]
        public string ClientAddress { get; set; }
    }
}