using MediatR;
using Todo_List.Infrastructure.Entities.Commitments;

namespace Todo_List.BusinessLogic.Queries.GetTodaysCommitments
{
    public class GetTodaysCommitmentsQuery : IRequest<(IEnumerable<OneTimeCommitment>, IEnumerable<RecurringCommitment>)>
    {

    }
}
