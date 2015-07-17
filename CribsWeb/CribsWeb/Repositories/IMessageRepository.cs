using System;
using System.Collections.Generic;
using Cribs.Web.Models.Chat;

namespace Cribs.Web.Repositories
{
    public interface IMessageRepository
    {
        IList<Message> All();
        Message Find(Guid id);
        Message Save(CribChatGroup group, Message message);
        IList<Message> Save(CribChatGroup group, IList<Message> messages);
    }
}