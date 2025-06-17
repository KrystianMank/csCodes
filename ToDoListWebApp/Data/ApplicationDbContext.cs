using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
