using Gym13.Application.Interfaces;
using Gym13.Application.Models.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PlanServiceController : ControllerBase
    {
        readonly IPlanService _planService;

        public PlanServiceController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpGet("list")]
        public async Task<List<PlanServiceModel>> GetPlanServices()
            => await _planService.GetPlanServices();

        [HttpPost]
        public async Task AddPlanService(PlanServiceModel planService) => await _planService.AddPlanService(planService);

        [HttpGet]
        public async Task<PlanServiceModel?> GetPlanService(int id) => await _planService.GetPlanService(id);

        [HttpPut]
        public async Task UpdatePlanService(PlanServiceModel request) => await _planService.UpdatePlanService(request);

        [HttpDelete]
        public async Task DeletePlanService(int id) => await _planService.DeletePlanService(id);
    }
}
