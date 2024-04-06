using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.Infrastructure.Repositories
{
    public class UnscheduledCommitmentRepository : GenericRepository<UnscheduledCommitment>, IUnscheduledCommitmentRepository
    {
        public UnscheduledCommitmentRepository(TodoListDbContext dbContext) : base(dbContext)
        {
        }
    }
}
