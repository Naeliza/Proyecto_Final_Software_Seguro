using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Proyecto_Final_Software_Seguro.Models;
using System.Security.Claims;


namespace Proyecto_Final_Software_Seguro.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        #region Controlador de Inicio de sesión
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
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    // La autenticación es exitosa
                    // Crear la identidad del usuario
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Iniciar sesión
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Redirigir al usuario a la página de tareas
                    return RedirectToAction("Index", "Tasks");
                }
                else
                {
                    // La autenticación falló
                    // Agregar el mensaje de error al modelo para mostrarlo en la vista
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
        #endregion

        #region Controlador de registro
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el nombre de usuario ya está en uso
                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "El nombre de usuario ya está en uso.");
                    return View(model);
                }

                try
                {
                    // Hashear la contraseña antes de guardarla en la base de datos
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                    // Crear un nuevo usuario con los datos del modelo
                    var newUser = new User
                    {
                        Id = Guid.NewGuid(), // Generar un nuevo GUID para el ID del usuario
                        Username = model.Username,
                        Password = model.Password,
                        PasswordHash = hashedPassword
                    };

                    // Agregar el nuevo usuario al contexto y guardar los cambios en la base de datos
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    // Redirigir al usuario a la página de inicio de sesión
                    return RedirectToAction("Login", "Account");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier error que pueda ocurrir al guardar el usuario en la base de datos
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al registrar el usuario.");
                    // Loguear el error para su posterior análisis
                    // logger.LogError(ex, "Error al registrar usuario");
                    return View(model);
                }
            }

            // Si llegamos aquí, algo falló, volvemos a mostrar el formulario de registro con los errores
            return View(model);
        }
    }
    #endregion

}