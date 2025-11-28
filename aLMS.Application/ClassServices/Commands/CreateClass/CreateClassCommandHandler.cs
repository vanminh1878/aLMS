using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassEntity;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.CreateClass
{
    public class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, Guid>
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public CreateClassCommandHandler(IClassRepository repo, IMapper mapper)
        {
            _classRepository = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateClassCommand request, CancellationToken ct)
        {
            var entity = _mapper.Map<Class>(request.ClassDto);
            entity.RaiseClassCreatedEvent();

            await _classRepository.AddClassAsync(entity);
            return entity.Id;
        }
    }
}
