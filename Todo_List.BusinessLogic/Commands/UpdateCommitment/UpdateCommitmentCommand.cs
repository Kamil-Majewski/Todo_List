using MediatR;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;

namespace Todo_List.BusinessLogic.Commands.UpdateCommitment
{
    public class UpdateCommitmentCommand : IRequest<Commitment>
    {
        public Commitment Commitment { get; }

        public UpdateCommitmentCommand(Commitment commitment)
        {
            Commitment = commitment;
        }
    }
}
