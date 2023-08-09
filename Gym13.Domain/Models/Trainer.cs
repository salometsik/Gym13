using AutoMapper;

namespace Gym13.Domain.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        [IgnoreMap]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow.AddHours(4);
        [IgnoreMap]
        public DateTime? UpdateDate { get; set; }
        public string Name { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
