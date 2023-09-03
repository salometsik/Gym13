using Gym13.Application.Models;
using Gym13.Application.Models.Trainer;

namespace Gym13.Application.Interfaces
{
    public interface ITrainerService
    {
        Task<List<TrainerModel>> GetTrainerList(int? pageSize);
        Task<TrainerResponseModel> GetTrainerDetails(int id);
        Task<BaseResponseModel> AddTrainer(TrainerModel request);
        Task<TrainerModel?> GetTrainer(int id);
        Task<BaseResponseModel> UpdateTrainer(TrainerModel request);
        Task<BaseResponseModel> DeleteTrainer(int id);
        Task<BaseResponseModel> ReactivateTrainer(int id);
        Task<BaseResponseModel> DeactivateTrainer(int id);
    }
}
