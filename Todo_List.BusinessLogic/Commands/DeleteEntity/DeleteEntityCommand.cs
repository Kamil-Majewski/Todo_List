using MediatR;

namespace Todo_List.BusinessLogic.Commands.DeleteEntity
{
    public class DeleteEntityCommand<TEntity> : IRequest<Unit> where TEntity : class
    {
        public int EntityId { get; }

        public DeleteEntityCommand(int entityId)
        {
            EntityId = entityId;
        }
    }
}
