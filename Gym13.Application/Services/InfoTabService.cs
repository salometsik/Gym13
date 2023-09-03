using Gym13.Domain.Data;

namespace Gym13.Application.Services
{
    public class InfoTabService : BaseService
    {
        readonly Gym13DbContext _db;

        public InfoTabService(Gym13DbContext db)
        {
            _db = db;
        }


    }
}
