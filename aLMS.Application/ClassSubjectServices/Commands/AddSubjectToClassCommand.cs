using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassSubjectEntity;
using MediatR;
using System;

namespace aLMS.Application.ClassSubjectServices.Commands.AddSubjectToClass
{
    public class AddSubjectToClassCommand : IRequest<AddSubjectToClassResult>
    {
        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class AddSubjectToClassCommandHandler : IRequestHandler<AddSubjectToClassCommand, AddSubjectToClassResult>
    {
        private readonly IClassSubjectRepository _classSubjectRepo;
        private readonly ISubjectRepository _subjectRepo; // Để check subject tồn tại
        private readonly IClassRepository _classRepo;     // Để check class tồn tại (nếu cần)

        public AddSubjectToClassCommandHandler(
            IClassSubjectRepository classSubjectRepo,
            ISubjectRepository subjectRepo,
            IClassRepository classRepo)
        {
            _classSubjectRepo = classSubjectRepo;
            _subjectRepo = subjectRepo;
            _classRepo = classRepo;
        }

        public async Task<AddSubjectToClassResult> Handle(AddSubjectToClassCommand request, CancellationToken ct)
        {
            // Validate: Check class và subject tồn tại
            var classExists = await _classRepo.ClassExistsAsync(request.ClassId);
            var subjectExists = await _subjectRepo.SubjectExistsAsync(request.SubjectId);

            if (!classExists || !subjectExists)
            {
                return new AddSubjectToClassResult
                {
                    Success = false,
                    Message = "Lớp hoặc môn học không tồn tại"
                };
            }

            // Check trùng (optional, tránh gán 2 lần)
            var existing = await _classSubjectRepo.GetClassSubjectByClassAndSubjectAsync(
                request.ClassId, request.SubjectId, request.SchoolYear);

            if (existing != null)
            {
                return new AddSubjectToClassResult
                {
                    Success = false,
                    Message = "Môn học đã được gán vào lớp này"
                };
            }

            // Tạo mới
            var classSubject = new ClassSubject(
                request.ClassId,
                request.SubjectId,
                request.SchoolYear);

            await _classSubjectRepo.AddClassSubjectAsync(classSubject);

            return new AddSubjectToClassResult
            {
                Success = true,
                ClassSubjectId = classSubject.Id
            };
        }
    }
}