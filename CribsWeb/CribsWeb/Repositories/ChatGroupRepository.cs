using System;
using System.Collections.Generic;
using Cribs.Web.Models.Chat;
using ServiceStack.Redis;

namespace Cribs.Web.Repositories
{
    public class ChatGroupRepository : IChatGroupRepository
    {
        private readonly IRedisClient _redisClient;

        public ChatGroupRepository(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public IList<CribChatGroup> All()
        {
            var chatGroups = _redisClient.As<CribChatGroup>();
            return chatGroups.GetAll();
        }

        public CribChatGroup Find(Guid id)
        {
            return _redisClient.As<CribChatGroup>().GetById(id);
        }

        public CribChatGroup Save(CribChatGroup group)
        {
            if (group.Id == default(Guid))
            {
                group.Id = Guid.NewGuid();
            }

            return _redisClient.Store(group);
        }      
    }
}
