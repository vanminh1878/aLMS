using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Queries
{
    public class GetAllClassesQuery : IRequest<IEnumerable<ClassDto>>
    {
    }

    public class GetAllClassesQueryHandler : IRequestHandler<GetAllClassesQuery, IEnumerable<ClassDto>>
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public GetAllClassesQueryHandler(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassDto>> Handle(GetAllClassesQuery request, CancellationToken cancellationToken)
        {
            var classes = await _classRepository.GetAllClassesAsync();
            return _mapper.Map<IEnumerable<ClassDto>>(classes);
        }
    }
}