// UpdateFinalTermRecordCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using MediatR;

namespace aLMS.Application.FinalTermRecordServices.Commands
{
    public class UpdateFinalTermRecordCommand : IRequest<UpdateFinalTermRecordResult>
    {
        public UpdateFinalTermRecordDto Dto { get; set; } = null!;
    }

    public class UpdateFinalTermRecordResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class UpdateFinalTermRecordCommandHandler : IRequestHandler<UpdateFinalTermRecordCommand, UpdateFinalTermRecordResult>
    {
        private readonly IFinalTermRecordRepository _repository;

        public UpdateFinalTermRecordCommandHandler(IFinalTermRecordRepository repository)
        {
            _repository = repository;
        }

        public async Task<UpdateFinalTermRecordResult> Handle(UpdateFinalTermRecordCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Lấy Entity (không phải DTO)
            var entity = await _repository.GetEntityByIdAsync(dto.Id);
            if (entity == null)
            {
                return new UpdateFinalTermRecordResult
                {
                    Success = false,
                    Message = "Không tìm thấy bản ghi"
                };
            }
            if (dto.FinalScore.HasValue)
                entity.FinalScore = dto.FinalScore.Value;

            if (!string.IsNullOrWhiteSpace(dto.FinalEvaluation))
                entity.FinalEvaluation = dto.FinalEvaluation;

            if (!string.IsNullOrWhiteSpace(dto.Comment))
                entity.Comment = dto.Comment;

            await _repository.UpdateAsync(entity);

            return new UpdateFinalTermRecordResult { Success = true };
        }
    }
}