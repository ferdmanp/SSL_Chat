using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using RSA_ChatService.interfaces;
using RSA_ChatService;

namespace RSA_WinClient
{
    class ChatSvcProxy : ClientBase<Chat>, IChat
    {
        public List<ChatClient> GetConnectionsList()
        {
            //throw new NotImplementedException();
            return base.Channel.GetConnectionsList();
        }

        public List<ChatMessage> RecieveMessagesById(int clientId)
        {
            //throw new NotImplementedException();
            return base.Channel.RecieveMessagesById(clientId);
        }

        public ChatClient Register(string clientName)
        {
            //throw new NotImplementedException();
            return base.Channel.Register(clientName);
        }

        public string SendMessage(string message, int recipientId)
        {
            //throw new NotImplementedException();
            return base.Channel.SendMessage(message, recipientId);
        }
    }
}
