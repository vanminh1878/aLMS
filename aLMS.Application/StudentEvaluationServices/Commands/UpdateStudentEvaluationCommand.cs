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
    public class UpdateStudentEvaluationCommand : IRequest<bool>
    {
        public UpdateStudentEvaluationDto Dto { get; set; }
    }

    public class UpdateStudentEvaluationCommandHandler : IRequestHandler<UpdateStudentEvaluationCommand, bool>
    {
        private readonly IStudentEvaluationRepository _repo;
        private readonly IMapper _mapper;

        public UpdateStudentEvaluationCommandHandler(IStudentEvaluationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateStudentEvaluationCommand request, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(request.Dto.Id);
            if (existing == null)
                return false;

            _mapper.Map(request.Dto, existing);
            existing.RaiseUpdatedEvent();

            await _repo.UpdateAsync(existing);
            return true;
        }
    }
}