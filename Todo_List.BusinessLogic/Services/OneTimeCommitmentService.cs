using Todo_List.BusinessLogic.Services.Interfaces;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Services
{
    public class OneTimeCommitmentService : GenericService<OneTimeCommitment>, IOneTimeCommitmentService
    {
        public OneTimeCommitmentService(IGenericRepository<OneTimeCommitment> repository) : base(repository)
        {
        }
    }
}
