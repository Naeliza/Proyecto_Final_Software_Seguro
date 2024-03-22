using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final_Software_Seguro.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<LoginViewModel> Users { get; set; }
    }
}
