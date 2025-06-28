using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ReservationSystem_WebApp.ViewModels
{
    public class ReservationViewModel
    {
        public ReservationViewModel() 
        { 
            BeginDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            ConferenceRoomId = 1;
        }
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "Begin Date")]
        public DateTime BeginDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required, NotNull]
        [Display(Name = "Conference Room Id")]
        public int ConferenceRoomId { get; set; }

        [AllowNull]
        [Display(Name = "Purpose")]
        [StringLength(100, ErrorMessage = "The purpose text is too long")]
        public string? Purpose { get; set; }

        [Required]
        [Display(Name = "Participants")]
        public int Participants {  get; set; }
    }
}
