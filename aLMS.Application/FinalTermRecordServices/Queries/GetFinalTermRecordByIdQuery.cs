// GetFinalTermRecordByIdQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace aLMS.Application.FinalTermRecordServices.Queries
{
    public class GetFinalTermRecordByIdQuery : IRequest<FinalTermRecordDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetFinalTermRecordByIdQueryHandler : IRequestHandler<GetFinalTermRecordByIdQuery, FinalTermRecordDto?>
    {
        private readonly IFinalTermRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetFinalTermRecordByIdQueryHandler(IFinalTermRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<FinalTermRecordDto?> Handle(GetFinalTermRecordByIdQuery request, CancellationToken cancellationToken)
        {
            var record = await _repository.GetByIdAsync(request.Id);
            return record;
        }
    }
}