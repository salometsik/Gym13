using Gym13.Common.Enums;

namespace Gym13.Domain.Models
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DiscountType Type { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
