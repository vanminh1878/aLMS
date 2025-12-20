using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Queries
{
    public class GetClassesByStudentIdQuery : IRequest<IEnumerable<ClassDto>>
    {
        public Guid StudentId { get; set; }
    }

    public class GetClassesByStudentIdQueryHandler : IRequestHandler<GetClassesByStudentIdQuery, IEnumerable<ClassDto>>
    {
        private readonly IClassRepository _classRepository;
        private readonly IStudentProfileRepository _studentProfileRepository;
        private readonly IMapper _mapper; 

        public GetClassesByStudentIdQueryHandler(
            IClassRepository classRepository,
            IStudentProfileRepository studentProfileRepository,
            IMapper mapper)
        {
            _classRepository = classRepository;
            _studentProfileRepository = studentProfileRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassDto>> Handle(GetClassesByStudentIdQuery request, CancellationToken cancellationToken)
        {
            var studentExists = await _studentProfileRepository.ExistsAsync(request.StudentId);
            if (!studentExists)
            {
                return Enumerable.Empty<ClassDto>();
            }
            var classes = await _classRepository.GetClassesByStudentIdAsync(request.StudentId);
            return _mapper.Map<IEnumerable<ClassDto>>(classes);
        }
    }
}
