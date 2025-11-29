using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace aLMS.Application.ClassServices.Queries
{
    public class GetClassesBySchoolIdQuery : IRequest<IEnumerable<ClassDto>>
    {
        public Guid SchoolId { get; set; }
    }

    public class GetClassesBySchoolIdQueryHandler : IRequestHandler<GetClassesBySchoolIdQuery, IEnumerable<ClassDto>>
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public GetClassesBySchoolIdQueryHandler(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassDto>> Handle(GetClassesBySchoolIdQuery request, CancellationToken cancellationToken)
        {
            var classes = await _classRepository.GetClassesBySchoolIdAsync(request.SchoolId);
            return _mapper.Map<IEnumerable<ClassDto>>(classes);
        }
    }
}