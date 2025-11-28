using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.BehaviourServices.Queries
{
    public class GetBehavioursByStudentQuery : IRequest<IEnumerable<BehaviourDto>>
    {
        public Guid StudentId { get; set; }
    }

    public class GetBehaviourByIdQuery : IRequest<BehaviourDto>
    {
        public Guid Id { get; set; }
    }

    public class BehaviourQueryHandler :
        IRequestHandler<GetBehavioursByStudentQuery, IEnumerable<BehaviourDto>>,
        IRequestHandler<GetBehaviourByIdQuery, BehaviourDto>
    {
        private readonly IBehaviourRepository _repo;
        private readonly IMapper _mapper;

        public BehaviourQueryHandler(IBehaviourRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BehaviourDto>> Handle(GetBehavioursByStudentQuery request, CancellationToken ct)
        {
            var behaviours = await _repo.GetByStudentIdAsync(request.StudentId);
            return _mapper.Map<IEnumerable<BehaviourDto>>(behaviours);
        }

        public async Task<BehaviourDto> Handle(GetBehaviourByIdQuery request, CancellationToken ct)
        {
            var behaviour = await _repo.GetByIdAsync(request.Id);
            return behaviour == null ? null : _mapper.Map<BehaviourDto>(behaviour);
        }
    }
}