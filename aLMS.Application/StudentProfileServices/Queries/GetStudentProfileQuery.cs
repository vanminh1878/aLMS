using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentProfileServices.Queries
{
    public class GetStudentProfileQuery : IRequest<StudentProfileDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetStudentProfileQueryHandler : IRequestHandler<GetStudentProfileQuery, StudentProfileDto>
    {
        private readonly IStudentProfileRepository _profileRepo;
        private readonly IMapper _mapper;

        public GetStudentProfileQueryHandler(IStudentProfileRepository profileRepo, IMapper mapper)
        {
            _profileRepo = profileRepo;
            _mapper = mapper;
        }

        public async Task<StudentProfileDto> Handle(GetStudentProfileQuery request, CancellationToken ct)
        {
            var profile = await _profileRepo.GetByUserIdAsync(request.UserId);
            return profile == null ? null : _mapper.Map<StudentProfileDto>(profile);
        }
    }
}