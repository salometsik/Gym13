namespace Gym13.Domain.Models.Base
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public bool IsDeleted { get; set; }
    }
}
