using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.Infrastructure.Repositories
{
    public class RecurringCommitmentRepository : GenericRepository<RecurringCommitment>, IReccuringCommitmentRepository
    {
        public RecurringCommitmentRepository(TodoListDbContext dbContext) : base(dbContext)
        {
        }
    }
}
