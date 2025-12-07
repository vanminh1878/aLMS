using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.StudentProfileServices.Queries
{
    public class GetStudentsByClassIdQuery : IRequest<List<StudentProfileDto>>
    {
        public Guid ClassId { get; set; }
    }

    public class GetStudentsByClassIdQueryHandler : IRequestHandler<GetStudentsByClassIdQuery, List<StudentProfileDto>>
    {
        private readonly IStudentProfileRepository _profileRepo;
        private readonly IMapper _mapper;

        public GetStudentsByClassIdQueryHandler(IStudentProfileRepository profileRepo, IMapper mapper)
        {
            _profileRepo = profileRepo;
            _mapper = mapper;
        }

        public async Task<List<StudentProfileDto>> Handle(GetStudentsByClassIdQuery request, CancellationToken ct)
        {
            var profiles = await _profileRepo.GetByClassIdAsync(request.ClassId);
            return _mapper.Map<List<StudentProfileDto>>(profiles);
        }
    }
}
