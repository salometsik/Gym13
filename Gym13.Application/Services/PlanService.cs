using AutoMapper;
using Gym13.Application.Interfaces;
using Gym13.Application.Models.Plan;
using Gym13.Common.Enums;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Application.Services
{
    public class PlanService : IPlanService
    {
        readonly Gym13DbContext _db;

        public PlanService(Gym13DbContext db)
        {
            _db = db;
        }

        public async Task<List<PlanModel>> GetPlans(bool? unlimited, PlanPeriodType? periodType)
        {
            var plans = await _db.Plans.Where(x => x.IsActive
                && (unlimited.HasValue || x.IsUnlimited == unlimited)
                && (periodType.HasValue || x.PeriodType == periodType)).ToListAsync();

            var resp = plans.Select(i => new PlanModel
            {
                PlanId = i.PlanId,
                Title = i.Title,
                Price = i.Price,
                PeriodNumber = i.PeriodNumber,
                PeriodType = i.PeriodType,
                HourFrom = i.HourFrom,
                HourTo = i.HourTo,
                IsUnlimited = i.IsUnlimited
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
                IsUnlimited = request.IsUnlimited
            };
            await _db.Plans.AddAsync(plan);
            await _db.SaveChangesAsync();
        }

        public async Task<PlanModel?> GetPlan(int id)
        {
            var plan = await _db.Plans.FirstOrDefaultAsync(x => x.PlanId == id);
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
                IsUnlimited = plan.IsUnlimited
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
                    IsUnlimited = request.IsUnlimited,
                    UpdateDate = DateTime.UtcNow.AddHours(4)
                };
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
    }
}
