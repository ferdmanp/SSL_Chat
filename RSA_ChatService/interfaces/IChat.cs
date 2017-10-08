using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RSA_ChatService.interfaces
{
    [ServiceContract]
    public interface IChat
    {
        [OperationContract]
        ChatClient Register(string clientName);

        [OperationContract]
        void Unregister(ChatClient client);

        [OperationContract]
        List<ChatClient> GetConnectionsList();

        [OperationContract]
        string SendMessageAnonymous(string message, int recipientId);

        [OperationContract]
        ChatMessage SendMessage(string message, ChatClient sender, ChatClient recipient);

        [OperationContract]
        List<ChatMessage> RecieveMessagesById(int clientId);
    }
}
