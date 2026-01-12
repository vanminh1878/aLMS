using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassEntity;
using AutoMapper;
using MediatR;

public class GetExercisesByTopicIdQuery : IRequest<IEnumerable<ExerciseDto>>
{
    public Guid TopicId { get; set; }
}

public class GetExercisesByLessonIdQueryHandler : IRequestHandler<GetExercisesByTopicIdQuery, IEnumerable<ExerciseDto>>
{
    private readonly IExerciseRepository _repo;
    private readonly ISubjectRepository _subjectrepo;
    private readonly IMapper _mapper;

    public GetExercisesByLessonIdQueryHandler(IExerciseRepository repo, IMapper mapper, ISubjectRepository subjectrepo)
    {
        _repo = repo;
        _mapper = mapper;
        _subjectrepo = subjectrepo;
    }

    public async Task<IEnumerable<ExerciseDto>> Handle(GetExercisesByTopicIdQuery request, CancellationToken ct)
    {
        // 1. Lấy danh sách bài tập theo TopicId
        var exercises = await _repo.GetExercisesByTopicIdAsync(request.TopicId);

        if (!exercises.Any())
        {
            return Enumerable.Empty<ExerciseDto>();
        }

        // 2. Lấy Subject từ TopicId → lấy ClassId
        var subject = await _subjectrepo.GetSubjectByTopicIdAsync(request.TopicId);

        //Guid classId = subject.ClassId;

        // 3. Map sang DTO
        var exerciseDtos = _mapper.Map<IEnumerable<ExerciseDto>>(exercises);

        // 4. Gán ClassId và ClassName cho từng exercise (vì tất cả cùng 1 lớp)
        //foreach (var dto in exerciseDtos)
        //{
        //    dto.ClassId = classId;
        //}

        return exerciseDtos;
    }
}