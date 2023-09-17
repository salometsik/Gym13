﻿using Gym13.Application.Models.InfoTab;
using Gym13.Application.Models;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Gym13.Application.Interfaces;

namespace Gym13.Application.Services
{
    public class InfoTabService : BaseService, IInfoTabService
    {
        readonly Gym13DbContext _db;

        public InfoTabService(Gym13DbContext db)
        {
            _db = db;
        }

        #region Manage
        public async Task<BaseResponseModel> AddInfoTab(InfoTabModel request)
        {
            if (_db.InfoTabs.Any(i => i.Order == request.Order))
                return Fail<BaseResponseModel>(message: "ასეთი ორდერით info tab უკვე არსებობს");
            var intoTab = new InfoTab
            {
                Title = TextLocalization.Create(request.TitleKa, request.TitleEn).SerializedText,
                Description = TextLocalization.Create(request.DescriptionKa, request.DescriptionEn).SerializedText,
                Order = request.Order
            };
            await _db.InfoTabs.AddAsync(intoTab);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }

        public async Task<InfoTabModel?> GetInfoTab(int id)
        {
            var intoTab = await _db.InfoTabs.FirstOrDefaultAsync(x => x.InfoTabId == id);
            if (intoTab == null)
                return null;
            var model = new InfoTabModel
            {
                InfoTabId = intoTab.InfoTabId,
                TitleKa = new TextLocalization(intoTab.Title).KA,
                TitleEn = new TextLocalization(intoTab.Title).EN,
                DescriptionKa = new TextLocalization(intoTab.Description).KA,
                DescriptionEn = new TextLocalization(intoTab.Description).EN
            };
            return model;
        }

        public async Task<BaseResponseModel> UpdateInfoTab(InfoTabModel request)
        {
            var intoTab = await _db.InfoTabs.FirstOrDefaultAsync(x => x.InfoTabId == request.InfoTabId);
            if (intoTab == null)
                return Fail<BaseResponseModel>(message: "info tab ვერ მოიძებნა");
            if (intoTab.Order != request.Order && _db.InfoTabs.Any(i => i.Order == request.Order))
                return Fail<BaseResponseModel>(message: "ასეთი ორდერით into tab უკვე არსებობს");
            intoTab = new InfoTab
            {
                Title = TextLocalization.Create(request.TitleKa, request.TitleEn).SerializedText,
                Description = TextLocalization.Create(request.DescriptionKa, request.DescriptionEn).SerializedText
            };
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> DeleteInfoTab(int id)
        {
            var intoTab = await _db.InfoTabs.FirstOrDefaultAsync(x => x.InfoTabId == id);
            if (intoTab == null)
                return Fail<BaseResponseModel>(message: "into tab ვერ მოიძებნა");
            _db.InfoTabs.Remove(intoTab);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }
        #endregion

        public async Task<List<InfoTabModel>> GetInfoTabList(int? pageSize)
        {
            var infoTabs = await _db.InfoTabs.ToListAsync();
            if (pageSize.HasValue)
                infoTabs = infoTabs.OrderBy(i => i.Order).Take(pageSize.Value).ToList();
            else
                infoTabs = infoTabs.OrderByDescending(i => i.InfoTabId).ToList();
            var resp = infoTabs.Select(i => new InfoTabModel
            {
                InfoTabId = i.InfoTabId,
                TitleKa = new TextLocalization(i.Title).KA,
                TitleEn = new TextLocalization(i.Title).EN,
                DescriptionKa = new TextLocalization(i.Description).KA,
                DescriptionEn = new TextLocalization(i.Description).EN,
                Order = i.Order
            }).ToList();
            return resp;
        }

    }
}
