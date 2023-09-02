using Gym13.Application.Interfaces;
using Gym13.Application.Models.Plan;
using Gym13.Common.Enums;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gym13.Application.Services
{
    public class PlanService : IPlanService
    {
        readonly Gym13DbContext _db;

        public PlanService(Gym13DbContext db)
        {
            _db = db;
        }

        #region Plan
        public async Task<List<PlanModel>> GetPlans(PlanPeriodType? periodType)
        {
            var plans = await _db.Plans.Where(x => x.IsActive
                && (periodType.HasValue || x.PeriodType == periodType)).ToListAsync();

            var resp = plans.Select(i => new PlanModel
            {
                PlanId = i.PlanId,
                Title = i.Title,
                Price = i.Price,
                PeriodNumber = i.PeriodNumber,
                PeriodType = i.PeriodType,
                HourFrom = i.HourFrom,
                HourTo = i.HourTo
            }).ToList();

            return resp;
        }

        public async Task AddPlan(PlanModel request)
        {
            var plan = new Plan
            {
                Title = request.Title,
                Price = request.Price,
                PeriodNumber = request.PeriodNumber,
                PeriodType = request.PeriodType,
                HourFrom = request.HourFrom,
                HourTo = request.HourTo,
                PlanServiceIds = string.Join(',', request.PlanServices.Select(i => i.Id))
            };
            await _db.Plans.AddAsync(plan);
            await _db.SaveChangesAsync();
        }

        public async Task<PlanModel?> GetPlan(int id)
        {
            var plan = await _db.Plans.Include(p => p.PlanServices).FirstOrDefaultAsync(x => x.PlanId == id);
            if (plan == null)
                return null;
            var planModel = new PlanModel
            {
                PlanId = plan.PlanId,
                Title = plan.Title,
                Price = plan.Price,
                PeriodNumber = plan.PeriodNumber,
                PeriodType = plan.PeriodType,
                HourFrom = plan.HourFrom,
                HourTo = plan.HourTo,
                PlanServices = plan.PlanServices.Select(i => new PlanServicesItem
                {
                    Id = i.PlanServiceId,
                    Title = JsonConvert.DeserializeObject<TextLocalization>(i.Title).KA
                }).ToList()
            };
            return planModel;
        }

        public async Task UpdatePlan(PlanModel request)
        {
            var plan = await _db.Plans.FirstOrDefaultAsync(x => x.PlanId == request.PlanId);
            if (plan != null)
            {
                plan = new Plan
                {
                    Title = request.Title,
                    Price = request.Price,
                    PeriodNumber = request.PeriodNumber,
                    PeriodType = request.PeriodType,
                    HourFrom = request.HourFrom,
                    HourTo = request.HourTo,
                    UpdateDate = DateTime.UtcNow.AddHours(4),
                    PlanServiceIds = string.Join(',', request.PlanServices.Select(i => i.Id))
                };
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeactivatePlan(int id)
        {
            var plan = await _db.Plans.FirstOrDefaultAsync(x => x.PlanId == id);
            if (plan != null)
            {
                plan.IsActive = false;
                plan.UpdateDate = DateTime.UtcNow.AddHours(4);
                await _db.SaveChangesAsync();
            }
        }

        public async Task ReactivatePlan(int id)
        {
            var plan = await _db.Plans.FirstOrDefaultAsync(x => x.PlanId == id);
            if (plan != null)
            {
                plan.IsActive = true;
                plan.UpdateDate = DateTime.UtcNow.AddHours(4);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeletePlan(int id)
        {
            var plan = await _db.Plans.FirstOrDefaultAsync(x => x.PlanId == id);
            if (plan != null)
            {
                _db.Plans.Remove(plan);
                await _db.SaveChangesAsync();
            }
        }
        #endregion
        #region Plan Services
        public async Task<List<PlanServiceModel>> GetPlanServices()
        {
            var planServices = _db.PlanServices.ToList();
            var resp = planServices.Select(i => new PlanServiceModel
            {
                PlanServiceId = i.PlanServiceId,
                TitleKa = JsonConvert.DeserializeObject<TextLocalization>(i.Title).KA,
                TitleEn = JsonConvert.DeserializeObject<TextLocalization>(i.Title).EN
            }).ToList();
            return resp;
        }

        public async Task AddPlanService(PlanServiceModel request)
        {
            var planService = new Domain.Models.PlanService
            {
                Title = TextLocalization.Create(request.TitleKa, request.TitleEn)
            };
            _db.PlanServices.Add(planService);
            await _db.SaveChangesAsync();
        }

        public async Task<PlanServiceModel> GetPlanService(int id)
        {
            var planService = await _db.PlanServices.FirstOrDefaultAsync(p => p.PlanServiceId == id);
            var resp = new PlanServiceModel
            {
                PlanServiceId = planService.PlanServiceId,
                TitleKa = JsonConvert.DeserializeObject<TextLocalization>(planService.Title).KA,
                TitleEn = JsonConvert.DeserializeObject<TextLocalization>(planService.Title).EN
            };
            return resp;
        }

        public async Task UpdatePlanService(PlanServiceModel request)
        {
            var planService = await _db.PlanServices.FirstOrDefaultAsync(p => p.PlanServiceId == request.PlanServiceId);
            if (planService != null)
            {
                planService.Title = TextLocalization.Create(request.TitleKa, request.TitleEn);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeletePlanService(int id)
        {
            var planService = await _db.PlanServices.FirstOrDefaultAsync(p => p.PlanServiceId == id);
            if (planService != null)
            {
                _db.PlanServices.Remove(planService);
                await _db.SaveChangesAsync();
            }
        }

        #endregion
    }
}
