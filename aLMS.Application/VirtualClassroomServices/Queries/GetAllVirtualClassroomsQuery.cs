using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;

namespace aLMS.Application.VirtualClassroomServices.Queries
{
    public class GetAllVirtualClassroomsQuery : IRequest<IEnumerable<VirtualClassroomDto>>
    {
    }

    public class GetAllVirtualClassroomsQueryHandler : IRequestHandler<GetAllVirtualClassroomsQuery, IEnumerable<VirtualClassroomDto>>
    {
        private readonly IVirtualClassroomRepository _repo;
        private readonly IMapper _mapper;

        public GetAllVirtualClassroomsQueryHandler(IVirtualClassroomRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VirtualClassroomDto>> Handle(GetAllVirtualClassroomsQuery request, CancellationToken ct)
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<VirtualClassroomDto>>(entities);
        }
    }
}