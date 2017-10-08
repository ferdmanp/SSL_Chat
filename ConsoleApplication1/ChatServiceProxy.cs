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

        public List<ChatMessage> RecieveMessagesById(int clientId)
        {
            return base.Channel.RecieveMessagesById(clientId);
        }

        public ChatClient Register(string clientName)
        {
            return base.Channel.Register(clientName);
        }

        public ChatMessage SendMessage(string message, ChatClient sender, ChatClient recipient)
        {
            throw new NotImplementedException();
        }

        public string SendMessageAnonymous(string message, int recipientId)
        {
            return base.Channel.SendMessageAnonymous(message, recipientId);
        }

        public void Unregister(ChatClient client)
        {
            throw new NotImplementedException();
        }
    }
}
