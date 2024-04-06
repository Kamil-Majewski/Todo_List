using Microsoft.Extensions.DependencyInjection;
using Todo_List.BusinessLogic.Services;
using Todo_List.BusinessLogic.Services.Interfaces;

namespace Todo_List.BusinessLogic.Initialization
{
    public static class DependenciesInitializer
    {
        public static void InitializeBusinessLogicDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            serviceCollection.AddScoped<IOneTimeCommitmentService, OneTimeCommitmentService>();
            serviceCollection.AddScoped<IUnscheduledCommitmentService, UnscheduledCommitmentService>();
            serviceCollection.AddScoped<IRecurringCommitmentService, RecurringCommitmentService>();
            serviceCollection.AddScoped<ILogService, LogService>();
            serviceCollection.AddScoped<IReminderService, ReminderService>();
        }
    }
}
