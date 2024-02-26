using Microsoft.EntityFrameworkCore;

namespace Cities.Api.Infrastruture.Models
{
    public class CityContext : DbContext
    {
        public CityContext(DbContextOptions<CityContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlServer(
                "User ID=sa;password=Senha_150894;Initial Catalog=CityProject;Data Source=tcp:::1,6433;TrustServerCertificate=True;",
                options => options.MigrationsAssembly("Cities.Api.Infrastruture"));
        }

        
        public DbSet<City> Cities { get; set; } = null!;

    }
}
