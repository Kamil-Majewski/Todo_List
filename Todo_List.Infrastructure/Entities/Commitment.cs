using Todo_List.Infrastructure.Enums;

namespace Todo_List.Infrastructure.Entities
{
    public class Commitment
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? SubtasksSerialized { get; set; }
        public int RecurInterval { get; set; }
        public DateTime RecurUntil { get; set; }
        public RecurrenceUnit? RecurUnit { get; set; }
        public bool IsCompleted { get; set; }
        public Priority? Priority { get; set; }

    }
}
