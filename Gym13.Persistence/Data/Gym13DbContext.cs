using Microsoft.EntityFrameworkCore;

namespace Gym13.Persistence.Data
{
    public class Gym13DbContext : DbContext
    {
        public Gym13DbContext(DbContextOptions<Gym13DbContext> options) : base(options)
        {

        }

    }
}
