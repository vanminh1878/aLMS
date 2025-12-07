using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace aLMS.Application.AccountServices.Queries
{
    public class GetAccountByIdQuery : IRequest<AccountDto>
    {
        public Guid Id { get; set; }
    }

    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;

        public GetAccountByIdQueryHandler(IAccountRepository accountRepo, IMapper mapper)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
        }

        public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken ct)
        {
            var account = await _accountRepo.GetByIdAsync(request.Id);
            return account == null ? null : _mapper.Map<AccountDto>(account);
        }
    }
}