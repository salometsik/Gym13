using Gym13.Application.Models.Discount;
using Gym13.Application.Models;

namespace Gym13.Application.Interfaces
{
    public interface IDiscountService
    {
        Task<List<DiscountModel>> GetDiscountList();
        Task<DiscountModel?> GetDiscount(int id);
        Task<BaseResponseModel> CreateDiscount(DiscountModel request);
        Task<BaseResponseModel> UpdateDiscount(DiscountModel request);
        Task<BaseResponseModel> DeleteDiscount(int id);
        Task<BaseResponseModel> ChangeDiscountActiveStatus(int id);
    }
}
