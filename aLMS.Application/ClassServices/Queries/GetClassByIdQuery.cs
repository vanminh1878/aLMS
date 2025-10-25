using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Queries
{
    public class GetClassByIdQuery : IRequest<ClassDto>
    {
        public Guid Id { get; set; }
    }

    public class GetClassByIdQueryHandler : IRequestHandler<GetClassByIdQuery, ClassDto>
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public GetClassByIdQueryHandler(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<ClassDto> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
        {
            var classEntity = await _classRepository.GetClassByIdAsync(request.Id);
            return _mapper.Map<ClassDto>(classEntity);
        }
    }
}