var calendar;

function initializeCalendar() {
    var calendarEl = document.getElementById('calendar-render');

    getEvents().then(events => {
        calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridMonth',
            headerToolbar: {
                left: 'prev today next',
                center: 'title',
                right: 'timeGridWeek dayGridMonth',
            },
            buttonText: {
                today: 'Dzisiaj',
                dayGridMonth: "Miesiąc",
                timeGridWeek: "Tydzień"

            },
            dayMaxEvents: true,
            dayMaxEvents: 1,
            moreLinkContent: function (args) {
                return '+' + args.num + ' więcej';
            },
            allDaySlot: false,
            locale: "pl",
            height: 580,
            businessHours: true,
            selectable: true,
            dateClick: function (info) {
                getTasksForCertainDay(info.dateStr);
            },
            eventColor: "#6f5cc4",
            events: events
        });

        calendar.render();
        calendar.next();
        calendar.prev();
    }).catch(error => {
        console.error(error);
    });

}

function getEvents() {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/Endpoints/GetAllTasks',
            type: 'GET',
            contentType: 'application/json',
            success: function (listOfTasks) {
                var events = listOfTasks.map(task => ({
                    event_id: task.id,
                    title: task.name,
                    start: task.dueDate == undefined ? null : new Date(task.dueDate).toISOString(),
                    color: "#6f5cc4"
                }));
                resolve(events);
            },
            error: function (error) {
                reject(error);
            }
        });
    });
}

function addNewTask(prefix, endpoint, isToday) {
    var name = $(`#${prefix}-task-name`).val();
    var priority = $(`#${prefix}-task-priority`).val();
    var notes = $(`#${prefix}-task-notes`).val();
    var dueDate = $(`#${prefix}-start-date`).val();
    var recurrenceUnit = $(`#${prefix}-recurrence-unit`).val();
    var recurrenceInterval = $(`#${prefix}-recurrence-interval`).val();
    var recurStart = $(`#${prefix}-reccur-start`).val();
    var recurUntil = $(`#${prefix}-reccur-until`).val();
    var reminderDate = $(`#${prefix}-reminder-date`).val();

    $.ajax({
        url: `/Endpoints/${endpoint}`,
        type: 'GET',
        contentType: 'application/json',
        data: { taskName: name, taskPriority: priority, taskNotes: notes, taskDueDate: dueDate, taskRecurrenceUnit: recurrenceUnit, taskRecurrenceInterval: recurrenceInterval, taskRecurStart: recurStart, taskRecurUntil: recurUntil, taskReminderDate: reminderDate },
        success: function (task) {
            if (isToday) {
                var date = new Date().toLocaleDateString();
            }
            else {
                var date = new Date($("#certain-days-tasks-table").attr("date")).toLocaleDateString();
            }
            
            if (task.dueDate != null && new Date(task.dueDate).toLocaleDateString() == date) {
                addTaskToTable(task, `${prefix}-tasks-table`, `${prefix}`);
            }
        },
        error: function (error) {
            console.error(error.responseText);
        }
    });
}

