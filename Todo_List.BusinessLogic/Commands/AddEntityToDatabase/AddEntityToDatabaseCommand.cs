using MediatR;

namespace Todo_List.BusinessLogic.Commands.AddEntityToDatabase
{
    public class AddEntityToDatabaseCommand<TEntity> : IRequest<TEntity> where TEntity: class
    {
        public TEntity Entity { get; }

        public AddEntityToDatabaseCommand(TEntity entity)
        {
            Entity = entity;
        }
    }
}
