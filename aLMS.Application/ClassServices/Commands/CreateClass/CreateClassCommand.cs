using aLMS.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.CreateClass
{
    public class CreateClassCommand : IRequest<CreateClassResult>
    {
        public CreateClassDto ClassDto { get; set; }
    }

    public class CreateClassResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? ClassId { get; set; }
    }
}
