using ReservationSystemWebApp.Data;
using System.ComponentModel.DataAnnotations;
namespace ReservationSystemWebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
        public Data.Data.AccessType AccessType { get; set; }
        public User(string name, string email, string password, Data.Data.AccessType accessType) 
        {
            Name = name;
            Email = email;
            Password = password;
            AccessType = accessType;
        }
    }
}
