namespace Proyecto_Final_Software_Seguro.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Propiedad para almacenar la contraseña en texto plano
        public string PasswordHash { get; set; } // Propiedad para almacenar el hash de la contraseña
    }
}
