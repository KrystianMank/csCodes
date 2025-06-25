using System.ComponentModel.DataAnnotations;

namespace ReservationSystem_WebApp.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remeber Me")]
        public bool RemeberMe { get; set; }
    }
}
