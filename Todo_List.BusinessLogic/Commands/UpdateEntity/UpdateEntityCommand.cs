using MediatR;

namespace Todo_List.BusinessLogic.Commands.UpdateEntity
{
    public class UpdateEntityCommand<TEntity> : IRequest<TEntity> where TEntity : class
    {
        public TEntity Entity { get; }

        public UpdateEntityCommand(TEntity entity)
        {
            Entity = entity;
        }
    }
}
