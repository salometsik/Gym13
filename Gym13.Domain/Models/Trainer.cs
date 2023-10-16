using Gym13.Domain.Models.Base;

namespace Gym13.Domain.Models
{
    public class Trainer : BaseEntity
    {
        public int TrainerId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public int Order { get; set; }
    }
}
