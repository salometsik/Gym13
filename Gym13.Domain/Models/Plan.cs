﻿using Gym13.Common.Enums;
using Gym13.Domain.Models.Base;

namespace Gym13.Domain.Models
{
    public class Plan : BaseEntity
    {
        public int PlanId { get; set; }
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
        public Discount Discount { get; set; }
        public int? DiscountId { get; set; }
    }
}
