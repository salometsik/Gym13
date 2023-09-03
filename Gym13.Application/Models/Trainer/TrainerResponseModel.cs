namespace Gym13.Application.Models.Trainer
{
    public class TrainerResponseModel : BaseResponseModel
    {
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? ImageUrl { get; set; }
    }
}
