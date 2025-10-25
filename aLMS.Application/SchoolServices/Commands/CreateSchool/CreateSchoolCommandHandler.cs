using aLMS.Application.Common.Interfaces;
using aLMS.Domain.SchoolEntity;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Commands.CreateSchool
{
    public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, Guid>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public CreateSchoolCommandHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var school = _mapper.Map<School>(request.SchoolDto);
            school.Id = Guid.NewGuid();
            school.RaiseSchoolCreatedEvent();

            await _schoolRepository.AddSchoolAsync(school);
            return school.Id;
        }
    }

}