function addListenersForForm(prefix) {
    $(`#${prefix}-start-date`).change(function () {
        if ($(`#${prefix}-start-date`).val() == "") {
            $(`#${prefix}-reminder-date`).val("").prop("disabled", true);
            $(`#${prefix}-recurrence-interval`).val("").prop('disabled', true);
            $(`#${prefix}-recurrence-unit`).prop('selectedIndex', 0);
            $(`#${prefix}-recurrence-unit`).val("").prop("disabled", true);
            $(`#${prefix}-reccur-start`).val("").prop('disabled', true);
            $(`#${prefix}-reccur-until`).val("").prop('disabled', true);
        }
        else {
            $(`#${prefix}-reminder-date`).prop("disabled", false).prop('max', `${$(`#${prefix}-start-date`).val()}`);
            $(`#${prefix}-recurrence-unit`).prop("disabled", false);
            $(`#${prefix}-reccur-start`).prop('min', `${$(`#${prefix}-start-date`).val().split('T')[0]}`);
        }
    });
    $(`#${prefix}-recurrence-unit`).change(function () {
        if ($(`#${prefix}-recurrence-unit`).val() == -1) {
            $(`#${prefix}-recurrence-interval`).val("").prop('disabled', true);
            $(`#${prefix}-reccur-start`).val("").prop('disabled', true);
            $(`#${prefix}-reccur-until`).val("").prop('disabled', true);
        }
        else {
            $(`#${prefix}-recurrence-interval`).prop('disabled', false);
        }
    });

    $(`#${prefix}-recurrence-interval`).change(function () {
        if ($(`#${prefix}-recurrence-interval`).val() == "") {
            $(`#${prefix}-reccur-start`).val("").prop('disabled', true);
            $(`#${prefix}-reccur-until`).val("").prop('disabled', true);
        }
        else {
            $(`#${prefix}-reccur-start`).prop('disabled', false);
        }

    })
    $(`#${prefix}-reccur-start`).change(function () {
        if ($(`#${prefix}-reccur-start`).val() == "") {
            $(`#${prefix}s-reccur-until`).val("").prop('disabled', true);
        }
        else {
            $(`#${prefix}-reccur-until`).prop('disabled', false).prop('min', `${$(`#${prefix}-reccur-start`).val().split('T')[0]}`);;
        }
    });
}

function openCertainDaysList(date) {
    $(".calendar-window").hide();
    $("#certain-days-tasks-table").attr("date", date);
    if (date >= new Date().toLocaleDateString('fr-ca')) {
        $("#certain-new-button").show();
    }
    $(".certain-days-container").show();
}

function getTasksForCertainDay(date) {
    $.ajax({
        url: '/Endpoints/GetCommitmentsForCertainDay',
        type: 'GET',
        data: { date },
        contentType: 'application/json',
        success: function (listOfTasks) {
            if (listOfTasks.length == 0 || listOfTasks.length == undefined) {
                console.log("No tasks found")
                openCertainDaysList(date);
                return;
            }
            listOfTasks.forEach(task => {
                addTaskToTable(task, "certain-days-tasks-table", "certain-days");

            });
            openCertainDaysList(date);
        },
        error: function (error) {
            console.error(error.responseText);
        }
    });
}

function openCreateTaskWindow() {
    $(".today-container").hide();
    $(".add-new-task").show();
    $("#todays-input-window-title").html("Utwórz zadanie");
    $("#createOrEditTodaysButton").html("Utwórz");
    var currentTime = new Date();
    currentTime.setMinutes(currentTime.getMinutes() - currentTime.getTimezoneOffset());
    currentTime.setMinutes(currentTime.getMinutes() + 10);

    var formattedDateTime = currentTime.toISOString().slice(0, 16);

    $("#todays-start-date").attr("min", `${formattedDateTime}`)
}

function openCreateTaskWindowCertain() {
    $(".add-new-task-certain-days").show();
    $(".certain-days-container").hide();
    $("#certain-days-input-window-title").html("Utwórz zadanie");
    $("#createOrEditCertainDaysButton").html("Utwórz");

    var currentTime = new Date();
    currentTime.setMinutes(currentTime.getMinutes() - currentTime.getTimezoneOffset());

    if (currentTime.toLocaleDateString() == $("#certain-days-tasks-table").attr('date')) {
        currentTime.setMinutes(currentTime.getMinutes() + 10);
        var time = currentTime.toISOString().slice(11, 16);
        var formattedDateTime = $("#certain-days-tasks-table").attr('date') + "T" + time;
    }
    else {
        var formattedDateTime = $("#certain-days-tasks-table").attr('date') + "T00:00"
    }

    $("#certain-days-start-date").attr("min", `${formattedDateTime}`)
}

function addTaskToTable(task, tableId, prefix) {
    var dueTimeParsed = new Date(task.dueDate);
    var currentDate = new Date();

    var priority = task.priority;

    if (priority != null) {
        if (typeof priority == "number") {
            priority = priorityMappings[priority];
        }
    }
    else {
        priority = "Brak";
    }

    const isCompleted = (currentDate > dueTimeParsed)
        ? (task.isCompleted) ? `
            <div class="round">
                <input class="completed-task-checkbox" type="Checkbox" id="checkbox-${task.id}" checked disabled/>
                <label for="checkbox-${task.id}"></label>
            <div>`
            : `
            <div class="round-red">
                <input class="failed-task-checkbox" type="Checkbox" id="checkbox-${task.id}" checked disabled/>
                <label for="checkbox-${task.id}"></label>
            <div>`
        : (task.isCompleted) ? `
            <div class="round">
                <input class="completed-task-checkbox" type="Checkbox" id="checkbox-${task.id}" checked disabled/>
                <label for="checkbox-${task.id}"></label>
            <div>`
            : `
            <div class="round">
                <input class="complete-task-checkbox" type="Checkbox" id="checkbox-${task.id}"/>
                <label for="checkbox-${task.id}"></label>
            <div>
            `

    const reminder = task.reminderSet ? `${new Date(task.reminderTime).toLocaleString('pl-PL').split(", ")[1]}` : "Nie ustawiono";

    const options = (currentDate > dueTimeParsed)
        ? (task.isCompleted) ? `
            <td class="centered-cell">
                <a href="#details-${task.id}" class="icon-list ${prefix}-details"><img src="/icons/view-doc.png" title="Szczegóły"/></a>
            </td>
            `
            : `
            <td class="centered-cell">
                <a href="#details-${task.id}" class="icon-list ${prefix}-details"><img src="/icons/view-doc.png" title="Szczegóły"/></a>
            </td>`
        : (task.isCompleted) ? `
            <td class="centered-cell">
                <a href="#details-${task.id}" class="icon-list ${prefix}-details"><img src="/icons/view-doc.png" title="Szczegóły"/></a>
            </td>
            `
            : `
            <td class="centered-cell">
                <a href="#details-${task.id}" class="icon-list ${prefix}-details"><img src="/icons/view-doc.png" title="Szczegóły"/></a>
                <a href="#edit-${task.id}" class="icon-list ${prefix}-edit"><img src="/icons/edit.png" title="Edytuj"/></a>
                <a href="#delete-${task.id}" class="icon-list todays-delete"><img src="/icons/trash.png" title="Usuń"/></a>
            </td>
            `


    var newRow = `
            <tr>
                <td>
                    ${isCompleted}
                </td>
                <td class="task-name">${task.name}</td>
                <td>${new Date(task.dueDate).toLocaleString('pl-PL').split(",")[1]}</td>
                <td>${reminder}</td>
                <td>${priority}</td>
                ${options}
            </tr>
        `;

    $(`#${tableId} tbody`).append(newRow);
}

function OnInputFilterTable(searchBarIdentification, tableIdentification) {
    $(searchBarIdentification).on('input', function () {

        var filterText = $(this).val().toLowerCase();

        $(`${tableIdentification} tbody tr`).each(function () {
            var rowHit = false;
            $(this).find('td').each(function () {
                if ($(this).text().toLowerCase().includes(filterText)) {
                    rowHit = true;
                    return false;
                }
            });

            $(this).toggle(rowHit);
        });
    });
}
function goBackToTodaysList() {
    $(".todays-options-form :input[type='text']").val('');
    $(".todays-options-form :input[type='datetime-local']").val('');
    $(".todays-options-form :input[type='date']").val('');
    $(".todays-options-form :input[type='number']").val('');
    $(".todays-options-form textarea").val('');
    $(".todays-options-form select").prop('selectedIndex', 0);
    $("#todays-recurrence-unit").prop('disabled', true);
    $("#todays-recurrence-interval").prop('disabled', true);
    $("#todays-reminder-date").prop("disabled", true);
    $("#todays-reccur-start").prop("disabled", true);
    $("#todays-reccur-until").prop("disabled", true);
    $("#createOrEditTodaysButton").html("");
    $("#createOrEditTodaysButton").removeAttr("entity-id")

    $(".add-new-task").hide();
    $(".todays-task-details-container").hide();
    $(".recurrence-details").show();
    $(".today-container").show();
}

function goBackToCalendar() {
    $(".certain-days-container").hide();
    $("#certain-new-button").hide();
    $("#certain-days-tasks-table tbody").empty();
    $(".calendar-window").show();
    calendar.render();
}

function goBackToCertainDaysList() {
    $(".certain-days-options-form :input[type='text']").val('');
    $(".certain-days-options-form :input[type='datetime-local']").val('');
    $(".certain-days-options-form :input[type='date']").val('');
    $(".certain-days-options-form :input[type='number']").val('');
    $(".certain-days-options-form textarea").val('');
    $(".certain-days-options-form select").prop('selectedIndex', 0);
    $("#certain-days-recurrence-unit").prop('disabled', true);
    $("#certain-days-recurrence-interval").prop('disabled', true);
    $("#certain-days-reminder-date").prop("disabled", true);
    $("#certain-days-reccur-start").prop("disabled", true);
    $("#certain-days-reccur-until").prop("disabled", true);
    $("#createOrEditCertainDaysButton").html("");
    $("#createOrEditCertainDaysButton").removeAttr("entity-id")

    $(".add-new-task-certain-days").hide();
    $(".certain-days-task-details-container").hide();
    $(".recurrence-details-certain").show();
    $(".certain-days-container").show();
}

function deleteTask(taskId) {
    $.ajax({
        url: '/Endpoints/DeleteTask',
        type: 'GET',
        data: { taskId },
        contentType: 'application/json',
        success: function () {

        },
        error: function (error) {
            console.error(error.responseText);
        }
    })
}

function getTodaysTasks() {
    $.ajax({
        url: '/Endpoints/GetTodaysTasks',
        type: 'GET',
        contentType: 'application/json',
        success: function (listOfTasks) {
            if (listOfTasks.length == 0 || listOfTasks.length == undefined) {
                console.log("No tasks found")
                return;
            }
            listOfTasks.forEach(task => {
                addTaskToTable(task, "todays-tasks-table", "todays");

            });
        },
        error: function (error) {
            console.error(error.responseText);
        }
    });
}

function updateTask(taskId, prefix) {

    var name = $(`#${prefix}-task-name`).val();
    var priority = $(`#${prefix}-task-priority`).val();
    var notes = $(`#${prefix}-task-notes`).val();
    var dueDate = $(`#${prefix}-start-date`).val();
    var recurrenceUnit = $(`#${prefix}-recurrence-unit`).val();
    var recurrenceInterval = $(`#${prefix}-recurrence-interval`).val();
    var recurStart = $(`#${prefix}-reccur-start`).val();
    var recurUntil = $(`#${prefix}-reccur-until`).val();
    var reminderDate = $(`#${prefix}-reminder-date`).val();

    $.ajax({
        url: '/Endpoints/UpdateCommitment',
        type: 'GET',
        contentType: 'application/json',
        data: { taskId, taskName: name, taskPriority: priority, taskNotes: notes, taskDueDate: dueDate, taskRecurrenceUnit: recurrenceUnit, taskRecurrenceInterval: recurrenceInterval, taskRecurStart: recurStart, taskRecurUntil: recurUntil, taskReminderDate: reminderDate },
        success: function (task) {

            $(`#${prefix}-tasks-table tbody`).empty();
            var date = $("#certain-days-tasks-table").attr("date");
            getTasksForCertainDay(date);
        },
        error: function (error) {
            console.error(error.responseText);
        }
    });

}