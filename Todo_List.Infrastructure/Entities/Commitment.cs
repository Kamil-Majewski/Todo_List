namespace Todo_List.Infrastructure.Entities
{
    public class Commitment
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
        public bool AlertUserAboutStart { get; set; }
        public bool AlertUserAboutEnd { get; set; }
        public bool Recurring { get; set; }
        List<Subtask>? Subtasks { get; set; }
        List<Log> History { get; set; } = new List<Log>();
        
    }
}
