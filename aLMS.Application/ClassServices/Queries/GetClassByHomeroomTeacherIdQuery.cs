using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.ClassServices.Queries
{
    public class GetClassByHomeroomTeacherIdQuery : IRequest<ClassDto?>
    {
        public Guid HomeroomTeacherId { get; set; }
    }

    public class GetClassByHomeroomTeacherIdQueryHandler : IRequestHandler<GetClassByHomeroomTeacherIdQuery, ClassDto?>
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public GetClassByHomeroomTeacherIdQueryHandler(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<ClassDto?> Handle(GetClassByHomeroomTeacherIdQuery request, CancellationToken cancellationToken)
        {
            var classEntity = await _classRepository.GetClassByHomeroomTeacherIdAsync(request.HomeroomTeacherId);
            return classEntity == null ? null : _mapper.Map<ClassDto>(classEntity);
        }
    }
}