// GetFinalTermRecordsByStudentQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace aLMS.Application.FinalTermRecordServices.Queries
{
    public class GetFinalTermRecordsByStudentQuery : IRequest<List<FinalTermRecordDto>>
    {
        public Guid StudentProfileId { get; set; }
    }

    public class GetFinalTermRecordsByStudentQueryHandler : IRequestHandler<GetFinalTermRecordsByStudentQuery, List<FinalTermRecordDto>>
    {
        private readonly IFinalTermRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetFinalTermRecordsByStudentQueryHandler(IFinalTermRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FinalTermRecordDto>> Handle(GetFinalTermRecordsByStudentQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetByStudentIdAsync(request.StudentProfileId);
            return records;
        }
    }
}