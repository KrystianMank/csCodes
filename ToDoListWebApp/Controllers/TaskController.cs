using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Controllers
{
    [Authorize]
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
            return RedirectToAction("ToDoList", "Task");
        }

        public IActionResult Filter(int option)
        {
            return RedirectToAction("ToDoList", new {option});
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

        public async Task<IActionResult> ToDoList(int option)
        {
            List<TaskItem> taskItems = option switch
            {
                2 => await _context.TaskItems.Where(t => t.IsDone).ToListAsync(),
                3 => await _context.TaskItems.Where(t => !t.IsDone).ToListAsync(),
                4 => await _context.TaskItems.Where(t => t.Priority == Priority.High).ToListAsync(),
                5 => await _context.TaskItems.Where(t => t.Priority == Priority.Medium).ToListAsync(),
                6 => await _context.TaskItems.Where(t => t.Priority == Priority.Low).ToListAsync(),
                _ => await _context.TaskItems.ToListAsync()
            };
            await DeletePastDueTasks();
            return View(taskItems);
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

        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus(int id, bool isDone)
        {
            try
            {
                var task = await _context.TaskItems.FindAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                task.IsDone = isDone;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        private async Task DeletePastDueTasks()
        {
            var list = await _context.TaskItems.Where(t => t.Due <= DateTime.Now).ToListAsync();
            _context.TaskItems.RemoveRange(list);
            await _context.SaveChangesAsync();

        }
    }
}
