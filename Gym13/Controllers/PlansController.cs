using Gym13.Application.Interfaces;
using Gym13.Application.Models.Plan;
using Gym13.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PlansController : ControllerBase
    {
        readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpGet("list")]
        public async Task<List<PlanModel>> GetPlans(PlanPeriodType? periodType)
            => await _planService.GetPlans(periodType);

        [HttpGet]
        public async Task<PlanModel?> GetPlan(int id) => await _planService.GetPlan(id);

        [HttpPost]
        public async Task AddPlan(PlanModel plan) => await _planService.AddPlan(plan);

        [HttpPut]
        public async Task UpdatePlan(PlanModel request) => await _planService.UpdatePlan(request);

        [HttpPatch("deactivate")]
        public async Task DeactivatePlan(int id) => await _planService.DeactivatePlan(id);

        [HttpPatch("reactivate")]
        public async Task ReactivatePlan(int id) => await _planService.ReactivatePlan(id);

        [HttpDelete]
        public async Task DeletePlan(int id) => await _planService.DeletePlan(id);
    }
}
