using MediatR;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;
using Todo_List.Infrastructure.Repositories.Interfaces;

namespace Todo_List.BusinessLogic.Queries.GetCommitmentById
{
    public class GetCommitmentByIdQueryHandler : IRequestHandler<GetCommitmentByIdQuery, Commitment>
    {
        private readonly IGenericRepository<Commitment> _repository;

        public GetCommitmentByIdQueryHandler(IGenericRepository<Commitment> repository)
        {
            _repository = repository;
        }

        public async Task<Commitment> Handle(GetCommitmentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.TaskId);
        }
    }
}
