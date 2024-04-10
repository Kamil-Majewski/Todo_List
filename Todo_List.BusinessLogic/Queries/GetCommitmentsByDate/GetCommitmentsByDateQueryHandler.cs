using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Queries.GetCommitmentsForByDate
{
    public class GetCommitmentsByDateQueryHandler : IRequestHandler<GetCommitmentsByDateQuery, (IEnumerable<OneTimeCommitment>, IEnumerable<RecurringCommitment>)>
    {
        private readonly IGenericRepository<OneTimeCommitment> _oneTimeCommitmentRepository;
        private readonly IGenericRepository<RecurringCommitment> _recurringCommitmentRepository;

        public GetCommitmentsByDateQueryHandler(IGenericRepository<OneTimeCommitment> oneTimeCommitmentRepository, IGenericRepository<RecurringCommitment> recurringCommitmentRepository)
        {
            _oneTimeCommitmentRepository = oneTimeCommitmentRepository;
            _recurringCommitmentRepository = recurringCommitmentRepository;
        }

        public async Task<(IEnumerable<OneTimeCommitment>, IEnumerable<RecurringCommitment>)> Handle(GetCommitmentsByDateQuery request, CancellationToken cancellation)
        {
            var oneTimeCommitmentsForThisMonth = await _oneTimeCommitmentRepository.GetAllEntries().Where(otc => otc.DueDate.HasValue && otc.DueDate.Value.Date == request.Date.Date).ToListAsync();
            var recurringCommitmentsForThisMonth = await _recurringCommitmentRepository.GetAllEntries().Where(rc => rc.DueDate.HasValue && rc.DueDate.Value.Date == request.Date.Date).ToListAsync();

            return (oneTimeCommitmentsForThisMonth, recurringCommitmentsForThisMonth);
        }
    }
}
