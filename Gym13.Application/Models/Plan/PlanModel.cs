using Gym13.Common.Enums;

namespace Gym13.Application.Models.Plan
{
    public class PlanModel
    {
        public int? PlanId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public int PeriodNumber { get; set; }
        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public PlanPeriodType PeriodType { get; set; }
        public bool IsUnlimited { get; set; }
        public List<PlanServicesItem> PlanServices { get; set; }
    }
    public class PlanServicesItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
