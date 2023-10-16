using Gym13.Domain.Models.Base;

namespace Gym13.Domain.Models
{
    public class PlanService : BaseEntity
    {
        public int PlanServiceId { get; set; }
        public string Title { get; set; }
    }
}
