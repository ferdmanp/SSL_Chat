using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_ChatService
{
    public class FileLogger
    {
        public static void Log(string message)
        {
            using (var stream= new System.IO.StreamWriter(@"ChatServiceLog.log",true))
            {
                stream.Write($"{DateTime.Now}:{message}{Environment.NewLine}");
            }
        }
    }
}
