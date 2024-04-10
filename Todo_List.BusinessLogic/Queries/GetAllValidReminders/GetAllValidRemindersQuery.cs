using MediatR;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;

namespace Todo_List.BusinessLogic.Queries.GetAllValidReminders
{
    public class GetAllValidRemindersQuery : IRequest<List<Commitment>>
    {
    }
}
