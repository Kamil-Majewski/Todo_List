using MediatR;

namespace Todo_List.BusinessLogic.Commands.AddRangeOfEntitiesToDatabase
{
    public class AddRangeOfEntitiesToDatabaseCommand<TEntity> : IRequest<IEnumerable<TEntity>> where TEntity : class
    {
        public IEnumerable<TEntity> Entities { get; }

        public AddRangeOfEntitiesToDatabaseCommand(IEnumerable<TEntity> entities)
        {
            Entities = entities;
        }

    }
}
