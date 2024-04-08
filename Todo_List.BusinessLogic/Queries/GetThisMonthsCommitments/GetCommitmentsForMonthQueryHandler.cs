using MediatR;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Todo_List.BusinessLogic.Queries.GetThisMonthsCommitments
{
    public class GetCommitmentsForMonthQueryHandler : IRequestHandler<GetCommitmentsForMonthQuery, (IEnumerable<OneTimeCommitment>, IEnumerable<RecurringCommitment>)>
    {
        private readonly IGenericRepository<OneTimeCommitment> _oneTimeCommitmentRepository;
        private readonly IGenericRepository<RecurringCommitment> _recurringCommitmentRepository;

        public GetCommitmentsForMonthQueryHandler(IGenericRepository<OneTimeCommitment> oneTimeCommitmentRepository, IGenericRepository<RecurringCommitment> recurringCommitmentRepository)
        {
            _oneTimeCommitmentRepository = oneTimeCommitmentRepository;
            _recurringCommitmentRepository = recurringCommitmentRepository;
        }

        public async Task<(IEnumerable<OneTimeCommitment>, IEnumerable<RecurringCommitment>)> Handle(GetCommitmentsForMonthQuery request, CancellationToken cancellationToken)
        {
            var oneTimeCommitmentsForThisMonth = await _oneTimeCommitmentRepository.GetAllEntries().Where(otc => otc.DueDate.HasValue && otc.DueDate.Value.Month == request.Month && otc.DueDate.Value.Year == request.Year).ToListAsync();
            var recurringCommitmentsForThisMonth = await _recurringCommitmentRepository.GetAllEntries().Where(rc => rc.DueDate.HasValue && rc.DueDate.Value.Month == request.Month && rc.DueDate.Value.Year == request.Year).ToListAsync();

            return (oneTimeCommitmentsForThisMonth, recurringCommitmentsForThisMonth);
        }
    }
}
