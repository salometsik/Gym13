using Gym13.Application.Models.Trainer;

namespace Gym13.Application.Interfaces
{
    public interface ITrainerService
    {
        Task<List<TrainerModel>> GetTrainers();
        Task AddTrainer(TrainerModel request);
        Task<TrainerModel?> GetTrainer(int id);
        Task UpdateTrainer(TrainerModel request);
        Task DeleteTrainer(int id);
        Task ReactivateTrainer(int id);
        Task DeactivateTrainer(int id);
    }
}
