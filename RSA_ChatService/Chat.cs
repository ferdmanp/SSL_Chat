using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RSA_ChatService.interfaces;

namespace RSA_ChatService
{
    
    public class Chat : IChat
    {

        static List<ChatClient> _connections = new List<ChatClient>();
        static int maxId = 0;      

        public List<ChatClient> GetConnectionsList()
        {
            throw new NotImplementedException();
        }

        public string SendMessage(string message, int recipientId)
        {
            string messageGuid = String.Empty;
            var connection = _connections.FirstOrDefault(s => s.Id == recipientId);
            if (connection != null)
            {
                ChatMessage chatMessage = new ChatMessage();
                chatMessage.MessageText = message;
                chatMessage.MessageId = Guid.NewGuid().ToString();
                messageGuid= chatMessage.MessageId;
            }
            return messageGuid;
            
        }

        public List<ChatMessage> RecieveMessagesById(int clientId)
        {
            var connection = _connections.FirstOrDefault(s => s.Id == clientId);
            return connection.GetMessages();
        }

        public ChatClient Register(string nickName)
        {
            ChatClient connection = new ChatClient(nickName);
            connection.Id = ++maxId;
            connection.IsOnline = true;
            _connections.Add(connection);
            return connection;
        }
    }
}
