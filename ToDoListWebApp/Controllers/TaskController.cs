using Microsoft.AspNetCore.Mvc;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult AddEditTask()
        {
            return View();
        }
        public IActionResult AddEditTaskSubmit(TaskItem taskItem)
        {
            if(taskItem.Id == 0)
            {
                _context.TaskItems.Add(taskItem);
            }
            else
            {
                _context.TaskItems.Update(taskItem);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
