using Gym13.Domain.Models.Base;

namespace Gym13.Domain.Models
{
    public class InfoTab : BaseEntity
    {
        public int InfoTabId { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
