using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.Infrastructure.Repositories
{
    public class UnScheduledCommitmentRepository : GenericRepository<UnScheduledCommitment>, IUnScheduledCommitmentRepository
    {
        public UnScheduledCommitmentRepository(TodoListDbContext dbContext) : base(dbContext)
        {
        }
    }
}
