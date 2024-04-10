using MediatR;
using Todo_List.Infrastructure.Entities.Commitments;

namespace Todo_List.BusinessLogic.Queries.GetCommitmentsForByDate
{
    public class GetCommitmentsByDateQuery : IRequest<(IEnumerable<OneTimeCommitment>, IEnumerable<RecurringCommitment>)>
    {
        public DateTime Date { get; }

        public GetCommitmentsByDateQuery(DateTime date)
        {
            Date = date;
        }
    }
}
