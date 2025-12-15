using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.BehaviourEntity;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.BehaviourServices.Commands
{
    public class CreateBehaviourCommand : IRequest<BehaviourDto>
    {
        public Guid StudentId { get; set; }
        public string? Video { get; set; }         // URL video minh chứng (có thể null)
        public string Result { get; set; } = string.Empty; // Ví dụ: "Vi phạm", "Khen thưởng"
        public int Order { get; set; }             // Số lần (lần thứ mấy)
        public DateTime? Date { get; set; } = DateTime.UtcNow; // Ngày xảy ra hành vi
    }

    public class CreateBehaviourCommandHandler : IRequestHandler<CreateBehaviourCommand, BehaviourDto>
    {
        private readonly IBehaviourRepository _behaviourRepository;
        private readonly IStudentProfileRepository _studentRepository;
        private readonly IMapper _mapper;

        public CreateBehaviourCommandHandler(
            IBehaviourRepository behaviourRepository,
            IStudentProfileRepository studentRepository,
            IMapper mapper)
        {
            _behaviourRepository = behaviourRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<BehaviourDto> Handle(CreateBehaviourCommand request, CancellationToken cancellationToken)
        {
            var nextOrder = await _behaviourRepository.GetNextOrderAsync(request.StudentId);
            var behaviourEntity = new Behaviour
            {
                Id = Guid.NewGuid(),
                StudentId = request.StudentId,
                Video = request.Video,
                Result = request.Result,
                Order = nextOrder,
                Date = request.Date ?? DateTime.UtcNow
            };

            await _behaviourRepository.AddAsync(behaviourEntity);

            return _mapper.Map<BehaviourDto>(behaviourEntity);
        }
    }
}