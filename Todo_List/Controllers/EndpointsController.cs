using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo_List.BusinessLogic.Commands.AddEntityToDatabase;
using Todo_List.BusinessLogic.Queries.GetTodaysCommitments;
using Todo_List.Infrastructure.Entities;
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
            var task = await _mediator.Send(new AddEntityToDatabaseCommand<T>(commitment));

            if (taskReminderDate != null)
            {
                var reminder = new Reminder
                {
                    CommitmentId = commitment.Id,
                    ReminderTime = DateTime.TryParse(taskReminderDate, out var reminderDateTime) ? reminderDateTime : DateTime.MinValue,
                    Status = "Oczekiwanie"
                };

                await _mediator.Send(new AddEntityToDatabaseCommand<Reminder>(reminder));
            }

            return Ok(task);
        }

        private async Task AddRecurringCommitmentInstances(RecurringCommitment commitment, string? taskReminderDate)
        {
            await AddCommitmentAndReminder(commitment, taskReminderDate);

            DateTime recurStart = commitment.RecurrenceStart;
            DateTime recurEnd = commitment.RecurUntil;
            int interval = commitment.RecurInterval;
            string unit = commitment.RecurUnit.ToString();

            int increment = unit == "Days" ? interval : interval * 7; // Adjust increment based on unit

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
                DateTime reminderDateTime = DateTime.TryParse(taskReminderDate, out var dt) ? dt : DateTime.MinValue;
                if (unit == "Days")
                {
                    dueDate = commitment.DueDate!.Value.AddDays(interval * (i + 1));
                    reminderDateTime = reminderDateTime.AddDays(interval * (i + 1));
                }
                else if (unit == "Weeks")
                {
                    dueDate = commitment.DueDate!.Value.AddDays(interval * (7 * (i + 1)));
                    reminderDateTime = reminderDateTime.AddDays(interval * (7 * (i + 1)));
                }
                else if (unit == "Months")
                {
                    dueDate = commitment.DueDate!.Value.AddMonths(interval * (i + 1));
                    reminderDateTime = reminderDateTime.AddMonths(interval * (i + 1));
                }
                else
                {
                    dueDate = commitment.DueDate!.Value.AddYears(interval * (i + 1));
                    reminderDateTime = reminderDateTime.AddYears(interval * (i + 1));
                }

                var recurringInstance = new RecurringCommitment
                {
                    Name = commitment.Name,
                    Notes = commitment.Notes,
                    IsCompleted = false,
                    Priority = commitment.Priority,
                    DueDate = dueDate,
                    SubtasksSerialized = null,
                    ReminderSet = commitment.ReminderSet,
                    RecurUnit = commitment.RecurUnit,
                    RecurInterval = commitment.RecurInterval,
                    RecurrenceStart = commitment.RecurrenceStart,
                    RecurUntil = commitment.RecurUntil,
                };

                await _mediator.Send(new AddEntityToDatabaseCommand<RecurringCommitment>(recurringInstance));

                if (taskReminderDate != null)
                {
                    var reminder = new Reminder
                    {
                        CommitmentId = recurringInstance.Id,
                        ReminderTime = reminderDateTime,
                        Status = "Oczekiwanie"
                    };

                    await _mediator.Send(new AddEntityToDatabaseCommand<Reminder>(reminder));
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
        public async Task<IActionResult> GetTodaysTasks()
        {
            var (todayOneTimeCommitments, todaysRecurringCommitments) = await _mediator.Send(new GetTodaysCommitmentsQuery());

            var allTodaysCommitments = todayOneTimeCommitments.Select(c => new {
                                            Name = c.Name,
                                            DueDate = c.DueDate,
                                            IsCompleted = c.IsCompleted,
                                            Priority = c.Priority.HasValue ? c.Priority.ToString() : "Brak",
                                            ReminderSet = c.ReminderSet
                                        })
                                        .Concat(todaysRecurringCommitments.Select(c => new
                                        {
                                            Name = c.Name,
                                            DueDate = c.DueDate,
                                            IsCompleted = c.IsCompleted,
                                            Priority = c.Priority.HasValue ? c.Priority.ToString() : "Brak",
                                            ReminderSet = c.ReminderSet
                                        }))
                                        .AsEnumerable()
                                        .ToList();

            return Ok(allTodaysCommitments);
        }
    }
}
