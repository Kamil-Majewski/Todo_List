using Todo_List.Infrastructure.Entities.Commitments.Abstract;

namespace Todo_List.Infrastructure.Entities.Commitments
{
    public class OneTimeCommitment : Commitment
    {
        public DateTime? DueDate { get; set; } = default!;
    }
}
