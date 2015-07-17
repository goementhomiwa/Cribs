using System;
using Cribs.Web.Models.Chat;
using Cribs.Web.Repositories;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Cribs.Web
{
    [HubName("CribChatHub")]
    public class CribChatHub : Hub
    {
        public IChatGroupRepository ChatGroupRepository { get; set; }
        public IMessageRepository MessageRepository { get; set; }

        public void Publish(Guid id, string username, string message)
        {
            var post = ChatGroupRepository.Find(id);
            if (post != null)
            {
                var savedMessage = MessageRepository.Save(post, new Message {Body = message, UserName = username});
                Clients.All.flush(message);
            }

        }
    }
}
