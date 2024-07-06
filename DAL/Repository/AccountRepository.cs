using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IAccountRepository
    {
        public void AddAccount(Account account);
        public List<Account> GetAccounts();
        public Account GetAccount(int id);
        public void UpdateAccount(Account account);
        public void DeleteAccount(int id);
        public List<Account> GetAlllAccountAdmin();

        public List<Account> GetAlllStaff();

        public List<Account> GetAllMember();
        public Account GetAccountByUserName(String username);

    }
    public class AccountRepository : IAccountRepository
    {
        ////using singleton pattern
        //private static AccountRepository instance = null;
        //public static readonly object instanceLock = new object();
        //private AccountRepository() { }
        //public static AccountRepository Instance
        //{
        //    get
        //    {
        //        lock (instanceLock)
        //        {
        //            if (instance == null)
        //            {
        //                instance = new AccountRepository();
        //            }
        //            return instance;
        //        }
        //    }
        //}
        ////------------------------------------------

        public void AddAccount(Account account)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    context.Set<Account>().Add(account);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Account GetAccount(int id)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    var account = context.Set<Account>().FirstOrDefault(x => x.AccountId == id);
                    return account;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Account> GetAccounts()
        {
            List<Account> accounts = new List<Account>();
            try
            {
                using (var context = new BSADBContext())
                {
                    accounts = context.Set<Account>().ToList();
                }
                return accounts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Account> GetAlllAccountAdmin()
        {
            List<Account> accounts = new List<Account>();
            try
            {
                using (var context = new BSADBContext())
                {
                    accounts = context.Set<Account>().Where(a => a.Role != AccountRoles.Admin).ToList();
                }
                return accounts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateAccount(Account account)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    Account accountOld = context.Set<Account>().FirstOrDefault(x => x.AccountId == account.AccountId);
                    if (accountOld != null)
                    {
                        accountOld.Name = account.Name;
                        accountOld.Email = account.Email;
                        accountOld.Username = account.Username;
                        accountOld.Password = account.Password;
                        accountOld.Status = account.Status;
                        accountOld.UpdateDate = DateTime.UtcNow;
                        context.SaveChanges();
                    }
                }

                }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            }

        public void DeleteAccount(int id)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    Account account = context.Set<Account>().FirstOrDefault(x => x.AccountId == id);
                    if(account != null)
                    {                       
                        account.Status = false;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            }

        public Account GetAccountByUserName(string username)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    var account = context.Set<Account>().FirstOrDefault(x => x.Username == username);
                    return account;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Account> GetAlllStaff()
        {
            var context = new BSADBContext();
            return context.Accounts.Where(x => x.Role == AccountRoles.Staff).ToList();
        }

        public List<Account> GetAllMember()
        {
            var context = new BSADBContext();
            return context.Accounts.Where(x => x.Role == AccountRoles.Member).ToList();
        }
    }
}
