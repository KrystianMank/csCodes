using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Models
{
    public enum Priority
    {
        Low,
        Medium,
        High,
    }
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }
        public DateTime Added { get; set; }
        public DateTime Due { get; set; }
        public bool IsDone { get; set; }
        public Priority Priority { get; set; }

        public string UserId { get; set; }
        public IdentityUser User {  get; set; }
    }
}
