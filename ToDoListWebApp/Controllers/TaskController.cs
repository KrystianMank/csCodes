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
        public IActionResult AddEditTask(int? id)
        {
            if (id != null)
            {
                var taskToEdit = _context.TaskItems.FirstOrDefault(t => t.Id == id);
                if (taskToEdit != null)
                {
                    return View(taskToEdit);
                }
            }
            return View();
        }

        public IActionResult DelteTask(int id)
        {
            var taskToDelte = _context.TaskItems.FirstOrDefault(t => t.Id == id);
            if (taskToDelte != null)
            {
                _context.TaskItems.Remove(taskToDelte);
                _context.SaveChanges();
            }
            return RedirectToAction("ToDoList");
        }

        public IActionResult ToDoList()
        {
            var allTasks = _context.TaskItems.ToList();
            return View(allTasks);
        }

        public IActionResult BackToHome()
        {
            return RedirectToAction("Index", "Home");
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
