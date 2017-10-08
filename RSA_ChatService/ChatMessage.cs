using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RSA_ChatService
{
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        public string MessageText { get; set; }

        [DataMember]
        public string MessageId { get;  set; }
        
        [DataMember]
        public ChatClient Sender { get; set; }

        [DataMember]
        public ChatClient Recipient { get; set; }

        [DataMember]
        public DateTime DateTimeRecieved { get; private set; }        

        public bool Sent;

        public ChatMessage()
        {
            this.DateTimeRecieved = DateTime.Now;
        }

        public override string ToString()
        {
            string message = $"{Sender.NickName}: {MessageText}";
            return message;
        }

    }
}
