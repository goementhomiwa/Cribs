using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Cribs.Web
{
    [HubName("CribChatHub")]
    public class CribChatHub : Hub
    {
        public void Subscribe(string chatId)
        {
            Groups.Add(Context.ConnectionId, chatId);
        }

        public void Publish(/*string toChatId,*/ string message)
        {
            //Clients.Group(toChatId).flush(message);
            Clients.All.flush(message);

        }
    }
}
