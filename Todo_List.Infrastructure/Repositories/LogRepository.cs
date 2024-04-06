using Todo_List.Infrastructure.Entities;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.Infrastructure.Repositories
{
    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        public LogRepository(TodoListDbContext dbContext) : base(dbContext)
        {
        }
    }
}
