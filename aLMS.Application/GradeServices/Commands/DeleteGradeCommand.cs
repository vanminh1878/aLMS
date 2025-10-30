using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.GradeServices.Commands.DeleteGrade
{
    public class DeleteGradeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class DeleteGradeCommand : IRequest<DeleteGradeResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteGradeCommandHandler : IRequestHandler<DeleteGradeCommand, DeleteGradeResult>
    {
        private readonly IGradeRepository _repo;

        public DeleteGradeCommandHandler(IGradeRepository repo) => _repo = repo;

        public async Task<DeleteGradeResult> Handle(DeleteGradeCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.GradeExistsAsync(request.Id);
                if (!exists) return new DeleteGradeResult { Success = false, Message = "Grade not found." };

                var grade = await _repo.GetGradeByIdAsync(request.Id);
                grade.RaiseGradeDeletedEvent();
                await _repo.DeleteGradeAsync(request.Id);

                return new DeleteGradeResult { Success = true, Message = "Grade deleted." };
            }
            catch (Exception ex)
            {
                return new DeleteGradeResult { Success = false, Message = ex.Message };
            }
        }
    }
}