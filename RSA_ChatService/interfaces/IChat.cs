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
        List<ChatClient> GetConnectionsList();

        [OperationContract]
        string SendMessage(string message, int recipientId);

        [OperationContract]
        string RecieveMessagesById(int clientId);
    }
}
