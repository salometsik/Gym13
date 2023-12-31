﻿using Gym13.Application.Interfaces;
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

        #region Plan
        public async Task<List<PlanModel>> GetPlans(PlanPeriodType? periodType, int? discountId)
        {
            var plans = await _db.Plans.Include(p => p.PlanServices).Include(p => p.Discount)
                .Where(x => x.IsActive && (!periodType.HasValue || x.PeriodType == periodType)
                && (!discountId.HasValue || x.DiscountId == discountId)).ToListAsync();

            var resp = plans.Select(i => new PlanModel
            {
                PlanId = i.PlanId,
                Title = i.Title,
                OriginalPrice = i.DiscountId.HasValue ? i.Price : null,
                Price = GetPlanDiscountedAmount(i.Price, i.Discount),
                PeriodNumber = i.PeriodNumber,
                PeriodType = i.PeriodType,
                HourFrom = i.HourFrom,
                HourTo = i.HourTo,
                PlanServices = i.PlanServices.Select(s => new PlanServicesItem
                {
                    Id = s.PlanServiceId,
                    Title = new TextLocalization(s.Title)
                }).ToList()
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
            var plan = await _db.Plans.Include(p => p.PlanServices).Include(p => p.Discount).FirstOrDefaultAsync(x => x.PlanId == id);
            if (plan == null)
                return null;
            var planModel = new PlanModel
            {
                PlanId = plan.PlanId,
                Title = plan.Title,
                OriginalPrice = plan.DiscountId.HasValue ? plan.Price : null,
                Price = GetPlanDiscountedAmount(plan.Price, plan.Discount),
                PeriodNumber = plan.PeriodNumber,
                PeriodType = plan.PeriodType,
                HourFrom = plan.HourFrom,
                HourTo = plan.HourTo,
                PlanServices = plan.PlanServices.Select(i => new PlanServicesItem
                {
                    Id = i.PlanServiceId,
                    Title = new TextLocalization(i.Title).KA
                }).ToList()
            };
            return planModel;
        }

        public async Task UpdatePlan(PlanModel request)
        {
            var plan = await _db.Plans.FirstOrDefaultAsync(x => x.PlanId == request.PlanId);
            if (plan != null)
            {
                plan.Title = request.Title;
                plan.Price = request.Price;
                plan.PeriodNumber = request.PeriodNumber;
                plan.PeriodType = request.PeriodType;
                plan.HourFrom = request.HourFrom;
                plan.HourTo = request.HourTo;
                plan.UpdateDate = DateTime.UtcNow.AddHours(4);
                plan.PlanServiceIds = string.Join(',', request.PlanServices.Select(i => i.Id));
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
                TitleKa = new TextLocalization(i.Title).KA,
                TitleEn = new TextLocalization(i.Title).EN
            }).ToList();
            return resp;
        }

        public async Task AddPlanService(PlanServiceModel request)
        {
            var planService = new Domain.Models.PlanService
            {
                Title = TextLocalization.Create(request.TitleKa, request.TitleEn).SerializedText
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
                TitleKa = new TextLocalization(planService.Title).KA,
                TitleEn = new TextLocalization(planService.Title).EN
            };
            return resp;
        }

        public async Task UpdatePlanService(PlanServiceModel request)
        {
            var planService = await _db.PlanServices.FirstOrDefaultAsync(p => p.PlanServiceId == request.PlanServiceId);
            if (planService != null)
            {
                planService.Title = TextLocalization.Create(request.TitleKa, request.TitleEn).SerializedText;
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
        public static decimal GetPlanDiscountedAmount(decimal price, Discount discount)
        {
            var now = DateTime.UtcNow.AddHours(4);
            if (discount != null && (discount.IsActive || (now >= discount.StartDate && now <= discount.EndDate)))
                return discount.Type switch
                {
                    DiscountType.Amount => price - discount.Amount,
                    DiscountType.Percent => Math.Round(price - price * discount.Amount / 100, 2)
                };
            return price;
        }
    }
}
