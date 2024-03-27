using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Importa el espacio de nombres necesario
using Proyecto_Final_Software_Seguro.Models;
using System.Diagnostics;

namespace Proyecto_Final_Software_Seguro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Verifica si el usuario está autenticado
            if (User.Identity.IsAuthenticated)
            {
                // Si el usuario está autenticado, muestra la vista correspondiente
                return View();
            }
            else
            {
                // Si el usuario no está autenticado, redirige a la página de inicio de sesión
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
