using System;
using System.ComponentModel.DataAnnotations;

namespace UserManager.Data
{
    public class User
    {
        public int Id { get; set; }
        [MinLength(2)]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MinLength(3)]
        [MaxLength(30)]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [MaxLength(200)]
        public string Info { get; set; }
    }
}