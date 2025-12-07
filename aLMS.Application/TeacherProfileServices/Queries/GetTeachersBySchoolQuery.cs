using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.TeacherProfileServices.Queries
{
    public class GetTeachersBySchoolQuery : IRequest<List<TeacherProfileDto>>
    {
        public Guid SchoolId { get; set; }
    }

    public class GetTeachersBySchoolQueryHandler : IRequestHandler<GetTeachersBySchoolQuery, List<TeacherProfileDto>>
    {
        private readonly ITeacherProfileRepository _profileRepo;
        private readonly IMapper _mapper;

        public GetTeachersBySchoolQueryHandler(ITeacherProfileRepository profileRepo, IMapper mapper)
        {
            _profileRepo = profileRepo;
            _mapper = mapper;
        }

        public async Task<List<TeacherProfileDto>> Handle(GetTeachersBySchoolQuery request, CancellationToken ct)
        {
            var teachers = await _profileRepo.GetBySchoolIdAsync(request.SchoolId);
            return _mapper.Map<List<TeacherProfileDto>>(teachers);
        }
    }
}
