using aLMS.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.DeleteClass
{
    public class DeleteClassCommand : IRequest<DeleteClassResult>
    {
        public Guid Id { get; set; }
    }
}
