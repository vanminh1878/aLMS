using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;

namespace aLMS.Application.StudentEvaluationServices.Queries
{
    public class GetStudentEvaluationByIdQuery : IRequest<StudentEvaluationDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetStudentEvaluationByIdQueryHandler : IRequestHandler<GetStudentEvaluationByIdQuery, StudentEvaluationDto?>
    {
        private readonly IStudentEvaluationRepository _repo;
        private readonly IMapper _mapper;

        public GetStudentEvaluationByIdQueryHandler(IStudentEvaluationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<StudentEvaluationDto?> Handle(GetStudentEvaluationByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            return entity == null ? null : _mapper.Map<StudentEvaluationDto>(entity);
        }
    }
}