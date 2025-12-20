using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.AccountServices.Queries
{
    public class GetRoleNameByAccountIdQuery : IRequest<string?>
    {
        public Guid AccountId { get; set; }
    }

    public class GetRoleNameByAccountIdQueryHandler : IRequestHandler<GetRoleNameByAccountIdQuery, string?>
    {
        private readonly IAccountRepository _accountRepository;

        public GetRoleNameByAccountIdQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<string?> Handle(GetRoleNameByAccountIdQuery request, CancellationToken cancellationToken)
        {
            return await _accountRepository.GetRoleNameByAccountIdAsync(request.AccountId);
        }
    }
}
