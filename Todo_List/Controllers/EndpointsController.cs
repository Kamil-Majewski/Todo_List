using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo_List.BusinessLogic.Commands.AddEntityToDatabase;
using Todo_List.BusinessLogic.Commands.DeleteCommitmentById;
using Todo_List.BusinessLogic.Commands.UpdateCommitment;
using Todo_List.BusinessLogic.Queries.GetAllCommitments;
using Todo_List.BusinessLogic.Queries.GetAllEntries;
using Todo_List.BusinessLogic.Queries.GetCommitmentById;
using Todo_List.BusinessLogic.Queries.GetCommitmentsForByDate;
using Todo_List.BusinessLogic.Queries.GetRecurrentCommitmentsForDeletion;
using Todo_List.BusinessLogic.Queries.GetTodaysCommitments;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;
using Todo_List.Infrastructure.Enums;

namespace Todo_List.WebApp.Controllers
{

    public class EndpointsController : Controller
    {
        private readonly IMediator _mediator;

        public EndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private async Task<IActionResult> AddCommitmentAndReminder<T>(T commitment, string? taskReminderDate) where T : Commitment
        {
            commitment.ReminderTime = DateTime.TryParse(taskReminderDate, out var reminderDateTime) ? reminderDateTime : null;

            return Ok(await _mediator.Send(new AddEntityToDatabaseCommand<T>(commitment)));
        }

