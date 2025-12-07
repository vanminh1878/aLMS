using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.AddStudentsToClass
{
      public class AddStudentsToClassCommand : IRequest<AddStudentsToClassResult>
    {
        public Guid ClassId { get; set; }
        public List<AddStudentToClassDto> Students { get; set; } = new();
    }

    public class AddStudentsToClassResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int SuccessCount => CreatedStudents.Count;
        public int ErrorCount => Errors.Count;
        public List<StudentCreationResult> CreatedStudents { get; set; } = new();
        public List<string> Errors { get; set; } = new();
    }

    public class StudentCreationResult
    {
        public string StudentName { get; set; } = string.Empty;
        public string StudentUsername { get; set; } = string.Empty;
        public string StudentPassword { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public string ParentUsername { get; set; } = string.Empty;
        public string ParentPassword { get; set; } = string.Empty;
    }

}
