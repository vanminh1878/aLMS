// GetFinalTermRecordsByClassQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using MediatR;

namespace aLMS.Application.FinalTermRecordServices.Queries
{
    public class GetFinalTermRecordsByClassQuery : IRequest<List<FinalTermRecordDto>>
    {
        public Guid ClassId { get; set; }
    }

    public class GetFinalTermRecordsByClassQueryHandler : IRequestHandler<GetFinalTermRecordsByClassQuery, List<FinalTermRecordDto>>
    {
        private readonly IFinalTermRecordRepository _repository;

        public GetFinalTermRecordsByClassQueryHandler(IFinalTermRecordRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FinalTermRecordDto>> Handle(GetFinalTermRecordsByClassQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByClassIdAsync(request.ClassId);
        }
    }
}