using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Agrega este espacio de nombres
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
        [Authorize] // Requiere autenticación para acceder a esta acción
        public IActionResult Index()
        {
            var tasks = _context.Tasks.ToList();
            return View(tasks);
        }
    }
}
