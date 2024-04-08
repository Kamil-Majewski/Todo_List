using Todo_List.Infrastructure.Entities.Commitments.Abstract;
using Todo_List.Infrastructure.Enums;

namespace Todo_List.Infrastructure.Entities.Commitments
{
    public class RecurringCommitment : Commitment
    {
        public DateTime RecurrenceStart{ get; set; } = default!;
        public DateTime? DueDate { get; set; }
        public int RecurInterval { get; set; }
        public DateTime RecurUntil { get; set; }
        public RecurrenceUnit RecurUnit { get; set; }
    }
}
