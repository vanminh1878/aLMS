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
    public class GetSchoolByIdQuery : IRequest<SchoolDto>
    {
        public Guid Id { get; set; }
    }

    public class GetSchoolByIdQueryHandler : IRequestHandler<GetSchoolByIdQuery, SchoolDto>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public GetSchoolByIdQueryHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public async Task<SchoolDto> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
        {
            var school = await _schoolRepository.GetSchoolByIdAsync(request.Id);
            return school == null ? null : _mapper.Map<SchoolDto>(school);
        }
    }
}
