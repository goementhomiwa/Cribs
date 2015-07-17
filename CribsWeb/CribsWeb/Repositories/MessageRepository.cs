using System;
using System.Collections.Generic;
using System.Linq;
using Cribs.Web.Models.Chat;
using ServiceStack.Redis;

namespace Cribs.Web.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IRedisClient _redisClient;

        public MessageRepository(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public IList<Message> All()
        {
            return _redisClient.As<Message>().GetAll();
        }

        public Message Find(Guid id)
        {
            return _redisClient.As<Message>().GetById(id);
        }

        public Message Save(CribChatGroup group, Message message)
        {
            var result = Save(group, new List<Message> {message});
            return result.FirstOrDefault();
        }

        public IList<Message> Save(CribChatGroup group, IList<Message> messages)
        {
            foreach (var message in messages)
            {
                if (message.Id == default(Guid))
                {
                    message.Id = Guid.NewGuid();
                }

                message.GroupId = group.Id;
                if (!group.Messages.Contains(message.Id))
                {
                    group.Messages.Add(message.Id);
                }
            }

            var messagesIds = messages.Select(g => g.Id.ToString()).ToList();
            using (var transaction = _redisClient.CreateTransaction())
            {
                transaction.QueueCommand(g => g.Store(group));
                transaction.QueueCommand(m => m.StoreAll(messages));
                transaction.QueueCommand(
                    k => k.AddRangeToSet(RedisKeys.GetChatGroupsOrdersReferenceKey(group.Id), messagesIds));
                transaction.Commit();
            }

            return messages;
        }
    }
}