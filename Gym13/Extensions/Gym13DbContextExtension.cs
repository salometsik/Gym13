using Gym13.Domain.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Extensions
{
    public class Gym13DbContextExtension : IDesignTimeDbContextFactory<Gym13DbContext>
    {
        public Gym13DbContext CreateDbContext(string[] args)
        {
            var connectionString = "User ID=doadmin;Password=AVNS_9bS5Alo9CWuLc_OJ7Yf;Host=gym13-db-do-user-14832054-0.c.db.ondigitalocean.com;Port=25060;Database=defaultdb;";

            var optionsBuilder = new DbContextOptionsBuilder<Gym13DbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new Gym13DbContext(optionsBuilder.Options);
        }
    }
}
