using System;
using System.Collections.Generic;
using Cribs.Web.Models.Chat;

namespace Cribs.Web.Repositories
{
    public interface IChatGroupRepository
    {
        IList<CribChatGroup> All();
        CribChatGroup Find(Guid id);
        CribChatGroup Save(CribChatGroup group);
    }
}
