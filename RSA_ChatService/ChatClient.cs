using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RSA_ChatService
{
    [DataContract]
    public class ChatClient
    {
        [DataMember]
        public int Id { get; internal set; }

        [DataMember]
        public string NickName { get; set; }

        [DataMember]
        public bool IsOnline { get; internal set; }

        Queue<ChatMessage> messages;

        public ChatClient(string nickName)
        {
            this.NickName = nickName;
            messages = new Queue<ChatMessage>();
        }

        protected internal void AddMessage(ChatMessage message)
        {
            messages.Enqueue(message);
        }

        public List<ChatMessage> GetMessages()
        {
            List<ChatMessage> res = new List<ChatMessage>();
            for (int i = 0; i < messages.Count; i++)
            {
                res.Add(messages.Dequeue());
            }

            return res;
        }

        public override string ToString()
        {
            return $"{this.NickName} ({this.Id})";
        }
    }
}
