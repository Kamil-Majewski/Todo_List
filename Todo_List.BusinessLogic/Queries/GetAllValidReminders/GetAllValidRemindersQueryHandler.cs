using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Queries.GetAllValidReminders
{
    public class GetAllValidRemindersQueryHandler : IRequestHandler<GetAllValidRemindersQuery, List<Commitment>>
    {
        private readonly IGenericRepository<Commitment> _repository;

        public GetAllValidRemindersQueryHandler(IGenericRepository<Commitment> repository)
        {
            _repository = repository;
        }

        public async Task<List<Commitment>> Handle(GetAllValidRemindersQuery request, CancellationToken cancellationToken)
        {
            var date = DateTime.Now;

            return await _repository.GetAllEntries().Where(c => c.ReminderSet && c.ReminderTime >= date && !c.IsCompleted).ToListAsync();
        }
    }
}
