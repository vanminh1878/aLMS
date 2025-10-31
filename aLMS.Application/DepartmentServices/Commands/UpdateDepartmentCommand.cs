// aLMS.Application.DepartmentServices.Commands.UpdateDepartment/UpdateDepartmentCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.DepartmentEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.DepartmentServices.Commands.UpdateDepartment
{
    public class UpdateDepartmentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? DepartmentId { get; set; }
    }

    public class UpdateDepartmentCommand : IRequest<UpdateDepartmentResult>
    {
        public UpdateDepartmentDto Dto { get; set; } = null!;
    }

    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, UpdateDepartmentResult>
    {
        private readonly IDepartmentRepository _repo;
        private readonly ISchoolRepository _schoolRepo;
        private readonly IMapper _mapper;

        public UpdateDepartmentCommandHandler(
            IDepartmentRepository repo,
            ISchoolRepository schoolRepo,
            IMapper mapper)
        {
            _repo = repo;
            _schoolRepo = schoolRepo;
            _mapper = mapper;
        }

        public async Task<UpdateDepartmentResult> Handle(UpdateDepartmentCommand request, CancellationToken ct)
        {
            try
            {
                var department = await _repo.GetByIdAsync(request.Dto.Id);
                if (department == null)
                    return new UpdateDepartmentResult { Success = false, Message = "Department not found." };

                var schoolExists = await _schoolRepo.SchoolExistsAsync(request.Dto.SchoolId);
                if (!schoolExists)
                    return new UpdateDepartmentResult { Success = false, Message = "School not found." };

                var nameExists = await _repo.NameExistsInSchoolAsync(request.Dto.DepartmentName, request.Dto.SchoolId, request.Dto.Id);
                if (nameExists)
                    return new UpdateDepartmentResult { Success = false, Message = "Department name already exists in this school." };

                _mapper.Map(request.Dto, department);
                department.RaiseDepartmentUpdatedEvent();
                await _repo.UpdateAsync(department);

                return new UpdateDepartmentResult
                {
                    Success = true,
                    Message = "Department updated successfully.",
                    DepartmentId = department.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateDepartmentResult
                {
                    Success = false,
                    Message = $"Error updating department: {ex.Message}"
                };
            }
        }
    }
}