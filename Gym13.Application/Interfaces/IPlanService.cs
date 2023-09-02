using Gym13.Application.Models.Plan;
using Gym13.Common.Enums;

namespace Gym13.Application.Interfaces
{
    public interface IPlanService
    {
        #region Plan
        Task<List<PlanModel>> GetPlans(PlanPeriodType? periodType);
        Task AddPlan(PlanModel request);
        Task<PlanModel?> GetPlan(int id);
        Task UpdatePlan(PlanModel request);
        Task ReactivatePlan(int id);
        Task DeactivatePlan(int id);
        Task DeletePlan(int id);
        #endregion
        #region Plan Service
        Task<List<PlanServiceModel>> GetPlanServices();
        Task AddPlanService(PlanServiceModel request);
        Task<PlanServiceModel> GetPlanService(int id);
        Task UpdatePlanService(PlanServiceModel request);
        Task DeletePlanService(int id);
        #endregion
    }
}
