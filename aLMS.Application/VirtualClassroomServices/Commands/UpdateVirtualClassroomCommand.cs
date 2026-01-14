using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.VirtualClassroomServices.Commands
{
    public class UpdateVirtualClassroomCommand : IRequest<bool>
    {
        public UpdateVirtualClassroomDto Dto { get; set; }
    }

    public class UpdateVirtualClassroomCommandHandler : IRequestHandler<UpdateVirtualClassroomCommand, bool>
    {
        private readonly IVirtualClassroomRepository _repo;
        private readonly IMapper _mapper;

        public UpdateVirtualClassroomCommandHandler(IVirtualClassroomRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateVirtualClassroomCommand request, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(request.Dto.Id);
            if (existing == null)
                return false;

            _mapper.Map(request.Dto, existing);
            existing.RaiseUpdatedEvent();

            await _repo.UpdateAsync(existing);
            return true;
        }
    }
}