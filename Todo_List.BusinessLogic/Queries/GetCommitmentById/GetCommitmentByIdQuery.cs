using MediatR;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;

namespace Todo_List.BusinessLogic.Queries.GetCommitmentById
{
    public class GetCommitmentByIdQuery : IRequest<Commitment>
    {
        public int TaskId { get; }

        public GetCommitmentByIdQuery(int taskId)
        {
            TaskId = taskId;
        }
    }
}
