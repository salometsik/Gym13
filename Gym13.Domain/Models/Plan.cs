using Gym13.Common.Enums;

namespace Gym13.Domain.Models
{
    public class Plan
    {
        public int PlanId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime? UpdateDate { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int PeriodNumber { get; set; }
        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public PlanPeriodType PeriodType { get; set; }
        public bool IsActive { get; set; }
        public string PlanServiceIds { get; set; }
        public ICollection<PlanService> PlanServices { get; set; }
    }
}
