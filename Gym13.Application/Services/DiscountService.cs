using Gym13.Application.Interfaces;
using Gym13.Application.Models.Discount;
using Gym13.Application.Models;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Application.Services
{
    public class DiscountService : BaseService, IDiscountService
    {
        readonly Gym13DbContext _db;

        public DiscountService(Gym13DbContext db)
        {
            _db = db;
        }

        #region Manage
        public async Task<List<DiscountModel>> GetDiscountList()
        {
            var discounts = _db.Discounts.OrderByDescending(b => b.DiscountId).ToList();
            var resp = discounts.Select(i => new DiscountModel
            {
                DiscountId = i.DiscountId,
                Title = i.Title,
                Type = i.Type,
                Amount = i.Amount,
                StartDate = i.StartDate,
                EndDate = i.EndDate,
                IsActive = i.IsActive
            }).ToList();
            return resp;
        }

        public async Task<DiscountModel?> GetDiscount(int id)
        {
            var discount = await _db.Discounts.FirstOrDefaultAsync(b => b.DiscountId == id);
            if (discount == null)
                return null;
            var response = new DiscountModel
            {
                DiscountId = discount.DiscountId,
                Title = discount.Title,
                Type = discount.Type,
                Amount = discount.Amount,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive
            };
            return response;
        }

        public async Task<BaseResponseModel> CreateDiscount(DiscountModel request)
        {
            var discount = new Discount
            {
                Title = request.Title,
                Type = request.Type,
                Amount = request.Amount,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = request.IsActive
            };
            _db.Discounts.Add(discount);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> UpdateDiscount(DiscountModel request)
        {
            var discount = await _db.Discounts.FirstOrDefaultAsync(b => b.DiscountId == request.DiscountId);
            if (discount == null)
                return Fail<BaseResponseModel>(message: "ფასდაკლება ვერ მოიძებნა");
            discount.Title = request.Title;
            discount.Type = request.Type;
            discount.Amount = request.Amount;
            discount.StartDate = request.StartDate;
            discount.EndDate = request.EndDate;
            discount.IsActive = request.IsActive;
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> DeleteDiscount(int id)
        {
            var discount = await _db.Discounts.FirstOrDefaultAsync(b => b.DiscountId == id);
            if (discount == null)
                return Fail<BaseResponseModel>(message: "ფასდაკლება ვერ მოიძებნა");
            var discountedPlans = _db.Plans.Where(p => p.DiscountId == id).ToList();
            discountedPlans.ForEach(p => { p.DiscountId = null; });
            _db.Discounts.Remove(discount);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }
        #endregion
    }
}
