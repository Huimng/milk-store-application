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
        public void UpdateAccount(Account account);
        public void DeleteAccount(int id);
        public List<Account> GetAlllAccountAdmin();
    }
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IServiceProvider serviceProvider) {
            _accountRepository = serviceProvider.GetRequiredService<IAccountRepository>();
        }

        public void AddAccount(Account account) => _accountRepository.AddAccount(account);

        public List<Account> GetAccounts() => _accountRepository.GetAccounts();


        Account IAccountService.GetAccount(int id) => _accountRepository.GetAccount(id);

        List<Account> IAccountService.GetAccounts() => _accountRepository.GetAccounts();
        public void UpdateAccount(Account account) => _accountRepository.UpdateAccount(account);
        public void DeleteAccount(int id) => _accountRepository.DeleteAccount(id);
        public List<Account> GetAlllAccountAdmin() => _accountRepository.GetAlllAccountAdmin();
    }
}
