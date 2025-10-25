using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.DeleteClass
{
    public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, DeleteClassResult>
    {
        private readonly IClassRepository _classRepository;

        public DeleteClassCommandHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<DeleteClassResult> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var classExists = await _classRepository.ClassExistsAsync(request.Id);
                if (!classExists)
                {
                    return new DeleteClassResult
                    {
                        Success = false,
                        Message = "Class not found."
                    };
                }

                var classEntity = await _classRepository.GetClassByIdAsync(request.Id);
                classEntity.RaiseClassDeletedEvent();
                await _classRepository.DeleteClassAsync(request.Id);

                return new DeleteClassResult
                {
                    Success = true,
                    Message = "Class deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new DeleteClassResult
                {
                    Success = false,
                    Message = $"Failed to delete class: {ex.Message}"
                };
            }
        }
    }
}
