using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.VirtualClassroomServices.Queries
{
    public class GetVirtualClassroomsByClassQuery : IRequest<IEnumerable<VirtualClassroomDto>>
    {
        public Guid ClassId { get; set; }
        public bool UpcomingOnly { get; set; }
    }

    public class GetVirtualClassroomsByClassQueryHandler : IRequestHandler<GetVirtualClassroomsByClassQuery, IEnumerable<VirtualClassroomDto>>
    {
        private readonly IVirtualClassroomRepository _repo;
        private readonly IMapper _mapper;

        public GetVirtualClassroomsByClassQueryHandler(IVirtualClassroomRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VirtualClassroomDto>> Handle(GetVirtualClassroomsByClassQuery request, CancellationToken ct)
        {
            var entities = await _repo.GetByClassIdAsync(request.ClassId, request.UpcomingOnly);
            return _mapper.Map<IEnumerable<VirtualClassroomDto>>(entities);
        }
    }
}