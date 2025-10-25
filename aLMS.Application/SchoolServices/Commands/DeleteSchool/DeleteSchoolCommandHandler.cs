using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Commands.DeleteSchool
{
    public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand, Guid>
    {
        private readonly ISchoolRepository _schoolRepository;

        public DeleteSchoolCommandHandler(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public async Task<Guid> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            var school = await _schoolRepository.GetSchoolByIdAsync(request.Id);
            if (school != null)
            {
                school.RaiseSchoolDeletedEvent();
                await _schoolRepository.DeleteSchoolAsync(request.Id);
            }
            return Guid.Empty;
        }
    }
}
