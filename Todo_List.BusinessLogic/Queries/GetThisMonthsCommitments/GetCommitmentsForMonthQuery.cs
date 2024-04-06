using MediatR;
using Todo_List.Infrastructure.Entities.Commitments;

namespace Todo_List.BusinessLogic.Queries.GetThisMonthsCommitments
{
    public class GetCommitmentsForMonthQuery : IRequest<(IEnumerable<OneTimeCommitment>, IEnumerable<RecurringCommitment>)>
    {
        public int Year { get; }
        public int Month { get; }

        public GetCommitmentsForMonthQuery(int year, int month)
        {
            Year = year;
            Month = month;
        }
    }
}
