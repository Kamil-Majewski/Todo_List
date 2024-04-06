using Todo_List.BusinessLogic.Services.Interfaces;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Services
{
    public class RecurringCommitmentService : GenericService<RecurringCommitment>, IRecurringCommitmentService
    {
        public RecurringCommitmentService(IGenericRepository<RecurringCommitment> repository) : base(repository)
        {
        }
    }
}
