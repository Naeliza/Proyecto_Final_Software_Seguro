using Microsoft.AspNetCore.Mvc;
using Proyecto_Final_Software_Seguro.Models;

namespace Proyecto_Final_Software_Seguro.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
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
