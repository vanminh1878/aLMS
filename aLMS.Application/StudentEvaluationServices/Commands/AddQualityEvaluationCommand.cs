using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentEvaluationEntity;
using aLMS.Domain.StudentQualityEvaluationEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentEvaluationServices.Commands
{
    public class AddQualityEvaluationCommand : IRequest<Guid>
    {
        public CreateStudentQualityEvaluationDto Dto { get; set; }
    }

    public class AddQualityEvaluationCommandHandler : IRequestHandler<AddQualityEvaluationCommand, Guid>
    {
        private readonly IStudentQualityEvaluationRepository _repo;
        private readonly IMapper _mapper;

        public AddQualityEvaluationCommandHandler(IStudentQualityEvaluationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddQualityEvaluationCommand request, CancellationToken ct)
        {
            var entity = _mapper.Map<StudentQualityEvaluation>(request.Dto);
            await _repo.AddAsync(entity);
            return entity.Id;
        }
    }
}