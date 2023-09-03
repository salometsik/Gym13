namespace Gym13.Application.Models.Trainer
{
    public class TrainerModel
    {
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public string DescriptionKa { get; set; }
        public string DescriptionEn { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
