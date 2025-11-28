using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.DepartmentEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.DepartmentServices.Commands.CreateDepartment
{
    public class CreateDepartmentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? DepartmentId { get; set; }
    }

    public class CreateDepartmentCommand : IRequest<CreateDepartmentResult>
    {
        public CreateDepartmentDto Dto { get; set; } = null!;
    }

    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, CreateDepartmentResult>
    {
        private readonly IDepartmentRepository _repo;
        private readonly ISchoolRepository _schoolRepo;
        private readonly IMapper _mapper;

        public CreateDepartmentCommandHandler(
            IDepartmentRepository repo,
            ISchoolRepository schoolRepo,
            IMapper mapper)
        {
            _repo = repo;
            _schoolRepo = schoolRepo;
            _mapper = mapper;
        }

        public async Task<CreateDepartmentResult> Handle(CreateDepartmentCommand request, CancellationToken ct)
        {
            try
            {
                var schoolExists = await _schoolRepo.SchoolExistsAsync(request.Dto.SchoolId);
                if (!schoolExists)
                    return new CreateDepartmentResult { Success = false, Message = "School not found." };

                var nameExists = await _repo.NameExistsInSchoolAsync(request.Dto.DepartmentName, request.Dto.SchoolId);
                if (nameExists)
                    return new CreateDepartmentResult { Success = false, Message = "Department name already exists in this school." };

                var department = _mapper.Map<Department>(request.Dto);
                department.Id = Guid.NewGuid();
                department.RaiseDepartmentCreatedEvent();
                await _repo.AddAsync(department);

                return new CreateDepartmentResult
                {
                    Success = true,
                    Message = "Department created successfully.",
                    DepartmentId = department.Id
                };
            }
            catch (Exception ex)
            {
                return new CreateDepartmentResult
                {
                    Success = false,
                    Message = $"Error creating department: {ex.Message}"
                };
            }
        }
    }
}