using Microsoft.Extensions.DependencyInjection;
using Todo_List.Infrastructure.Repositories;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.Infrastructure.Initialization
{
    public static class DependenciesInitializer
    {
        public static void InitializeInfrastructureDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
