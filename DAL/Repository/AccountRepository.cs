﻿using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IAccountRepository
    {
        public void AddAccount(Account account);
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
    }
}
