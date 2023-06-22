using Gym13.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Domain.Data
{
    public class Gym13DbContext : DbContext
    {
        public Gym13DbContext(DbContextOptions<Gym13DbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
