using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.VirtualClassroomServices.Queries
{
    public class GetVirtualClassroomByIdQuery : IRequest<VirtualClassroomDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetVirtualClassroomByIdQueryHandler : IRequestHandler<GetVirtualClassroomByIdQuery, VirtualClassroomDto?>
    {
        private readonly IVirtualClassroomRepository _repo;
        private readonly IMapper _mapper;

        public GetVirtualClassroomByIdQueryHandler(IVirtualClassroomRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<VirtualClassroomDto?> Handle(GetVirtualClassroomByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            return entity == null ? null : _mapper.Map<VirtualClassroomDto>(entity);
        }
    }
}