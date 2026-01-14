using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.VirtualClassroomServices.Queries
{
    public class GetVirtualClassroomsByStudentQuery : IRequest<IEnumerable<VirtualClassroomDto>>
    {
        public Guid StudentId { get; set; }
        public bool UpcomingOnly { get; set; }
    }

    public class GetVirtualClassroomsByStudentQueryHandler : IRequestHandler<GetVirtualClassroomsByStudentQuery, IEnumerable<VirtualClassroomDto>>
    {
        private readonly IVirtualClassroomRepository _repo;
        private readonly IMapper _mapper;

        public GetVirtualClassroomsByStudentQueryHandler(IVirtualClassroomRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VirtualClassroomDto>> Handle(GetVirtualClassroomsByStudentQuery request, CancellationToken ct)
        {
            var entities = await _repo.GetByStudentIdAsync(request.StudentId, request.UpcomingOnly);
            return _mapper.Map<IEnumerable<VirtualClassroomDto>>(entities);
        }
    }
}