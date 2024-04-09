using MediatR;
using Todo_List.Infrastructure.Entities.Commitments;

namespace Todo_List.BusinessLogic.Queries.GetRecurrentCommitmentsForDeletion
{
    public class GetRecurrentCommitmentIdsQuery : IRequest<List<int>>
    {
        public int ParentId { get; set; }

        public GetRecurrentCommitmentIdsQuery(int parentId)
        {
            ParentId = parentId;
        }
    }
}
