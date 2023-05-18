using Microsoft.EntityFrameworkCore;
using RemoteAlohWork.Data.Entities;

namespace RemoteAlohWork.Data.Repository
{
    public class MyDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<District> Districts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=EnverErdincApi;trusted_connection=true");
        }
    }
}
