using Gym13.Application.Models;
using Gym13.Application.Models.Banner;

namespace Gym13.Application.Interfaces
{
    public interface IBannerService
    {
        Task<List<BannerModel>> GetBannerList();
        Task<BannerModel?> GetBanner(int id);
        Task<BaseResponseModel> AddBanner(BannerModel request);
        Task<BaseResponseModel> UpdateBanner(BannerModel request);
        Task<BaseResponseModel> DeleteBanner(int id);
        Task<List<BannerResponseModel>> GetBanners();
    }
}
