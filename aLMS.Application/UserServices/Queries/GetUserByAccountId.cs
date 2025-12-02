using aLMS.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.UserServices.Queries
{
    public class GetUserByAccountIdQuery : IRequest<UserDto?>
    {
        public Guid AccountId { get; set; }
    }
}
