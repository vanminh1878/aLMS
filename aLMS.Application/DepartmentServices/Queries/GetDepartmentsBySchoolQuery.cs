using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.DepartmentEntity;
using AutoMapper;
using MediatR;

public class GetDepartmentsBySchoolQuery : IRequest<IEnumerable<DepartmentDto>>
{
    public Guid SchoolId { get; set; }
}

public class GetDepartmentByIdQuery : IRequest<DepartmentDto>
{
    public Guid Id { get; set; }
}

public class DepartmentQueryHandler :
    IRequestHandler<GetDepartmentsBySchoolQuery, IEnumerable<DepartmentDto>>,
    IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
{
    private readonly IDepartmentRepository _repo;
    private readonly ITeacherProfileRepository _teacherProfileRepository;
    private readonly IMapper _mapper;

    public DepartmentQueryHandler(IDepartmentRepository repo, IMapper mapper, ITeacherProfileRepository teacherProfileRepository)
    {
        _repo = repo;
        _mapper = mapper;
        _teacherProfileRepository = teacherProfileRepository;
    }

    //public async Task<IEnumerable<DepartmentDto>> Handle(GetDepartmentsBySchoolQuery request, CancellationToken ct)
    //{
    //    //var departments = await _repo.GetAllBySchoolIdAsync(request.SchoolId);
    //    return await _repo.GetAllBySchoolIdAsync(request.SchoolId);
    //}

    public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery request, CancellationToken ct)
    {
        var department = await _repo.GetByIdAsync(request.Id);
        return department == null ? null : _mapper.Map<DepartmentDto>(department);
    }
    public async Task<IEnumerable<DepartmentDto>?> Handle(GetDepartmentsBySchoolQuery request, CancellationToken ct)
    {
        var departments = await _repo.GetAllBySchoolIdAsync(request.SchoolId);

        if (departments == null)
            return null;

        var departmentDtos = new List<DepartmentDto>();
        if (departments != null)
        {
            foreach (var department in departments)
            {
                var teachers = await _teacherProfileRepository.GetByDepartmentIdAsync(department.Id);

                int teacherCount = teachers.Count();
                var departmentDto = _mapper.Map<DepartmentDto>(department);

                departmentDto.NumTeachers = teacherCount;

                departmentDtos.Add(departmentDto);
            }
        }
        return departmentDtos;
    }
}