using Gym13.Application.Interfaces;
using Gym13.Domain.Data;

namespace Gym13.Infrastructure.Services
{
    public class GymService : IGymService
    {
        readonly Gym13DbContext _db;

        public GymService(Gym13DbContext db)
        {
            _db = db;
        }

    }
}
