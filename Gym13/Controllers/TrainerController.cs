using Gym13.Application.Interfaces;
using Gym13.Application.Models;
using Gym13.Application.Models.Trainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class TrainerController : ControllerBase
    {
        readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpGet("list")]
        public async Task<List<TrainerModel>> GetTrainers(int? pageSize) => await _trainerService.GetTrainerList(pageSize);

        [HttpGet("details")]
        public async Task<TrainerResponseModel> GetTrainerDetails(int id) => await _trainerService.GetTrainerDetails(id);
        #region Manage
        [HttpGet]
        public async Task<TrainerModel?> GetTrainer(int id) => await _trainerService.GetTrainer(id);

        [HttpPost]
        public async Task<BaseResponseModel> AddTrainer(TrainerModel trainer) => await _trainerService.AddTrainer(trainer);

        [HttpPut]
        public async Task<BaseResponseModel> UpdateTrainer(TrainerModel request) => await _trainerService.UpdateTrainer(request);

        [HttpPatch("deactivate")]
        public async Task<BaseResponseModel> DeactivateTrainer(int id) => await _trainerService.DeactivateTrainer(id);

        [HttpPatch("reactivate")]
        public async Task<BaseResponseModel> ReactivateTrainer(int id) => await _trainerService.ReactivateTrainer(id);

        [HttpDelete]
        public async Task<BaseResponseModel> DeleteTrainer(int id) => await _trainerService.DeleteTrainer(id);
        #endregion
    }
}
