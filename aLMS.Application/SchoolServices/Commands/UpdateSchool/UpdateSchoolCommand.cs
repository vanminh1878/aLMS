using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.SchoolEntity;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Commands.UpdateSchool
{
    public class UpdateSchoolCommand : IRequest<Unit>
    {
        public UpdateSchoolDto SchoolDto { get; set; }
    }

  
}
