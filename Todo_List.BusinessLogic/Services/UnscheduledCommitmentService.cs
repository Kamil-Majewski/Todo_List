using Todo_List.BusinessLogic.Services.Interfaces;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Services
{
    public class UnscheduledCommitmentService : GenericService<UnscheduledCommitment>, IUnscheduledCommitmentService
    {
        public UnscheduledCommitmentService(IGenericRepository<UnscheduledCommitment> repository) : base(repository)
        {
        }
    }
}
