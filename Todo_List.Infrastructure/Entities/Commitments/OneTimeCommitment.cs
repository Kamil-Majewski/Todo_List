using Todo_List.Infrastructure.Entities.Commitments.Abstract;

namespace Todo_List.Infrastructure.Entities.Commitments
{
    public class OneTimeCommitment : Commitment
    {
        public DateTime StartDate { get; set; } = default!;
        public DateTime? DueDate { get; set; }
    }
}
