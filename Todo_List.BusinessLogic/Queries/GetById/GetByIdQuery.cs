using MediatR;

namespace Todo_List.BusinessLogic.Queries.GetById
{
    public class GetByIdQuery<TEntity> : IRequest<TEntity> where TEntity : class
    {
        public int EntityId { get; }

        public GetByIdQuery(int entityId)
        {
            EntityId = entityId;
        }
    }
}
