using MediatR;

namespace Todo_List.BusinessLogic.Queries.GetAllEntries
{
    public class GetAllEntriesQuery<TEntity> : IRequest<IEnumerable<TEntity>> where TEntity : class
    {
    }
}
