using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.GradeEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.GradeServices.Commands.CreateGrade
{
    public class CreateGradeCommand : IRequest<Guid>
    {
        public CreateGradeDto GradeDto { get; set; }
    }

    public class CreateGradeCommandHandler : IRequestHandler<CreateGradeCommand, Guid>
    {
        private readonly IGradeRepository _repo;
        private readonly IMapper _mapper;

        public CreateGradeCommandHandler(IGradeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateGradeCommand request, CancellationToken ct)
        {
            var grade = _mapper.Map<Grade>(request.GradeDto);
            grade.RaiseGradeCreatedEvent();
            await _repo.AddGradeAsync(grade);
            return grade.Id;
        }
    }
}