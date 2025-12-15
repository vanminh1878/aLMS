using aLMS.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;

namespace aLMS.Application.BehaviourServices.Queries
{
    public class GetBehavioursByClassQuery : IRequest<List<StudentBehaviourSummaryDto>>
    {
        public Guid ClassId { get; set; }
    }

    public class StudentBehaviourSummaryDto
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public List<BehaviourDto> Behaviours { get; set; } = new();
    }
}