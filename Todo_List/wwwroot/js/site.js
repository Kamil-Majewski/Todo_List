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
            url: '/Endpoints/GetAllTasks/',
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