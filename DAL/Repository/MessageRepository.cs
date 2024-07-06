using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class MessageRepository
    {
        public BSADBContext dbContext = new BSADBContext();

        public List<Message> GetMessageByMessageGroup(int id)
        {
            return dbContext.Messages.Where(m => m.GroupId == id).ToList();
        }

        public List<Message> GetMessages() { 
            return dbContext.Messages.ToList();
        }

        public Message? GetMessage(int id)
        {
            return dbContext.Messages.FirstOrDefault(m => m.MessageId == id);
        }

        public void CreateMessage(Message message)
        {
            dbContext.Messages.Add(message);
            dbContext.SaveChanges();
        }

        public void DeleteMessage(int id)
        {
            var message = dbContext.Messages.FirstOrDefault(m =>m.MessageId == id);
            dbContext.Messages.Remove(message);
            dbContext.SaveChanges();
        }


    }
}
