using Gym13.Domain.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Extensions
{
    public class Gym13DbContextExtension : IDesignTimeDbContextFactory<Gym13DbContext>
    {
        public Gym13DbContext CreateDbContext(string[] args)
        {
            var connectionString = "User ID=postgres;Password=Gym13-secret;Host=gym13-db.cektfzc1tnqp.eu-central-1.rds.amazonaws.com;Port=5432;Database=postgres;";

            var optionsBuilder = new DbContextOptionsBuilder<Gym13DbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new Gym13DbContext(optionsBuilder.Options);
        }
    }
}
