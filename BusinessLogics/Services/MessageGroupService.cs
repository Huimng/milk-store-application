using BusinessObjects;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Services
{
    public interface IMessageGroupService
    {
        public List<MessageGroup> GetMessageGroups();
        public MessageGroup GetMessageGroup(int id);
        public void AddMessageGroup(MessageGroup messageGroup);
        public void DeleteMessageGroup(int id);
        public void UpdateMessageGroup(MessageGroup messageGroup);
        public MessageGroup GetMessageGroupByCustomerAndManager(int customerId,  int managerId);
        public List<MessageGroup> GetMessageGroupByAccount(int accountId);

    }
    public class MessageGroupService : IMessageGroupService
    {
        private MessageGroupRepository messageGroupRepository;
        public MessageGroupService()
        {
            messageGroupRepository = new MessageGroupRepository();
        }
        public void AddMessageGroup(MessageGroup messageGroup) => messageGroupRepository.CreateMessageGroup(messageGroup);

        public void DeleteMessageGroup(int id) => messageGroupRepository.DeleteMessageGroup(id);

        public MessageGroup GetMessageGroup(int id) => messageGroupRepository.GetMessageGroup(id);

        public List<MessageGroup> GetMessageGroups() => messageGroupRepository.GetAllMessageGroup();

        public void UpdateMessageGroup(MessageGroup messageGroup) => messageGroupRepository.UpdateMessageGroup(messageGroup);

        public MessageGroup? GetMessageGroupByCustomerAndManager(int customerId, int managerId) => messageGroupRepository.GetMessageGroupByCustomerAndManager(customerId, managerId);

        public List<MessageGroup> GetMessageGroupByAccount(int accountId) => messageGroupRepository.GetMessageGroupByAccountId(accountId);
    }
}
