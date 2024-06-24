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
        public Account GetAccount(int id);
    }
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IServiceProvider serviceProvider) {
            _accountRepository = serviceProvider.GetRequiredService<IAccountRepository>();
        }

        public void AddAccount(Account account) => _accountRepository.AddAccount(account);

        public List<Account> GetAccounts() => _accountRepository.GetAccounts();

        void IAccountService.AddAccount(Account account)
        {
            throw new NotImplementedException();
        }

        Account IAccountService.GetAccount(int id) => _accountRepository.GetAccount(id);

        List<Account> IAccountService.GetAccounts() => _accountRepository.GetAccounts();
    }
}
