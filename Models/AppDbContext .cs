using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final_Software_Seguro.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> Users { get; set; } // Cambiar LoginViewModel a User
    }
}
