// aLMS.Application.TeacherProfileServices.Queries/GetTeacherProfileQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.TeacherProfileServices.Queries
{
    public class GetTeacherProfileQuery : IRequest<TeacherProfileDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetTeacherProfileQueryHandler : IRequestHandler<GetTeacherProfileQuery, TeacherProfileDto>
    {
        private readonly ITeacherProfileRepository _profileRepo;
        private readonly IMapper _mapper;

        public GetTeacherProfileQueryHandler(ITeacherProfileRepository profileRepo, IMapper mapper)
        {
            _profileRepo = profileRepo;
            _mapper = mapper;
        }

        public async Task<TeacherProfileDto> Handle(GetTeacherProfileQuery request, CancellationToken ct)
        {
            var profile = await _profileRepo.GetByUserIdAsync(request.UserId);
            return profile == null ? null : _mapper.Map<TeacherProfileDto>(profile);
        }
    }
}