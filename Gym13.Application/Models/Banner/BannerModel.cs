namespace Gym13.Application.Models.Banner
{
    public class BannerModel
    {
        public int? BannerId { get; set; }
        public string TitleKa { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionKa { get; set; }
        public string DescriptionEn { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
