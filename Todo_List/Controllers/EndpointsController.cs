using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo_List.BusinessLogic.Commands.AddEntityToDatabase;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Enums;

namespace Todo_List.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndpointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> CreateNewTask(string taskName, string taskPriority, string taskNotes, string? taskStartDate, string? taskEndDate, string taskRecurrenceUnit, int? taskRecurrenceInterval)
        {
            if (taskStartDate == null)
            {
                var unscheduledCommitment = new UnscheduledCommitment
                {
                    Name = taskName,
                    Notes = taskNotes,
                    IsCompleted = false,
                    Priority = int.Parse(taskPriority) == -1 ? null : (Priority)int.Parse(taskPriority),
                    SubtasksSerialized = null
                };

                return Ok(await _mediator.Send(new AddEntityToDatabaseCommand<UnscheduledCommitment>(unscheduledCommitment)));
            }
            else if(taskStartDate != "" && taskRecurrenceInterval != 0 && int.Parse(taskRecurrenceUnit) != -1)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
