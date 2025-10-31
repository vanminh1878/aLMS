// aLMS.Application.ParentProfileServices.Queries/GetParentProfileQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ParentProfileServices.Queries
{
    public class GetParentProfileQuery : IRequest<ParentProfileDto>
    {
        public Guid ParentId { get; set; }
    }

    public class GetChildrenByParentQuery : IRequest<IEnumerable<ParentProfileDto>>
    {
        public Guid ParentId { get; set; }
    }

    public class GetParentsByStudentQuery : IRequest<IEnumerable<ParentProfileDto>>
    {
        public Guid StudentId { get; set; }
    }

    public class GetParentProfileQueryHandler :
        IRequestHandler<GetParentProfileQuery, ParentProfileDto>,
        IRequestHandler<GetChildrenByParentQuery, IEnumerable<ParentProfileDto>>,
        IRequestHandler<GetParentsByStudentQuery, IEnumerable<ParentProfileDto>>
    {
        private readonly IParentProfileRepository _profileRepo;
        private readonly IMapper _mapper;

        public GetParentProfileQueryHandler(IParentProfileRepository profileRepo, IMapper mapper)
        {
            _profileRepo = profileRepo;
            _mapper = mapper;
        }

        public async Task<ParentProfileDto> Handle(GetParentProfileQuery request, CancellationToken ct)
        {
            var profile = await _profileRepo.GetByParentIdAsync(request.ParentId);
            return profile == null ? null : _mapper.Map<ParentProfileDto>(profile);
        }

        public async Task<IEnumerable<ParentProfileDto>> Handle(GetChildrenByParentQuery request, CancellationToken ct)
        {
            var profiles = await _profileRepo.GetByParentIdAsync(request.ParentId);
            return _mapper.Map<IEnumerable<ParentProfileDto>>(profiles);
        }

        public async Task<IEnumerable<ParentProfileDto>> Handle(GetParentsByStudentQuery request, CancellationToken ct)
        {
            var profiles = await _profileRepo.GetByStudentIdAsync(request.StudentId);
            return _mapper.Map<IEnumerable<ParentProfileDto>>(profiles);
        }
    }
}