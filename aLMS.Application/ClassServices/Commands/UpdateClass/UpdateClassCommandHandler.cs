using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.UpdateClass
{
    public class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, UpdateClassResult>
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public UpdateClassCommandHandler(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<UpdateClassResult> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
            var classEntity = _mapper.Map<Class>(request.ClassDto);

            try
            {
                classEntity.RaiseClassUpdatedEvent();
                await _classRepository.UpdateClassAsync(classEntity);

                return new UpdateClassResult
                {
                    Success = true,
                    Message = "Class updated successfully.",
                    ClassId = classEntity.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateClassResult
                {
                    Success = false,
                    Message = $"Failed to update class: {ex.Message}"
                };
            }
        }
    }
}