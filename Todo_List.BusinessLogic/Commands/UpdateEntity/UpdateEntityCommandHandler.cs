using MediatR;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Commands.UpdateEntity
{
    public class UpdateEntityCommandHandler<TEntity> : IRequestHandler<UpdateEntityCommand<TEntity>, TEntity> where TEntity: class
    {
        private readonly IGenericRepository<TEntity> _repository;

        public UpdateEntityCommandHandler(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<TEntity> Handle(UpdateEntityCommand<TEntity> request, CancellationToken cancellationToken)
        {
            return await _repository.UpdateEntityAsync(request.Entity);
        }
    }
}
