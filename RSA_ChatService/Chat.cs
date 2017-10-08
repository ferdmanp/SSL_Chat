using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RSA_ChatService.interfaces;

namespace RSA_ChatService
{

    public delegate void LogMethod(string message);
    
    public class Chat : IChat
    {

        static List<ChatClient> _connectedClients = new List<ChatClient>();
        static int maxId = 0;
        public LogMethod Log;

        public Chat()
        {
            Log = FileLogger.Log;
        }

        public List<ChatClient> GetConnectionsList()
        {
            return Chat._connectedClients;
        }

        public string SendMessageAnonymous(string message, int recipientId)
        {
            Log($"SendMessage({message},{recipientId})");
            string messageGuid = String.Empty;
            var connection = _connectedClients.FirstOrDefault(s => s.Id == recipientId);
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
            var connection = _connectedClients.FirstOrDefault(s => s.Id == clientId);
            if (connection == null) return new List<ChatMessage>();
            return connection.GetMessages();
        }

        public ChatClient Register(string nickName)
        {
            
            ChatClient connection = new ChatClient(nickName);
            connection.Id = ++maxId;
            connection.IsOnline = true;
            _connectedClients.Add(connection);
            Log($"Client registered: {nickName}. Id={connection.Id}");
            return connection;
        }

        public void Unregister(ChatClient client)
        {
            //throw new NotImplementedException();
            _connectedClients.Remove(client);
            Log($"Client {client}  unregistered!");
        }

        public ChatMessage SendMessage(string message, ChatClient sender, ChatClient recipient)
        {
            string newId = Guid.NewGuid().ToString();
            ChatMessage msg = new ChatMessage();
            msg.MessageId = newId;
            msg.MessageText = message;
            msg.Sender = sender;
            msg.Recipient = recipient;

            var _recipientConnection = _connectedClients
                                            .FirstOrDefault(p => p.Id == recipient.Id);
            _recipientConnection.AddMessage(msg);
            //Log($"SendMessage([{DateTime.Now.ToString()}]{sender.Id}>>{recipient.Id}: {message}");
            Log($"SendMessage([{DateTime.Now.ToString()}]{sender.Id}>>{recipient.Id}: {message.Length.ToString()}");
            return msg;            
        }

        public void SetSecurityParams(ChatClient client, RSAParameters publicKey)
        {
            var _client = _connectedClients.FirstOrDefault(p => p.Id == client.Id);
            _client.PublicKey = publicKey;
        }
    }
}
