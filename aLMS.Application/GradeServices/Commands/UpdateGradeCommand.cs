using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.GradeEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.GradeServices.Commands.UpdateGrade
{
    public class UpdateGradeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? GradeId { get; set; }
    }

    public class UpdateGradeCommand : IRequest<UpdateGradeResult>
    {
        public UpdateGradeDto GradeDto { get; set; }
    }

    public class UpdateGradeCommandHandler : IRequestHandler<UpdateGradeCommand, UpdateGradeResult>
    {
        private readonly IGradeRepository _repo;
        private readonly IMapper _mapper;

        public UpdateGradeCommandHandler(IGradeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdateGradeResult> Handle(UpdateGradeCommand request, CancellationToken ct)
        {
            var grade = _mapper.Map<Grade>(request.GradeDto);
            try
            {
                grade.RaiseGradeUpdatedEvent();
                await _repo.UpdateGradeAsync(grade);
                return new UpdateGradeResult { Success = true, Message = "Grade updated.", GradeId = grade.Id };
            }
            catch (Exception ex)
            {
                return new UpdateGradeResult { Success = false, Message = ex.Message };
            }
        }
    }
}