using Todo_List.BusinessLogic.Services.Interfaces;
using Todo_List.Infrastructure.Entities;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Services
{
    public class ReminderService : GenericService<Reminder>, IReminderService
    {
        public ReminderService(IGenericRepository<Reminder> repository) : base(repository)
        {
        }
    }
}
