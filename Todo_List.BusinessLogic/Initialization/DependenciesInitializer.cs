using Microsoft.Extensions.DependencyInjection;
using Todo_List.BusinessLogic.Commands.AddEntityToDatabase;
using Todo_List.Infrastructure.Entities;

namespace Todo_List.BusinessLogic.Initialization
{
    public static class DependenciesInitializer
    {
        public static void InitializeBusinessLogicDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddEntityToDatabaseCommand<Log>).Assembly));
        }
    }
}
