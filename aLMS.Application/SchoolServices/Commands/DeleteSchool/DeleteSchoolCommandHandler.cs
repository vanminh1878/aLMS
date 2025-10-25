using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Commands.DeleteSchool
{
    public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand, DeleteSchoolResult>
    {
        private readonly ISchoolRepository _schoolRepository;

        public DeleteSchoolCommandHandler(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public async Task<DeleteSchoolResult> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            var school = await _schoolRepository.GetSchoolByIdAsync(request.Id);

            if (school == null)
            {
                return new DeleteSchoolResult
                {
                    Success = false,
                    Message = "School not found."
                };
            }

            try
            {
                school.RaiseSchoolDeletedEvent();
                await _schoolRepository.DeleteSchoolAsync(request.Id);

                return new DeleteSchoolResult
                {
                    Success = true,
                    Message = "School deleted successfully.",
                    DeletedSchoolId = request.Id
                };
            }
            catch (Exception ex)
            {
                return new DeleteSchoolResult
                {
                    Success = false,
                    Message = $"Failed to delete school: {ex.Message}"
                };
            }
        }
    }
}
