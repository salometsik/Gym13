using AutoMapper;
using Gym13.Application.Interfaces;
using Gym13.Application.Models.Trainer;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Application.Services
{
    public class TrainerService : ITrainerService
    {
        readonly Gym13DbContext _db;

        public TrainerService(Gym13DbContext db)
        {
            _db = db;
        }

        public async Task<List<TrainerModel>> GetTrainers()
        {
            var trainers = await _db.Trainers.ToListAsync();

            var resp = trainers.Select(i => new TrainerModel
            {
                TrainerId = i.TrainerId,
                Name = i.Name,
            }).ToList();
            return resp;

        }

        public async Task AddTrainer(TrainerModel request)
        {
            var trainer = new Trainer
            {
                Name = request.Name,
                FacebookUrl = request.FacebookUrl,
                InstagramUrl = request.InstagramUrl,
                TwitterUrl = request.TwitterUrl,
                ImageUrl = request.ImageUrl
            };
            await _db.Trainers.AddAsync(trainer);
            await _db.SaveChangesAsync();
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
                ImageUrl = trainer.ImageUrl
            };
            return model;
        }

        public async Task UpdateTrainer(TrainerModel request)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == request.TrainerId);
            if (trainer != null)
            {
                trainer = new Trainer
                {
                    Name = request.Name,
                    FacebookUrl = request.FacebookUrl,
                    InstagramUrl = request.InstagramUrl,
                    TwitterUrl = request.TwitterUrl,
                    ImageUrl = request.ImageUrl,
                    UpdateDate = DateTime.UtcNow.AddHours(4)
                };
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteTrainer(int id)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == id);
            if (trainer != null)
            {
                _db.Trainers.Remove(trainer);
                await _db.SaveChangesAsync();
            }
        }

        public async Task ReactivateTrainer(int id)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == id);
            if (trainer != null)
            {
                trainer.IsActive = true;
                trainer.UpdateDate = DateTime.UtcNow.AddHours(4);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeactivateTrainer(int id)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == id);
            if (trainer != null)
            {
                trainer.IsActive = false;
                trainer.UpdateDate = DateTime.UtcNow.AddHours(4);
                await _db.SaveChangesAsync();
            }
        }
    }
}
