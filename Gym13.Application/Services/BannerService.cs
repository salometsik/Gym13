using Gym13.Application.Models.Banner;
using Gym13.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Application.Services
{
    public class BannerService
    {
        readonly Gym13DbContext _db;

        public BannerService(Gym13DbContext db)
        {
            _db = db;
        }

        public async Task<List<BannerModel>> GetBanners()
        {
            var banners = _db.Banners.ToList();
            var resp = banners.Select(i => new BannerModel
            {
                BannerId = i.BannerId,
                TitleKa = new TextLocalization(i.Title).KA,
                TitleEn = new TextLocalization(i.Title).EN,
                DescriptionKa = new TextLocalization(i.Description).KA,
                DescriptionEn = new TextLocalization(i.Description).EN,
                ImageUrl = i.ImageUrl,
                Order = i.Order
            }).ToList();
            return resp;
        }

        public async Task<BannerModel?> GetBanner(int id)
        {
            var banner = await _db.Banners.FirstOrDefaultAsync(b => b.BannerId == id);
            if (banner == null)
                return null;
            var response = new BannerModel
            {
                BannerId = banner.BannerId,
                TitleKa = new TextLocalization(banner.Title).KA,
                TitleEn = new TextLocalization(banner.Title).EN,
                DescriptionKa = new TextLocalization(banner.Description).KA,
                DescriptionEn = new TextLocalization(banner.Description).EN,
                ImageUrl = banner.ImageUrl,
                Order = banner.Order
            };
            return response;
        }
    }
}
