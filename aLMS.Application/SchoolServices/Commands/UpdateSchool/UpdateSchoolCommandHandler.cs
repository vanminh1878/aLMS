using aLMS.Application.Common.Interfaces;
using aLMS.Domain.SchoolEntity;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Commands.UpdateSchool
{
    public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand, Unit>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public UpdateSchoolCommandHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var school = _mapper.Map<School>(request.SchoolDto);
            school.RaiseSchoolUpdatedEvent();

            await _schoolRepository.UpdateSchoolAsync(school);
            return Unit.Value;
        }
    }
}
