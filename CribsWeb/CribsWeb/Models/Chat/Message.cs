using System;

namespace Cribs.Web.Models.Chat
{
    public class Message
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Body { get; set; }
        public Guid GroupId { get; set; }
    }
}