using Microsoft.EntityFrameworkCore;
using Trabajadores.Web.Models;

namespace Trabajadores.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trabajador> Trabajadores { get; set; }
    }
}