        private async Task AddRecurringCommitmentInstances(RecurringCommitment commitment, string? taskReminderDate)
        {
            await AddCommitmentAndReminder(commitment, taskReminderDate);

            DateTime recurStart = commitment.RecurrenceStart;
            DateTime recurEnd = commitment.RecurUntil;
            int interval = commitment.RecurInterval;
            string unit = commitment.RecurUnit.ToString();

            int numberOfIntervals;
            if (unit == "Days")
            {
                TimeSpan difference = recurEnd - recurStart;
                numberOfIntervals = difference.Days / interval;
            }
            else if (unit == "Weeks")
            {
                int totalWeeks = (int)(recurEnd - recurStart).TotalDays / 7;
                numberOfIntervals = totalWeeks / interval;
            }
            else if (unit == "Months")
            {
                int totalMonths = ((recurEnd.Year - recurStart.Year) * 12) + recurEnd.Month - recurStart.Month;
                numberOfIntervals = totalMonths / interval;
            }
            else
            {
                int totalYears = recurEnd.Year - recurStart.Year;
                numberOfIntervals = totalYears / interval;
            }

            for (int i = 0; i < numberOfIntervals; i++)
            {
                DateTime dueDate;
                DateTime? reminderDateTime = DateTime.TryParse(taskReminderDate, out var dt) ? dt : null;
                if (unit == "Days")
                {
                    dueDate = commitment.DueDate!.Value.AddDays(interval * (i + 1));
                    reminderDateTime = reminderDateTime != null ? reminderDateTime.Value.AddDays(interval * (i + 1)) : null;
                }
                else if (unit == "Weeks")
                {
                    dueDate = commitment.DueDate!.Value.AddDays(interval * (7 * (i + 1)));
                    reminderDateTime = reminderDateTime != null ? reminderDateTime.Value.AddDays(interval * (7 * (i + 1))) : null;
                }
                else if (unit == "Months")
                {
                    dueDate = commitment.DueDate!.Value.AddMonths(interval * (i + 1));
                    reminderDateTime = reminderDateTime!= null ? reminderDateTime.Value.AddMonths(interval * (i + 1)) : null;
                }
                else
                {
                    dueDate = commitment.DueDate!.Value.AddYears(interval * (i + 1));
                    reminderDateTime = reminderDateTime != null ? reminderDateTime.Value.AddYears(interval * (i + 1)) : null;
                }

                var recurringInstance = new RecurringCommitment
                {
                    ParentId = commitment.Id,
                    Name = commitment.Name,
                    Notes = commitment.Notes,
                    IsCompleted = false,
                    Priority = commitment.Priority,
                    DueDate = dueDate,
                    SubtasksSerialized = null,
                    ReminderSet = commitment.ReminderSet,
                    ReminderTime = reminderDateTime,
                    RecurUnit = commitment.RecurUnit,
                    RecurInterval = commitment.RecurInterval,
                    RecurrenceStart = commitment.RecurrenceStart,
                    RecurUntil = commitment.RecurUntil,
                };
                    
                await _mediator.Send(new AddEntityToDatabaseCommand<RecurringCommitment>(recurringInstance));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            return Json(await _mediator.Send(new GetCommitmentByIdQuery(taskId)));
        }

        [HttpGet]
        public async Task<IActionResult> FinishTask(int taskId)
        {
            var task = await _mediator.Send(new GetCommitmentByIdQuery(taskId));
            task.IsCompleted = true;

            return Json(await _mediator.Send(new UpdateCommitmentCommand(task)));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCommitment(int taskId, string taskName, string taskPriority, string? taskNotes, string? taskDueDate, string taskRecurrenceUnit, int taskRecurrenceInterval, string? taskRecurStart, string? taskRecurUntil, string? taskReminderDate)
        {
            var task = await _mediator.Send(new GetCommitmentByIdQuery(taskId));
            var parsedRecurrenceUnit = int.Parse(taskRecurrenceUnit);
            var parsedPriority = int.Parse(taskPriority);
            string updatedType;

            if(taskDueDate == null)
            {
                updatedType = "Unscheduled";
            }
            else if(taskDueDate != null && (parsedRecurrenceUnit == -1 || taskRecurrenceInterval == 0 || taskRecurStart == null))
            {
                updatedType = "OneTime";
            }
            else
            {
                updatedType = "Recurring";
            }

            if (updatedType == "Unscheduled")
            {
                if (task is UnscheduledCommitment)
                {
                    task.Name = taskName;
                    task.Priority = parsedPriority == -1 ? null : (Priority)parsedPriority;
                    task.Notes = taskNotes;
                    task.ReminderSet = taskReminderDate != null;
                    task.ReminderTime = DateTime.TryParse(taskReminderDate, out var reminderDateTime) ? reminderDateTime : null;

                    return Json(_mediator.Send(new UpdateCommitmentCommand(task)));
                }
                else if (task is OneTimeCommitment)
                {
                    var newTask = new UnscheduledCommitment
                    {
                        Name = taskName,
                        Priority = parsedPriority == -1 ? null : (Priority)parsedPriority,
                        Notes = taskNotes,
                        ReminderSet = taskReminderDate != null,
                        ReminderTime = DateTime.TryParse(taskReminderDate, out var reminderDateTime) ? reminderDateTime : null,
                        IsCompleted = task.IsCompleted,
                        SubtasksSerialized = null,
                    };

                    await _mediator.Send(new DeleteCommitmentByIdCommand(task.Id));

                    return Json(await _mediator.Send(new AddEntityToDatabaseCommand<UnscheduledCommitment>(newTask)));
                }
                else
                {
                    var recurringTask = (RecurringCommitment)task;

                    var newUnscheduledTask = new UnscheduledCommitment
                    {
                        Name = taskName,
                        Priority = parsedPriority == -1 ? null : (Priority)parsedPriority,
                        Notes = taskNotes,
                        ReminderSet = taskReminderDate != null,
                        ReminderTime = DateTime.TryParse(taskReminderDate, out var reminderDateTime) ? reminderDateTime : null,
                        IsCompleted = recurringTask.IsCompleted,
                        SubtasksSerialized = null,
                    };

                    var parentId = recurringTask.ParentId == 0 ? recurringTask.Id : recurringTask.ParentId;
                    var listOfRecurringTaskIds = await _mediator.Send(new GetRecurrentCommitmentIdsQuery(parentId));

                    foreach (var id in listOfRecurringTaskIds)
                    {
                        await _mediator.Send(new DeleteCommitmentByIdCommand(id));
                    }

                    await _mediator.Send(new DeleteCommitmentByIdCommand(recurringTask.Id));

                    await _mediator.Send(new AddEntityToDatabaseCommand<UnscheduledCommitment>(newUnscheduledTask));

                    return Json(newUnscheduledTask);
                }
            }
            else if (updatedType == "OneTime")
            {
                if (task is UnscheduledCommitment)
                {
                    var newTask = new OneTimeCommitment
                    {
                        Name = taskName,
                        Priority = parsedPriority == -1 ? null : (Priority)parsedPriority,
                        Notes = taskNotes,
                        ReminderSet = taskReminderDate != null,
                        ReminderTime = DateTime.TryParse(taskReminderDate, out var reminderDateTime) ? reminderDateTime : null,
                        IsCompleted = task.IsCompleted,
                        SubtasksSerialized = null,
                        DueDate = DateTime.Parse(taskDueDate!)
                    };

                    await _mediator.Send(new DeleteCommitmentByIdCommand(task.Id));

                    return Json(await _mediator.Send(new AddEntityToDatabaseCommand<OneTimeCommitment>(newTask)));
                }
                else if (task is OneTimeCommitment otTask)
                {
                    otTask.Name = taskName;
                    otTask.Priority = parsedPriority == -1 ? null : (Priority)parsedPriority;
                    otTask.Notes = taskNotes;
                    otTask.ReminderSet = taskReminderDate != null;
                    otTask.ReminderTime = DateTime.TryParse(taskReminderDate, out var reminderDateTime) ? reminderDateTime : null;
                    otTask.DueDate = DateTime.Parse(taskDueDate!);

                    return Json(await _mediator.Send(new UpdateCommitmentCommand(otTask)));
                }
                else
                {
                    var recurringTask = (RecurringCommitment)task;

                    var newOneTimeTask = new OneTimeCommitment
                    {
                        Name = taskName,
                        Priority = parsedPriority == -1 ? null : (Priority)parsedPriority,
                        Notes = taskNotes,
                        ReminderSet = taskReminderDate != null,
                        ReminderTime = DateTime.TryParse(taskReminderDate, out var reminderDateTime) ? reminderDateTime : null,
                        IsCompleted = recurringTask.IsCompleted,
                        SubtasksSerialized = null,
                        DueDate = recurringTask.DueDate,
                    };

                    var parentId = recurringTask.ParentId == 0 ? recurringTask.Id : recurringTask.ParentId;
                    var listOfRecurringTaskIds = await _mediator.Send(new GetRecurrentCommitmentIdsQuery(parentId));

                    foreach (var id in listOfRecurringTaskIds)
                    {
                        await _mediator.Send(new DeleteCommitmentByIdCommand(id));
                    }

                    await _mediator.Send(new DeleteCommitmentByIdCommand(recurringTask.Id));

                    await _mediator.Send(new AddEntityToDatabaseCommand<OneTimeCommitment>(newOneTimeTask));

                    return Json(newOneTimeTask);

                }

            }
            else
            {
                if (task is UnscheduledCommitment || task is OneTimeCommitment)
                {
                    var newRecurringTask = new RecurringCommitment
                    {
                        ParentId = 0,
                        Name = taskName,
                        Notes = taskNotes,
                        IsCompleted = task.IsCompleted,
                        Priority = parsedPriority == -1 ? null : (Priority)parsedPriority,
                        SubtasksSerialized = null,
                        DueDate = DateTime.Parse(taskDueDate!),
                        ReminderSet = taskReminderDate != null,
                        RecurUnit = (RecurrenceUnit)int.Parse(taskRecurrenceUnit),
                        RecurInterval = taskRecurrenceInterval,
                        RecurrenceStart = DateTime.Parse(taskRecurStart!),
                        RecurUntil = taskRecurUntil == null ? DateTime.Parse(taskRecurStart!).AddMonths(1) : DateTime.Parse(taskRecurUntil)
                    };

                    await _mediator.Send(new DeleteCommitmentByIdCommand(task.Id));

                    await AddRecurringCommitmentInstances(newRecurringTask, taskReminderDate);

                    return Json(newRecurringTask);

                }
                else
                {
                    var recurringTask = (RecurringCommitment)task;

                    var newRecurringTask = new RecurringCommitment
                    {
                        ParentId = 0,
                        Name = taskName,
                        Notes = taskNotes,
                        IsCompleted = recurringTask.IsCompleted,
                        Priority = parsedPriority == -1 ? null : (Priority)parsedPriority,
                        SubtasksSerialized = null,
                        DueDate = DateTime.Parse(taskDueDate!),
                        ReminderSet = taskReminderDate != null,
                        RecurUnit = (RecurrenceUnit)int.Parse(taskRecurrenceUnit),
                        RecurInterval = taskRecurrenceInterval,
                        RecurrenceStart = DateTime.Parse(taskRecurStart!),
                        RecurUntil = taskRecurUntil == null ? DateTime.Parse(taskRecurStart!).AddMonths(1) : DateTime.Parse(taskRecurUntil)
                    };

                    var parentId = recurringTask.ParentId == 0 ? recurringTask.Id : recurringTask.ParentId;
                    var listOfRecurringTaskIds = await _mediator.Send(new GetRecurrentCommitmentIdsQuery(parentId));

                    foreach (var id in listOfRecurringTaskIds)
                    {
                        await _mediator.Send(new DeleteCommitmentByIdCommand(id));
                    }

                    await _mediator.Send(new DeleteCommitmentByIdCommand(recurringTask.Id));

                    await AddRecurringCommitmentInstances(newRecurringTask, taskReminderDate);

                    return Json(newRecurringTask);
                }
            }       
        }

        [HttpGet]
        public async Task<IActionResult> CreateNewTask(string taskName, string taskPriority, string? taskNotes, string? taskDueDate, string taskRecurrenceUnit, int taskRecurrenceInterval, string? taskRecurStart, string? taskRecurUntil, string? taskReminderDate)
        {
            var parsedRecurrenceUnit = int.Parse(taskRecurrenceUnit);
            var parsedPriority = int.Parse(taskPriority);

            if (taskDueDate == null)
            {
                var unscheduledCommitment = new UnscheduledCommitment
                {
                    Name = taskName,
                    Notes = taskNotes,
                    IsCompleted = false,
                    Priority = parsedPriority == -1 ? null : (Priority)parsedPriority,
                    SubtasksSerialized = null,
                    ReminderSet = taskReminderDate != null
                };

                return await AddCommitmentAndReminder(unscheduledCommitment, taskReminderDate);

            }
            else if (taskDueDate != null && (parsedRecurrenceUnit == -1 || taskRecurrenceInterval == 0 || taskRecurStart == null))
            {
                var oneTimeCommitment = new OneTimeCommitment
                {
                    Name = taskName,
                    Notes = taskNotes,
                    IsCompleted = false,
                    Priority = parsedPriority == -1 ? null : (Priority)parsedPriority,
                    SubtasksSerialized = null,
                    ReminderSet = taskReminderDate != null ? true : false,
                    DueDate = DateTime.Parse(taskDueDate)
                };

                return await AddCommitmentAndReminder(oneTimeCommitment, taskReminderDate);
            }
            else
            {
                var recurringCommitment = new RecurringCommitment
                {
                    ParentId = 0,
                    Name = taskName,
                    Notes = taskNotes,
                    IsCompleted = false,
                    Priority = parsedPriority == -1 ? null : (Priority)parsedPriority,
                    SubtasksSerialized = null,
                    DueDate = DateTime.Parse(taskDueDate!),
                    ReminderSet = taskReminderDate != null,
                    RecurUnit = (RecurrenceUnit)(int.Parse(taskRecurrenceUnit)),
                    RecurInterval = taskRecurrenceInterval,
                    RecurrenceStart = DateTime.Parse(taskRecurStart!),
                    RecurUntil = taskRecurUntil == null ? DateTime.Parse(taskRecurStart!).AddMonths(1) : DateTime.Parse(taskRecurUntil)
                };

                await AddRecurringCommitmentInstances(recurringCommitment, taskReminderDate);

                return Ok(recurringCommitment);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCommitmentsForCertainDay(DateTime date)
        {
            var (todayOneTimeCommitments, todaysRecurringCommitments) = await _mediator.Send(new GetCommitmentsByDateQuery(date));

            var allCertainDaysCommitments = todayOneTimeCommitments.Select(c => new {
                                            Id = c.Id,
                                            Name = c.Name,
                                            DueDate = c.DueDate,
                                            IsCompleted = c.IsCompleted,
                                            Priority = c.Priority.HasValue ? c.Priority.ToString() : "Brak",
                                            ReminderSet = c.ReminderSet,
                                            ReminderTime = c.ReminderTime
                                        })
                                        .Concat(todaysRecurringCommitments.Select(c => new
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            DueDate = c.DueDate,
                                            IsCompleted = c.IsCompleted,
                                            Priority = c.Priority.HasValue ? c.Priority.ToString() : "Brak",
                                            ReminderSet = c.ReminderSet,
                                            ReminderTime = c.ReminderTime
                                        }))
                                        .AsEnumerable()
                                        .ToList();

            return Ok(allCertainDaysCommitments.OrderBy(c => c.DueDate));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            await _mediator.Send(new DeleteCommitmentByIdCommand(taskId));
            return Ok("Task deleted successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetTodaysTasks()
        {
            var (todayOneTimeCommitments, todaysRecurringCommitments) = await _mediator.Send(new GetTodaysCommitmentsQuery());

            var allTodaysCommitments = todayOneTimeCommitments.Select(c => new {
                                            Id = c.Id,
                                            Name = c.Name,
                                            DueDate = c.DueDate,
                                            IsCompleted = c.IsCompleted,
                                            Priority = c.Priority.HasValue ? c.Priority.ToString() : "Brak",
                                            ReminderSet = c.ReminderSet,
                                            ReminderTime = c.ReminderTime
                                        })
                                        .Concat(todaysRecurringCommitments.Select(c => new
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            DueDate = c.DueDate,
                                            IsCompleted = c.IsCompleted,
                                            Priority = c.Priority.HasValue ? c.Priority.ToString() : "Brak",
                                            ReminderSet = c.ReminderSet,
                                            ReminderTime = c.ReminderTime
                                        }))
                                        .AsEnumerable()
                                        .ToList();

            return Ok(allTodaysCommitments.OrderBy(c => c.DueDate));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var commitments = await _mediator.Send(new GetAllCommitmentsQuery());

            var newList = new List<object>();

            foreach(var commitment in commitments)
            {
                if (commitment is OneTimeCommitment || commitment is RecurringCommitment)
                {
                    newList.Add(commitment);
                }
            }

            return Json(newList);
        }
    }
}
