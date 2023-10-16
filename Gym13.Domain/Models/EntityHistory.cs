using Gym13.Common.Enums;

namespace Gym13.Domain.Models
{
    public class EntityHistory
    {
        public int EntityHistoryId { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public EntityActionType ActionType { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow.AddHours(4);
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
}
