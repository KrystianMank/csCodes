using System.ComponentModel.DataAnnotations;

namespace ReservationSystem_WebApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "Password must contain at least {2} and at most {1} characters", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password don't match")]
        public string ConfirmPassword { get; set; }
    }
}
