using Gym13.Domain.Models;

namespace Gym13.Application.Interfaces
{
    public interface IGymService
    {
        Task<User?> GetUser(string userId);
    }
}
