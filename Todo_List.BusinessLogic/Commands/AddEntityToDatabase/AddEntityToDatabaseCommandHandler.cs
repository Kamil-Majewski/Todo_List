using MediatR;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Commands.AddEntityToDatabase
{
    public class AddEntityToDatabaseCommandHandler<TEntity> : IRequestHandler<AddEntityToDatabaseCommand<TEntity>, TEntity> where TEntity: class
    {
        private readonly IGenericRepository<TEntity> _repository;

        public AddEntityToDatabaseCommandHandler(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<TEntity> Handle(AddEntityToDatabaseCommand<TEntity> request, CancellationToken cancellationToken)
        {
            return await _repository.AddEntityToDatabaseAsync(request.Entity);
        }
    }
}
