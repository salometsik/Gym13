using Gym13.Application.Interfaces;
using Gym13.Application.Models;
using Gym13.Application.Models.Banner;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Application.Services
{
    public class BannerService : BaseService, IBannerService
    {
        readonly Gym13DbContext _db;

        public BannerService(Gym13DbContext db)
        {
            _db = db;
        }

        #region Manage
        public async Task<List<BannerModel>> GetBannerList()
        {
            var banners = _db.Banners.OrderBy(b => b.BannerId).ToList();
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

        public async Task<BaseResponseModel> AddBanner(BannerModel request)
        {
            if (_db.Banners.Any(b => b.Order == request.Order))
                return Fail<BaseResponseModel>(message: "ასეთი ორდერით ბანერი უკვე არსებობს");
            var banner = new Banner
            {
                Title = TextLocalization.Create(request.TitleKa, request.TitleEn),
                Description = TextLocalization.Create(request.DescriptionKa, request.DescriptionEn),
                Order = request.Order,
                ImageUrl = request.ImageUrl
            };
            _db.Banners.Add(banner);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> UpdateBanner(BannerModel request)
        {
            var banner = await _db.Banners.FirstOrDefaultAsync(b => b.BannerId == request.BannerId);
            if (banner == null)
                return Fail<BaseResponseModel>(message: "ბანერი ვერ მოიძებნა");
            if (banner.Order != request.Order && _db.Banners.Any(b => b.Order == request.Order))
                return Fail<BaseResponseModel>(message: "ასეთი ორდერით ბანერი უკვე არსებობს");
            banner.Title = TextLocalization.Create(request.TitleKa, request.TitleEn);
            banner.Description = TextLocalization.Create(request.DescriptionKa, request.DescriptionEn);
            banner.Order = request.Order;
            banner.ImageUrl = request.ImageUrl;
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> DeleteBanner(int id)
        {
            var banner = await _db.Banners.FirstOrDefaultAsync(b => b.BannerId == id);
            if (banner == null)
                return Fail<BaseResponseModel>(message: "ბანერი ვერ მოიძებნა");
            _db.Banners.Remove(banner);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }
        #endregion
        public async Task<List<BannerResponseModel>> GetBanners()
        {
            var banners = _db.Banners.ToList().OrderBy(b => b.Order).Take(5);
            var response = banners.Select(i => new BannerResponseModel
            {
                BannerId = i.BannerId,
                ImageUrl = i.ImageUrl,
                Title = new TextLocalization(i.Title),
                Description = new TextLocalization(i.Description)
            }).ToList();
            return response;
        }
    }
}
