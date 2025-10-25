using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Queries
{
    public class GetAllSchoolsQuery : IRequest<IEnumerable<SchoolDto>>
    {
    }

    public class GetAllSchoolsQueryHandler : IRequestHandler<GetAllSchoolsQuery, IEnumerable<SchoolDto>>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public GetAllSchoolsQueryHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SchoolDto>> Handle(GetAllSchoolsQuery request, CancellationToken cancellationToken)
        {
            var schools = await _schoolRepository.GetAllSchoolsAsync();
            return _mapper.Map<IEnumerable<SchoolDto>>(schools);
        }
    }
}
