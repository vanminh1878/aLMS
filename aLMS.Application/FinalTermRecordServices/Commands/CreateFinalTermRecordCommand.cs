// CreateFinalTermRecordCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentFinalTermRecordEntity;
using AutoMapper;
using MediatR;

namespace aLMS.Application.FinalTermRecordServices.Commands
{
    public class CreateFinalTermRecordCommand : IRequest<CreateFinalTermRecordResult>
    {
        public CreateFinalTermRecordDto Dto { get; set; } = null!;
    }

    public class CreateFinalTermRecordResult
    {
        public bool Success { get; set; }
        public Guid? RecordId { get; set; }
        public string? Message { get; set; }
    }

    public class CreateFinalTermRecordCommandHandler : IRequestHandler<CreateFinalTermRecordCommand, CreateFinalTermRecordResult>
    {
        private readonly IFinalTermRecordRepository _repository;
        private readonly IMapper _mapper;

        public CreateFinalTermRecordCommandHandler(IFinalTermRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CreateFinalTermRecordResult> Handle(CreateFinalTermRecordCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Kiểm tra trùng (tùy chọn - có thể bỏ nếu cho phép nhiều record cho cùng học sinh)
            if (await _repository.ExistsForStudentAsync(dto.StudentProfileId, dto.ClassId))
            {
                return new CreateFinalTermRecordResult
                {
                    Success = false,
                    Message = "Đã tồn tại bản ghi cuối kỳ cho học sinh này trong lớp/năm học."
                };
            }

            var entity = _mapper.Map<StudentFinalTermRecord>(dto);
            entity.RaiseRecordCreatedEvent(); // nếu có domain event

            await _repository.AddAsync(entity);

            return new CreateFinalTermRecordResult
            {
                Success = true,
                RecordId = entity.Id
            };
        }
    }
}