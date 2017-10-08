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
    class ChatSvcProxy : ClientBase<IChat>, IChat
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

        public ChatMessage SendMessage(string message, ChatClient sender, ChatClient recipient)
        {
            return base.Channel.SendMessage(message, sender, recipient);
        }

        public string SendMessageAnonymous(string message, int recipientId)
        {
            //throw new NotImplementedException();
            return base.Channel.SendMessageAnonymous(message, recipientId);
        }

        public void Unregister(ChatClient client)
        {
            base.Channel.Unregister(client);
        }
    }
}
