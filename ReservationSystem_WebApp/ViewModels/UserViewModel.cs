using ReservationSystem_WebApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem_WebApp.ViewModels
{
    public class UserViewModel
    {
        public string? Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Access Type")]
        public ApplicationData.AccessType AccessType { get; set; }
    }
}
