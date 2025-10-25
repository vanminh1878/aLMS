using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Commands.DeleteSchool
{
    public class DeleteSchoolCommand : IRequest<DeleteSchoolResult>
    {
        public Guid Id { get; set; }
    }

   
}
