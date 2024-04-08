using MediatR;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Commands.DeleteCommitmentById
{
    public class DeleteCommitmentByIdCommandHandler : IRequestHandler<DeleteCommitmentByIdCommand, Unit>
    {
        private readonly IGenericRepository<Commitment> _repository;

        public DeleteCommitmentByIdCommandHandler(IGenericRepository<Commitment> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteCommitmentByIdCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteEntityAsync(request.CommitmentId);
            return Unit.Value;
        }
    }
}
