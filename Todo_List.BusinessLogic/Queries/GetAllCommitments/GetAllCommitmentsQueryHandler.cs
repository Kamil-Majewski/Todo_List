using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Queries.GetAllCommitments
{
    public class GetAllCommitmentsQueryHandler : IRequestHandler<GetAllCommitmentsQuery, List<Commitment>>
    {
        private readonly IGenericRepository<Commitment> _repository;

        public GetAllCommitmentsQueryHandler(IGenericRepository<Commitment> repository)
        {
            _repository = repository;
        }

        public async Task<List<Commitment>> Handle(GetAllCommitmentsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllEntries().ToListAsync();
        }
    }
}
