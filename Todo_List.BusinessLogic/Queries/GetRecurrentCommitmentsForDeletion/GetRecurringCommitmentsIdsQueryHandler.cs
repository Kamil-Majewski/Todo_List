using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Queries.GetRecurrentCommitmentsForDeletion
{
    public class GetRecurringCommitmentsIdsQueryHandler : IRequestHandler<GetRecurrentCommitmentIdsQuery, List<int>>
    {
        private readonly IGenericRepository<RecurringCommitment> _repository;

        public GetRecurringCommitmentsIdsQueryHandler(IGenericRepository<RecurringCommitment> repository)
        {
            _repository = repository;
        }

        public async Task<List<int>> Handle(GetRecurrentCommitmentIdsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllEntries().Where(c => c.ParentId == request.ParentId).Where(c => c.DueDate > DateTime.UtcNow).Select(c => c.Id).ToListAsync();
        }
    }
}
