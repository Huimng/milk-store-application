using BusinessObjects;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Services
{
    public interface IMessageService
    {
        public void Chat(Message message);
        public void DeleteMessage(int id);
        public List<Message> MessageInAGroupMessage(int id);
        public Message GetMessage(int id);
        public List<Message> GetMessages();
    }
    public class MessageService : IMessageService
    {
        public MessageRepository messageRepository;
        public MessageService() 
        {
            messageRepository = new MessageRepository();
        }

        public void Chat(Message message) => messageRepository.CreateMessage(message);
        public void DeleteMessage(int id) => messageRepository.DeleteMessage(id);
        public List<Message> MessageInAGroupMessage(int id) => messageRepository.GetMessageByMessageGroup(id);
        public Message? GetMessage(int id) => messageRepository.GetMessage(id);
        public List<Message> GetMessages() => messageRepository.GetMessages();
    }
}
