using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.BehaviourServices.Queries
{
    public class GetBehavioursByClassQueryHandler
        : IRequestHandler<GetBehavioursByClassQuery, List<StudentBehaviourSummaryDto>>
    {
        private readonly IStudentProfileRepository _studentRepo;
        private readonly IBehaviourRepository _behaviourRepo;
        private readonly IUsersRepository _userRepo;
        private readonly IMapper _mapper;

        public GetBehavioursByClassQueryHandler(
            IStudentProfileRepository studentRepo,
            IBehaviourRepository behaviourRepo,
            IUsersRepository userRepo,
            IMapper mapper)
        {
            _studentRepo = studentRepo;
            _behaviourRepo = behaviourRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<List<StudentBehaviourSummaryDto>> Handle(
            GetBehavioursByClassQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Lấy danh sách UserId của học sinh trong lớp
            var studentUsers = await _studentRepo.GetByClassIdAsync(request.ClassId);

            if (!studentUsers.Any())
                return new List<StudentBehaviourSummaryDto>();

            // 2. Duyệt từng học sinh: lấy tên + lấy hành vi
            var result = new List<StudentBehaviourSummaryDto>();

            foreach (var student in studentUsers)
            {
                // Lấy thông tin tên học sinh
                var user = await _userRepo.GetByIdAsync(student.UserId);
                var fullName = user?.Name ?? "Chưa có tên";

                // Lấy hành vi của học sinh này
                var behaviours = await _behaviourRepo.GetByStudentIdAsync(student.UserId);
                var behaviourDtos = _mapper.Map<List<BehaviourDto>>(
    behaviours.OrderByDescending(b => b.Date)
);

                result.Add(new StudentBehaviourSummaryDto
                {
                    StudentId = student.UserId,
                    FullName = fullName,
                    Behaviours = behaviourDtos
                });
            }
            return result.OrderBy(r => r.FullName).ToList();
        }
    }
}