using Todo_List.Infrastructure.Entities;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.Infrastructure.Repositories
{
    public class ReminderRepository : GenericRepository<Reminder>, IReminderRepository
    {
        public ReminderRepository(TodoListDbContext dbContext) : base(dbContext)
        {
        }
    }
}
