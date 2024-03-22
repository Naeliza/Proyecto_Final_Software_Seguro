using Microsoft.AspNetCore.Mvc;
using Proyecto_Final_Software_Seguro.Models;
using System.Linq;

namespace Proyecto_Final_Software_Seguro.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskDbContext _context;

        public TasksController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: Tasks/Index
        public IActionResult Index()
        {
            var tasks = _context.Tasks.ToList();
            return View(tasks);
        }
    }
}
