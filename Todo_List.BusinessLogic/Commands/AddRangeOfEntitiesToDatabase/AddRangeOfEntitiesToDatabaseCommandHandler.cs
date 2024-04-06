using MediatR;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Commands.AddRangeOfEntitiesToDatabase
{
    public class AddRangeOfEntitiesToDatabaseCommandHandler<TEntity> : IRequestHandler<AddRangeOfEntitiesToDatabaseCommand<TEntity>, IEnumerable<TEntity>> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;

        public AddRangeOfEntitiesToDatabaseCommandHandler(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TEntity>> Handle(AddRangeOfEntitiesToDatabaseCommand<TEntity> request, CancellationToken cancellationToken)
        {
            return await _repository.AddRangeOfEntitiesToDatabaseAsync(request.Entities);
        }
    }
}
