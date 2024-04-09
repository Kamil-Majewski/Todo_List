using MediatR;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Commands.UpdateCommitment
{
    public class UpdateCommitmentCommandHandler : IRequestHandler<UpdateCommitmentCommand, Commitment>
    {
        private readonly IGenericRepository<Commitment> _repository;

        public UpdateCommitmentCommandHandler(IGenericRepository<Commitment> repository)
        {
            _repository = repository;
        }

        public async Task<Commitment> Handle(UpdateCommitmentCommand request, CancellationToken cancellationToken)
        {
            return await _repository.UpdateEntityAsync(request.Commitment);
        }
    }
}
