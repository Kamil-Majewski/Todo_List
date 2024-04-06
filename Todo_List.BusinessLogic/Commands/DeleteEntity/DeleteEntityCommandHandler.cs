using MediatR;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Commands.DeleteEntity
{
    public class DeleteEntityCommandHandler<TEntity> : IRequestHandler<DeleteEntityCommand<TEntity>, Unit> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;

        public DeleteEntityCommandHandler(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteEntityCommand<TEntity> request, CancellationToken cancellationToken)
        {
            await _repository.DeleteEntityAsync(request.EntityId);
            return Unit.Value;
        }
    }
}
