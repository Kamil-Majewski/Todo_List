using MediatR;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Queries.GetById
{
    public class GetByIdQueryHandler<TEntity> : IRequestHandler<GetByIdQuery<TEntity>, TEntity> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        public GetByIdQueryHandler(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<TEntity> Handle(GetByIdQuery<TEntity> request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.EntityId);
        }
    }
}
