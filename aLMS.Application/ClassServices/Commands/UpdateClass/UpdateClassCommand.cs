using aLMS.Application.Common.Dtos;
using MediatR;

namespace aLMS.Application.ClassServices.Commands.UpdateClass
{
    public class UpdateClassCommand : IRequest<UpdateClassResult>
    {
        public UpdateClassDto ClassDto { get; set; }
    }
}