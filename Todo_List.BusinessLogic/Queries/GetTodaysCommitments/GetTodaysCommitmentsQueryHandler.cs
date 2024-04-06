using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Queries.GetTodaysCommitments
{
    public class GetTodaysCommitmentsQueryHandler : IRequestHandler<GetTodaysCommitmentsQuery, (IEnumerable<OneTimeCommitment>, IEnumerable<RecurringCommitment>)>
    {
        private readonly IGenericRepository<OneTimeCommitment> _oneTimeCommitmentRepository;
        private readonly IGenericRepository<RecurringCommitment> _recurringCommitmentRepository;
        public GetTodaysCommitmentsQueryHandler(IGenericRepository<OneTimeCommitment> oneTimeCommitmentRepository, IGenericRepository<RecurringCommitment> recurringCommitmentRepository)
        {
            _oneTimeCommitmentRepository = oneTimeCommitmentRepository;
            _recurringCommitmentRepository = recurringCommitmentRepository;
        }

        public async Task<(IEnumerable<OneTimeCommitment>, IEnumerable<RecurringCommitment>)> Handle(GetTodaysCommitmentsQuery request, CancellationToken cancellation)
        {
            var todaysDate = DateTime.Now.Date;

            var todaysOneTimeCommitments = await _oneTimeCommitmentRepository.GetAllEntries().Where(otc => otc.StartDate.Date == todaysDate || (otc.DueDate.HasValue && otc.DueDate.Value.Date == todaysDate)).ToListAsync();
            var todaysRecurringCommitments = await _recurringCommitmentRepository.GetAllEntries().Where(rc => rc.StartDate.Date == todaysDate || (rc.DueDate.HasValue && rc.DueDate.Value.Date == todaysDate)).ToListAsync();

            return (todaysOneTimeCommitments, todaysRecurringCommitments);
        }
    }
}
    