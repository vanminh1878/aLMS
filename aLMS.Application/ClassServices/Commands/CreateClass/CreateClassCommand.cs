using aLMS.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.CreateClass
{
    public class CreateClassCommand : IRequest<Guid>
    {
        public CreateClassDto ClassDto { get; set; }
    }
}
