using MediatR;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;

namespace Todo_List.BusinessLogic.Queries.GetAllCommitments
{
    public class GetAllCommitmentsQuery : IRequest<List<Commitment>>
    {
    }
}
