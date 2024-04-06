using Todo_List.BusinessLogic.Services.Interfaces;
using Todo_List.Infrastructure.Entities;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Services
{
    public class LogService : GenericService<Log>, ILogService

    {
        public LogService(IGenericRepository<Log> repository) : base(repository)
        {
        }
    }
}
