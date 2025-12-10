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
    public class GetTeachersByDepartmentQuery : IRequest<List<TeacherProfileDto>>
    {
        public Guid DepartmentId { get; set; }
    }

    public class GetTeachersByDepartmentQueryHandler : IRequestHandler<GetTeachersByDepartmentQuery, List<TeacherProfileDto>>
    {
        private readonly ITeacherProfileRepository _profileRepo;
        private readonly IMapper _mapper;

        public GetTeachersByDepartmentQueryHandler(ITeacherProfileRepository profileRepo, IMapper mapper)
        {
            _profileRepo = profileRepo;
            _mapper = mapper;
        }

        public async Task<List<TeacherProfileDto>> Handle(GetTeachersByDepartmentQuery request, CancellationToken ct)
        {
            var teachers = await _profileRepo.GetByDepartmentIdAsync(request.DepartmentId);
            return _mapper.Map<List<TeacherProfileDto>>(teachers);
        }
    }
}
