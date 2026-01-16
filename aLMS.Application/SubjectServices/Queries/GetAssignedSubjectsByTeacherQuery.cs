using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.DTOs;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.SubjectServices.Queries
{
    public class GetAssignedSubjectsByTeacherQuery : IRequest<List<AssignedSubjectDto>>
    {
        public Guid TeacherId { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class GetAssignedSubjectsByTeacherQueryHandler : IRequestHandler<GetAssignedSubjectsByTeacherQuery, List<AssignedSubjectDto>>
    {
        private readonly ISubjectRepository _repo;
        // private readonly IMapper _mapper; 

        public GetAssignedSubjectsByTeacherQueryHandler(ISubjectRepository repo)
        {
            _repo = repo;
            // _mapper = mapper; 
        }

        public async Task<List<AssignedSubjectDto>> Handle(
            GetAssignedSubjectsByTeacherQuery request,
            CancellationToken cancellationToken)
        {
            var assignedSubjects = await _repo.GetAssignedSubjectsByTeacherAsync(
                request.TeacherId,
                request.SchoolYear);
            return assignedSubjects;
        }
    }
}