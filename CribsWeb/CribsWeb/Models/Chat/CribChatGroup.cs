using System;
using System.Collections.Generic;

namespace Cribs.Web.Models.Chat
{
    public class CribChatGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Guid> Messages { get; set; } 
    }
}
