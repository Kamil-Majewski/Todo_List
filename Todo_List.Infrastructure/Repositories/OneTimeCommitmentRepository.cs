using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.Infrastructure.Repositories
{
    public class OneTimeCommitmentRepository : GenericRepository<OneTimeCommitment>, IOneTimeCommitmentRepository
    {
        public OneTimeCommitmentRepository(TodoListDbContext dbContext) : base(dbContext)
        {
        }
    }
}
