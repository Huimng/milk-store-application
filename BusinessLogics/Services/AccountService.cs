using BusinessObjects;
using DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Services
{
    public interface IAccountService
    {
        public void AddAccount(Account account);
        public List<Account> GetAccounts();
    }
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IServiceProvider serviceProvider) {
            _accountRepository = serviceProvider.GetRequiredService<IAccountRepository>();
        }

        public void AddAccount(Account account) => _accountRepository.AddAccount(account);

        public List<Account> GetAccounts() => _accountRepository.GetAccounts();

    }
}
