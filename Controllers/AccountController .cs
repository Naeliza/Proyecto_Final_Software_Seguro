using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Proyecto_Final_Software_Seguro.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Proyecto_Final_Software_Seguro.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Evitar Inyección SQL
                // Se utiliza parámetros en la consulta para evitar la inyección SQL
                var user = await _context.Users.FromSqlRaw("SELECT * FROM Users WHERE Username = {0}", model.Username).FirstOrDefaultAsync();

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    // Crear la identidad del usuario
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        // Aquí podrías agregar más claims según la información del usuario que quieras almacenar en la cookie de autenticación
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Iniciar sesión
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Redirigir al usuario a la página de tareas
                    return RedirectToAction("Index", "Tasks");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos");
                }
            }

            // Si llegamos aquí, algo falló, volvemos a mostrar el formulario de inicio de sesión con los errores
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            // Cerrar sesión
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirigir al usuario a la página de inicio
            return RedirectToAction("Index", "Home");
        }
    }
}
