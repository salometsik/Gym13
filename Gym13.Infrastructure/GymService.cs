using Gym13.Application;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Infrastructure
{
    public class GymService : IGymService
    {
        readonly Gym13DbContext _db;

        public GymService(Gym13DbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetUser(string userId)
            => await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }
}
