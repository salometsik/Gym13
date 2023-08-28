using Gym13.Application.Models.Plan;
using Gym13.Common.Enums;
using Gym13.Domain;
using Gym13.Domain.Models;

namespace Gym13.Application.Interfaces
{
    public interface IPlanService
    {
        Task<List<PlanModel>> GetPlans(bool? unlimited, PlanPeriodType? periodType);
        Task AddPlan(PlanModel request);
        Task<PlanModel?> GetPlan(int id);
        Task UpdatePlan(PlanModel request);
        Task DeletePlan(int id);
        Task ReactivatePlan(int id);
        Task DeactivatePlan(int id);
    }
}
