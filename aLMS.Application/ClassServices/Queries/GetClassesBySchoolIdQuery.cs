using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace aLMS.Application.ClassServices.Queries
{
    public class GetClassesBySchoolIdQuery : IRequest<IEnumerable<ClassDto>>
    {
        public Guid SchoolId { get; set; }
    }

    public class GetClassesBySchoolIdQueryHandler : IRequestHandler<GetClassesBySchoolIdQuery, IEnumerable<ClassDto>>
    {
        private readonly IClassRepository _classRepository;
        private readonly IStudentProfileRepository _studentProfileRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public GetClassesBySchoolIdQueryHandler(IClassRepository classRepository, IMapper mapper, IStudentProfileRepository studentProfileRepository, IUsersRepository usersRepository)
        {
            _classRepository = classRepository;
            _mapper = mapper;
            _studentProfileRepository = studentProfileRepository;
            _usersRepository = usersRepository;
        }

        public async Task<IEnumerable<ClassDto>?> Handle(GetClassesBySchoolIdQuery request, CancellationToken cancellationToken)
        {
            var classes = await _classRepository.GetClassesBySchoolIdAsync(request.SchoolId);
            
            var classDtos = new List<ClassDto>();
            foreach (var c in classes)
            {
                var studentlist = await _studentProfileRepository.GetByClassIdAsync(c.Id);
                int studentCount = studentlist.Count();

                var user = c.HomeroomTeacherId.HasValue
                            ? await _usersRepository.GetByIdAsync(c.HomeroomTeacherId.Value)
                            : null;


                var classDto = _mapper.Map<ClassDto>(c);
                classDto.NumStudent = studentCount;
                classDto.IsDelete = c.IsDeleted;
                if(user != null)
                {
                    classDto.homeroomTeacherName = user.Name;
                }
                classDtos.Add(classDto);
            }
            return classDtos;
        }
    }
}