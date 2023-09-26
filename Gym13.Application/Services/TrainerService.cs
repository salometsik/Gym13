using AutoMapper;
using Gym13.Application.Interfaces;
using Gym13.Application.Models;
using Gym13.Application.Models.Trainer;
using Gym13.Common.Resources;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Application.Services
{
    public class TrainerService : BaseService, ITrainerService
    {
        readonly Gym13DbContext _db;

        public TrainerService(Gym13DbContext db)
        {
            _db = db;
        }

        #region Manage
        public async Task<BaseResponseModel> AddTrainer(TrainerModel request)
        {
            if (_db.Trainers.Any(t => t.Order == request.Order))
                return Fail<BaseResponseModel>(message: "ასეთი ორდერით ტრენერი უკვე არსებობს");
            var trainer = new Trainer
            {
                Name = request.Name,
                FacebookUrl = request.FacebookUrl,
                InstagramUrl = request.InstagramUrl,
                TwitterUrl = request.TwitterUrl,
                ImageUrl = request.ImageUrl,
                Order = request.Order,
                Description = TextLocalization.Create(request.DescriptionKa, request.DescriptionEn).SerializedText
            };
            await _db.Trainers.AddAsync(trainer);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }

        public async Task<TrainerModel?> GetTrainer(int id)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == id);
            if (trainer == null)
                return null;
            var model = new TrainerModel
            {
                TrainerId = trainer.TrainerId,
                Name = trainer.Name,
                FacebookUrl = trainer.FacebookUrl,
                InstagramUrl = trainer.InstagramUrl,
                TwitterUrl = trainer.TwitterUrl,
                ImageUrl = trainer.ImageUrl,
                DescriptionKa = new TextLocalization(trainer.Description).KA,
                DescriptionEn = new TextLocalization(trainer.Description).EN
            };
            return model;
        }

        public async Task<BaseResponseModel> UpdateTrainer(TrainerModel request)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == request.TrainerId);
            if (trainer == null)
                return Fail<BaseResponseModel>(message: "ტრენერი ვერ მოიძებნა");
            if (trainer.Order != request.Order && _db.Trainers.Any(t => t.Order == request.Order))
                return Fail<BaseResponseModel>(message: "ასეთი ორდერით ტრენერი უკვე არსებობს");
            trainer.Name = request.Name;
            trainer.FacebookUrl = request.FacebookUrl;
            trainer.InstagramUrl = request.InstagramUrl;
            trainer.TwitterUrl = request.TwitterUrl;
            trainer.ImageUrl = request.ImageUrl;
            trainer.UpdateDate = DateTime.UtcNow.AddHours(4);
            trainer.Description = TextLocalization.Create(request.DescriptionKa, request.DescriptionEn).SerializedText;
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> DeleteTrainer(int id)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == id);
            if (trainer == null)
                return Fail<BaseResponseModel>(message: "ტრენერი ვერ მოიძებნა");
            _db.Trainers.Remove(trainer);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> ReactivateTrainer(int id)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == id);
            if (trainer == null)
                return Fail<BaseResponseModel>(message: "ტრენერი ვერ მოიძებნა");
            trainer.IsActive = true;
            trainer.UpdateDate = DateTime.UtcNow.AddHours(4);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> DeactivateTrainer(int id)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == id);
            if (trainer == null)
                return Fail<BaseResponseModel>(message: "ტრენერი ვერ მოიძებნა");
            trainer.IsActive = false;
            trainer.UpdateDate = DateTime.UtcNow.AddHours(4);
            await _db.SaveChangesAsync();
            return Success<BaseResponseModel>();
        }
        #endregion
        public async Task<List<TrainerModel>> GetTrainerList(int? pageSize)
        {
            var trainers = await _db.Trainers.Where(t => t.IsActive).ToListAsync();
            if (pageSize.HasValue)
                trainers = trainers.OrderBy(t => t.Order).Take(pageSize.Value).ToList();
            else
                trainers = trainers.OrderByDescending(t => t.TrainerId).ToList();
            var resp = trainers.Select(i => new TrainerModel
            {
                TrainerId = i.TrainerId,
                Name = i.Name,
                Order = i.Order,
                ImageUrl = i.ImageUrl,
                FacebookUrl = i.FacebookUrl,
                InstagramUrl = i.InstagramUrl,
                TwitterUrl = i.TwitterUrl
            }).ToList();
            return resp;
        }

        public async Task<TrainerResponseModel> GetTrainerDetails(int id)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(t => t.TrainerId == id && t.IsActive);
            if (trainer == null)
                return Fail<TrainerResponseModel>(message: Gym13Resources.RecordNotFound);
            var response = new TrainerResponseModel
            {
                TrainerId = trainer.TrainerId,
                Name = trainer.Name,
                FacebookUrl = trainer.FacebookUrl,
                InstagramUrl = trainer.InstagramUrl,
                TwitterUrl = trainer.TwitterUrl,
                ImageUrl = trainer.ImageUrl,
                Description = new TextLocalization(trainer.Description)
            };
            return Success(response);
        }
    }
}
