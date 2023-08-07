using AutoMapper;
using Gym13.Application.Interfaces;
using Gym13.Application.Models.Plan;
using Gym13.Common.Enums;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using System.Data.Entity;

namespace Gym13.Infrastructure.Services
{
    public class PlanService : IPlanService
    {
        readonly Gym13DbContext _db;
        readonly IMapper _mapper;

        public PlanService(Gym13DbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
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
            var plan = _mapper.Map<Plan>(request);
            await _db.Plans.AddAsync(plan);
            await _db.SaveChangesAsync();
        }

        public async Task<PlanModel> GetPlan(int id)
        {
            var plan = await _db.Plans.FirstOrDefaultAsync(x => x.PlanId == id);
            return _mapper.Map<PlanModel>(plan);
        }

        public async Task UpdatePlan(PlanModel request)
        {
            var plan = await _db.Plans.FirstOrDefaultAsync(x => x.PlanId == request.PlanId);
            if (plan != null)
            {
                plan = _mapper.Map<Plan>(request);
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
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeactivatePlan(int id)
        {
            var plan = await _db.Plans.FirstOrDefaultAsync(x => x.PlanId == id);
            if (plan != null)
            {
                plan.IsActive = false;
                await _db.SaveChangesAsync();
            }
        }
    }
}
