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

namespace aLMS.Application.SchoolServices.Commands.CreateSchool
{
    public class CreateSchoolCommand : IRequest<Guid>
    {
        public CreateSchoolDto SchoolDto { get; set; }
    }

   }
