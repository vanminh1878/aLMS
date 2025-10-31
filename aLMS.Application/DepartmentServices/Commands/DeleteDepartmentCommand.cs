// aLMS.Application.DepartmentServices.Commands.DeleteDepartment/DeleteDepartmentCommand.cs
using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.DepartmentServices.Commands.DeleteDepartment
{
    public class DeleteDepartmentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? DepartmentId { get; set; }
    }

    public class DeleteDepartmentCommand : IRequest<DeleteDepartmentResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, DeleteDepartmentResult>
    {
        private readonly IDepartmentRepository _repo;

        public DeleteDepartmentCommandHandler(IDepartmentRepository repo) => _repo = repo;

        public async Task<DeleteDepartmentResult> Handle(DeleteDepartmentCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.ExistsAsync(request.Id);
                if (!exists)
                    return new DeleteDepartmentResult { Success = false, Message = "Department not found.", DepartmentId = request.Id };

                await _repo.DeleteAsync(request.Id);
                return new DeleteDepartmentResult
                {
                    Success = true,
                    Message = "Department deleted successfully.",
                    DepartmentId = request.Id
                };
            }
            catch (Exception ex)
            {
                return new DeleteDepartmentResult
                {
                    Success = false,
                    Message = $"Error deleting department: {ex.Message}",
                    DepartmentId = request.Id
                };
            }
        }
    }
}