using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Queries.GetAllEntries
{
    public class GetAllEntriesQueryHandler<TEntity> : IRequestHandler<GetAllEntriesQuery<TEntity>, IEnumerable<TEntity>> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;

        public GetAllEntriesQueryHandler(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TEntity>> Handle(GetAllEntriesQuery<TEntity> request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllEntries().ToListAsync();
        }
    }
}
