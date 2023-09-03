using Gym13.Application.Interfaces;
using Gym13.Application.Models;
using Gym13.Application.Models.Banner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BannerController : ControllerBase
    {
        readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet("list")]
        public async Task<List<BannerResponseModel>> GetBanners() => await _bannerService.GetBanners();
        #region Manage
        [HttpGet("manage-list")]
        public async Task<List<BannerModel>> GetBannerList() => await _bannerService.GetBannerList();

        [HttpGet]
        public async Task<BannerModel?> GetBanner(int id) => await _bannerService.GetBanner(id);

        [HttpPost]
        public async Task<BaseResponseModel> CreateBanner(BannerModel request)
            => await _bannerService.CreateBanner(request);

        [HttpPut]
        public async Task<BaseResponseModel> UpdateBanner(BannerModel request)
            => await _bannerService.UpdateBanner(request);

        [HttpDelete]
        public async Task DeleteBanner(int id) => await _bannerService.DeleteBanner(id);
        #endregion
    }
}
