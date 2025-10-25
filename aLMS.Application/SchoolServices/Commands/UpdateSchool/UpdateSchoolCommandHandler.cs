using aLMS.Application.Common.Interfaces;
using aLMS.Domain.SchoolEntity;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Commands.UpdateSchool
{
    public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand, UpdateSchoolResult>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public UpdateSchoolCommandHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public async Task<UpdateSchoolResult> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var school = _mapper.Map<School>(request.SchoolDto);

            try
            {
                school.RaiseSchoolUpdatedEvent();
                await _schoolRepository.UpdateSchoolAsync(school);

                return new UpdateSchoolResult
                {
                    Success = true,
                    Message = "School updated successfully.",
                    SchoolId = school.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateSchoolResult
                {
                    Success = false,
                    Message = $"Failed to update school: {ex.Message}"
                };
            }
        }
    }
}
