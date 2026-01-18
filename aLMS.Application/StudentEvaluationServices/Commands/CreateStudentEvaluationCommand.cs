using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentEvaluationEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentEvaluationServices.Commands
{
    public class CreateStudentEvaluationCommand : IRequest<Guid>
    {
        public CreateStudentEvaluationDto Dto { get; set; }
    }

    public class CreateStudentEvaluationCommandHandler : IRequestHandler<CreateStudentEvaluationCommand, Guid>
    {
        private readonly IStudentEvaluationRepository _repo;
        private readonly IMapper _mapper;

        public CreateStudentEvaluationCommandHandler(IStudentEvaluationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateStudentEvaluationCommand request, CancellationToken ct)
        {
            try
            {
                var dto = request.Dto;

                var entity = _mapper.Map<StudentEvaluation>(dto);

                // Đảm bảo CreatedBy được set (nếu AutoMapper không map được)
                entity.CreatedBy = dto.CreatedBy;

                entity.RaiseCreatedEvent();

                await _repo.AddAsync(entity);

                return entity.Id;
            }

            catch (Exception ex)
            {

                throw new Exception("Đã xảy ra lỗi hệ thống khi tạo đánh giá học sinh. Vui lòng thử lại sau.", ex);
            }
        }
    }
}