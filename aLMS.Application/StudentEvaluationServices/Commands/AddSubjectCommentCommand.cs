using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentEvaluationEntity;
using aLMS.Domain.StudentSubjectCommentEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.StudentEvaluationServices.Commands
{
    public class AddSubjectCommentCommand : IRequest<Guid>
    {
        public CreateStudentSubjectCommentDto Dto { get; set; }
    }

    public class AddSubjectCommentCommandHandler : IRequestHandler<AddSubjectCommentCommand, Guid>
    {
        private readonly IStudentSubjectCommentRepository _repo;
        private readonly IMapper _mapper;

        public AddSubjectCommentCommandHandler(IStudentSubjectCommentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddSubjectCommentCommand request, CancellationToken ct)
        {
            var entity = _mapper.Map<StudentSubjectComment>(request.Dto);
            await _repo.AddAsync(entity);
            return entity.Id;
        }
    }
}