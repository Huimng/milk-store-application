using BusinessObjects.Models;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class MessageGroupRepository
    {
        public BSADBContext dbContext = new BSADBContext();

        public void CreateMessageGroup(MessageGroup messageGroup)
        {
            dbContext.MessageGroups.Add(messageGroup);
            dbContext.SaveChanges();
        }

        public void DeleteMessageGroup(int id)
        {
            var x = GetMessageGroup(id);
            dbContext.MessageGroups.Remove(x);
            dbContext.SaveChanges();
        }

        public List<MessageGroup> GetAllMessageGroup()
        {
            return dbContext.MessageGroups.Include(m => m.Customer)
                .Include(m => m.Manager).ToList();
        }

        public MessageGroup GetMessageGroup(int id)
        {
            var x = dbContext.MessageGroups.Include(m => m.Manager).Include(m =>m.Customer).FirstOrDefault(x => x.MessageGroupId == id);
            return x;
        }

        public void UpdateMessageGroup(MessageGroup messageGroup)
        {
            var trackedTag = dbContext.ChangeTracker.Entries<MessageGroup>()
                                  .FirstOrDefault(e => e.Entity.MessageGroupId == messageGroup.MessageGroupId);
            if (trackedTag != null)
            {
                dbContext.Entry(trackedTag.Entity).State = EntityState.Detached;
            }

            dbContext.MessageGroups.Update(messageGroup);
            dbContext.SaveChanges();

        }
        public MessageGroup? GetMessageGroupByCustomerAndManager(int customerId, int managerId)
        {
            var x =  dbContext.MessageGroups
                .Include(mg => mg.Customer)
                .Include(mg => mg.Manager)
                .Include(mg => mg.Messages)
                .FirstOrDefault(mg => mg.CustomerId == customerId && mg.ManagerId == managerId);
            return x;
        }

        public List<MessageGroup> GetMessageGroupByAccountId(int accountId)
        {
            var messageGroups = dbContext.MessageGroups.Include(m => m.Customer)
                .Include(m => m.Manager).Where(m => m.CustomerId == accountId || m.ManagerId == accountId).ToList();

            return messageGroups;
        }

        
    }
}
