
namespace Todo_List.Infrastructure.Entities
{
    public class Reminder
    {
        public int Id { get; set; }
        public int CommitmentId { get; set; }
        public DateTime ReminderTime { get; set; }
        public string Status { get; set; } = default!;
    }
}
