namespace Todo_List.Infrastructure.Entities
{
    public class Commitment
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public List<DateTime>? Reminders { get; set; }
        public List<Subtask>? Subtasks { get; set; }
        public bool IsCompleted { get; set; }
        public Priority Priority { get; set; }
        
    }
}
