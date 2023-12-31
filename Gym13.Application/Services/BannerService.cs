﻿using Gym13.Application.Interfaces;
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
            var banners = _db.Banners.Where(b => !b.IsDeleted).OrderByDescending(b => b.BannerId).ToList();
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
            var banner = await _db.Banners.FirstOrDefaultAsync(b => b.BannerId == id && !b.IsDeleted);
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

        public async Task<BaseResponseModel> CreateBanner(BannerModel request)
        {
            if (_db.Banners.Any(b => b.Order == request.Order))
                return Fail<BaseResponseModel>(message: "ასეთი ორდერით ბანერი უკვე არსებობს");
            var banner = new Banner
            {
                Title = TextLocalization.Create(request.TitleKa, request.TitleEn).SerializedText,
                Description = TextLocalization.Create(request.DescriptionKa, request.DescriptionEn).SerializedText,
                Order = request.Order,
                ImageUrl = request.ImageUrl
            };
            _db.Banners.Add(banner);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> UpdateBanner(BannerModel request)
        {
            var banner = await _db.Banners.FirstOrDefaultAsync(b => b.BannerId == request.BannerId && !b.IsDeleted);
            if (banner == null)
                return Fail<BaseResponseModel>(message: "ბანერი ვერ მოიძებნა");
            if (banner.Order != request.Order && _db.Banners.Any(b => b.Order == request.Order))
                return Fail<BaseResponseModel>(message: "ასეთი ორდერით ბანერი უკვე არსებობს");
            await InsertBannerUpdateEntityHistory(banner, request);
            banner.Title = TextLocalization.Create(request.TitleKa, request.TitleEn).SerializedText;
            banner.Description = TextLocalization.Create(request.DescriptionKa, request.DescriptionEn).SerializedText;
            banner.Order = request.Order;
            banner.ImageUrl = request.ImageUrl;
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> DeleteBanner(int id)
        {
            var banner = await _db.Banners.FirstOrDefaultAsync(b => b.BannerId == id && !b.IsDeleted);
            if (banner == null)
                return Fail<BaseResponseModel>(message: "ბანერი ვერ მოიძებნა");
            banner.IsDeleted = true;
            InsertBannerDeleteEntityHistory(id);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }
        #endregion
        public async Task<List<BannerResponseModel>> GetBanners()
        {
            var banners = _db.Banners.Where(b => !b.IsDeleted).ToList().OrderBy(b => b.Order).Take(5);
            var response = banners.Select(i => new BannerResponseModel
            {
                BannerId = i.BannerId,
                ImageUrl = i.ImageUrl,
                Title = new TextLocalization(i.Title),
                Description = new TextLocalization(i.Description)
            }).ToList();
            return response;
        }

        async Task InsertBannerUpdateEntityHistory(Banner banner, BannerModel request)
        {
            var histories = new List<EntityHistory>();
            var titleKa = new TextLocalization(banner.Title).KA;
            var titleEn = new TextLocalization(banner.Title).EN;
            if (titleKa != request.TitleKa)
                histories.Add(new EntityHistory
                {
                    ActionType = Common.Enums.EntityActionType.Updated,
                    Table = "Banners",
                    Column = nameof(banner.Title),
                    OldValue = titleKa,
                    NewValue = request.TitleKa
                });
            if (titleEn != request.TitleEn)
                histories.Add(new EntityHistory
                {
                    ActionType = Common.Enums.EntityActionType.Updated,
                    Table = "Banners",
                    Column = nameof(banner.Title),
                    OldValue = titleEn,
                    NewValue = request.TitleEn
                });
            var descriptionKa = new TextLocalization(banner.Description).KA;
            var descriptionEn = new TextLocalization(banner.Description).EN;
            if (descriptionKa != request.DescriptionKa)
                histories.Add(new EntityHistory
                {
                    ActionType = Common.Enums.EntityActionType.Updated,
                    Table = "Banners",
                    Column = nameof(banner.Description),
                    OldValue = descriptionKa,
                    NewValue = request.DescriptionKa
                });
            if (descriptionEn != request.DescriptionEn)
                histories.Add(new EntityHistory
                {
                    ActionType = Common.Enums.EntityActionType.Updated,
                    Table = "Banners",
                    Column = nameof(banner.Description),
                    OldValue = descriptionEn,
                    NewValue = request.DescriptionEn
                });
            if (banner.Order != request.Order)
                histories.Add(new EntityHistory
                {
                    ActionType = Common.Enums.EntityActionType.Updated,
                    Table = "Banners",
                    Column = nameof(banner.Order),
                    OldValue = banner.Order.ToString(),
                    NewValue = request.Order.ToString()
                });
            if (banner.ImageUrl != request.ImageUrl)
                histories.Add(new EntityHistory
                {
                    ActionType = Common.Enums.EntityActionType.Updated,
                    Table = "Banners",
                    Column = nameof(banner.ImageUrl),
                    OldValue = banner.ImageUrl,
                    NewValue = request.ImageUrl
                });
            if (histories.Count > 0)
            {
                _db.EntityHistories.AddRange(histories);
                await _db.SaveChangesAsync();
            }
        }
        void InsertBannerDeleteEntityHistory(int id)
        {
            _db.EntityHistories.Add(new EntityHistory
            {
                ActionType = Common.Enums.EntityActionType.Deleted,
                Table = "Banners",
                Column = nameof(Banner.BannerId),
                OldValue = id.ToString()
            });
        }
    }
}
