using Todo_List.Infrastructure.Enums;

namespace Todo_List.Infrastructure.Entities.Commitments.Abstract
{
    public abstract class Commitment
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Notes { get; set; }
        public string? SubtasksSerialized { get; set; }
        public bool IsCompleted { get; set; }
        public bool ReminderSet { get; set; }
        public DateTime? ReminderTime { get; set; }
        public Priority? Priority { get; set; }

    }
}
