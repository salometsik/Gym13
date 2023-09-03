﻿using Gym13.Application.Interfaces;
using Gym13.Application.Models.Trainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
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
        public async Task<TrainerModel?> GetPlan(int id) => await _trainerService.GetTrainer(id);

        [HttpPost]
        public async Task AddPlan(TrainerModel plan) => await _trainerService.AddTrainer(plan);

        [HttpPut]
        public async Task UpdatePlan(TrainerModel request) => await _trainerService.UpdateTrainer(request);

        [HttpPatch("deactivate")]
        public async Task DeactivatePlan(int id) => await _trainerService.DeactivateTrainer(id);

        [HttpPatch("reactivate")]
        public async Task ReactivatePlan(int id) => await _trainerService.ReactivateTrainer(id);

        [HttpDelete]
        public async Task DeletePlan(int id) => await _trainerService.DeleteTrainer(id);
        #endregion
    }
}
