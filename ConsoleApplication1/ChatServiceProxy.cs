using RSA_ChatService.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RSA_ChatService;

namespace ConsoleApplication1
{
    public class ChatServiceProxy : ClientBase<IChat>, IChat
    {
        public List<ChatClient> GetConnectionsList()
        {
            return base.Channel.GetConnectionsList();
        }

        public string RecieveMessagesById(int clientId)
        {
            return base.Channel.RecieveMessagesById(clientId);
        }

        public ChatClient Register(string clientName)
        {
            return base.Channel.Register(clientName);
        }       
        

        public string SendMessage(string message, int recipientId)
        {
            return base.Channel.SendMessage(message, recipientId);
        }
    }
}
