using Microsoft.EntityFrameworkCore;

namespace P01_DonisSanchez_MoralesQuezada.Modelos
{
    public class ParqueoDBcontext : DbContext
    {
        public ParqueoDBcontext(DbContextOptions<ParqueoDBcontext> options) : base(options)
        {
        }

        public DbSet<EspaciosParqueo> espacios { get; set; }

        public DbSet<Reservas> reservas { get; set; }

        public DbSet<Sucursales> sucursales { get; set; }

        public DbSet<Usuarios> usuarios { get; set; }

    }
}
