using AutoMapper;
using Gym13.Application.Interfaces;
using Gym13.Application.Models.Trainer;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using System.Data.Entity;

namespace Gym13.Application.Services
{
    public class TrainerService : ITrainerService
    {
        readonly Gym13DbContext _db;
        readonly IMapper _mapper;

        public TrainerService(Gym13DbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
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
            var trainer = _mapper.Map<Trainer>(request);
            await _db.Trainers.AddAsync(trainer);
            await _db.SaveChangesAsync();
        }

        public async Task<TrainerModel> GetTrainer(int id)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == id);
            return _mapper.Map<TrainerModel>(trainer);
        }

        public async Task UpdateTrainer(TrainerModel request)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(x => x.TrainerId == request.TrainerId);
            if (trainer != null)
            {
                trainer = _mapper.Map<Trainer>(request);
                trainer.UpdateDate = DateTime.UtcNow.AddHours(4);
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
