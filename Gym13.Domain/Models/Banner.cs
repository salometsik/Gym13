using Gym13.Domain.Models.Base;

namespace Gym13.Domain.Models
{
    public class Banner : BaseEntity
    {
        public int BannerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
