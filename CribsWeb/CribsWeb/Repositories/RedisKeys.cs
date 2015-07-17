using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cribs.Web.Repositories
{
    public static class RedisKeys
    {
        public static string ChatMessages = "urn:ChatMessages";

        public static string GetChatGroupsOrdersReferenceKey(Guid chatGroupId)
        {
            return String.Format("{0}_{1}", ChatMessages, chatGroupId);
        }
    }
}
