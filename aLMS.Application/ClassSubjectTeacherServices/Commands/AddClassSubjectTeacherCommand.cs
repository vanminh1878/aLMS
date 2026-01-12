using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassSubjectEntity;
using MediatR;
using System;

namespace aLMS.Application.ClassSubjectTeacherServices.Commands.AddClassSubjectTeacher
{
    public class AddClassSubjectTeacherCommand : IRequest<AddClassSubjectTeacherResult>
    {
        public Guid ClassSubjectId { get; set; }
        public Guid TeacherId { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class AddClassSubjectTeacherResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? Id { get; set; }
    }

    public class AddClassSubjectTeacherCommandHandler : IRequestHandler<AddClassSubjectTeacherCommand, AddClassSubjectTeacherResult>
    {
        private readonly IClassSubjectTeacherRepository _repo;
        private readonly IClassSubjectRepository _classSubjectRepo;
        private readonly ITeacherProfileRepository _teacherRepo;

        public AddClassSubjectTeacherCommandHandler(
            IClassSubjectTeacherRepository repo,
            IClassSubjectRepository classSubjectRepo,
            ITeacherProfileRepository teacherRepo)
        {
            _repo = repo;
            _classSubjectRepo = classSubjectRepo;
            _teacherRepo = teacherRepo;
        }

        public async Task<AddClassSubjectTeacherResult> Handle(AddClassSubjectTeacherCommand request, CancellationToken ct)
        {
            // Validate tồn tại
            var classSubject = await _classSubjectRepo.GetClassSubjectByIdAsync(request.ClassSubjectId);
            var teacherExists = await _teacherRepo.ExistsAsync(request.TeacherId);

            if (classSubject == null || !teacherExists)
            {
                return new AddClassSubjectTeacherResult { Success = false, Message = "Môn-lớp hoặc GV không tồn tại" };
            }

            // Check trùng phân công
            var existing = await _repo.GetByClassSubjectAndTeacherAsync(request.ClassSubjectId, request.TeacherId, request.SchoolYear);
            if (existing != null)
            {
                return new AddClassSubjectTeacherResult { Success = false, Message = "GV này đã được phân công cho môn-lớp này" };
            }

            var assignment = new ClassSubjectTeacher(request.ClassSubjectId, request.TeacherId, request.SchoolYear);
            await _repo.AddAsync(assignment);

            return new AddClassSubjectTeacherResult { Success = true, Id = assignment.Id };
        }
    }
}