using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }
        public DateTime Added { get; set; }
        public bool IsDone { get; set; }
    }
}
