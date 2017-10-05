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
        public string MessageId { get; internal set; }
        
        public bool Sent;
    }
}
