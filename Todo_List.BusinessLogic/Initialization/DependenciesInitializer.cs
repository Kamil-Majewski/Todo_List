﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Todo_List.BusinessLogic.Commands.AddEntityToDatabase;
using Todo_List.BusinessLogic.Commands.DeleteCommitmentById;
using Todo_List.BusinessLogic.Commands.DeleteEntity;
using Todo_List.Infrastructure.Entities;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;

namespace Todo_List.BusinessLogic.Initialization
{
    public static class DependenciesInitializer
    {
        public static void InitializeBusinessLogicDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteCommitmentByIdCommand).Assembly));
            serviceCollection
                .AddTransient<IRequestHandler<AddEntityToDatabaseCommand<UnscheduledCommitment>, UnscheduledCommitment>, AddEntityToDatabaseCommandHandler<UnscheduledCommitment>>()
                .AddTransient<IRequestHandler<AddEntityToDatabaseCommand<RecurringCommitment>, RecurringCommitment>, AddEntityToDatabaseCommandHandler<RecurringCommitment>>()
                .AddTransient<IRequestHandler<AddEntityToDatabaseCommand<OneTimeCommitment>, OneTimeCommitment>, AddEntityToDatabaseCommandHandler<OneTimeCommitment>>()
                .AddTransient<IRequestHandler<AddEntityToDatabaseCommand<Reminder>, Reminder>, AddEntityToDatabaseCommandHandler<Reminder>>();

        }
    }
}
