using MediatR;

namespace Todo_List.BusinessLogic.Commands.DeleteCommitmentById
{
    public class DeleteCommitmentByIdCommand : IRequest<Unit>
    {
        public int CommitmentId { get; }

        public DeleteCommitmentByIdCommand(int commitmentId)
        {
            CommitmentId = commitmentId;
        }
    }
}
