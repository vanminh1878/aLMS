using aLMS.Application.Common.Interfaces;
using aLMS.Application.UserServices.Commands.CreateUser;
using aLMS.Domain.AccountEntity;
using aLMS.Domain.ClassEntity;
using aLMS.Domain.UserEntity;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.CreateClass
{
    public class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, CreateClassResult>
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public CreateClassCommandHandler(IClassRepository repo, IMapper mapper)
        {
            _classRepository = repo;
            _mapper = mapper;
        }

        public async Task<CreateClassResult> Handle(CreateClassCommand request, CancellationToken ct)
        {
            var classNameExists = await _classRepository.ClassNameExistsAsync(request.ClassDto.ClassName);
            if (classNameExists)
                return new CreateClassResult { Success = false, Message = "Tên lớp đã tồn tại" };
            var entity = _mapper.Map<Class>(request.ClassDto);
            entity.RaiseClassCreatedEvent();

            await _classRepository.AddClassAsync(entity);
            return new CreateClassResult
            {
                Success = true,
                Message = "Lớp học đã tạo thành công.",
                ClassId = entity.Id
            };
        }
    }
}
