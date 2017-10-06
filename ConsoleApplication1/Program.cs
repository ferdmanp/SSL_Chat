using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServiceProxy proxy = new ChatServiceProxy();
            Console.WriteLine("Client is running at " + DateTime.Now.ToString());
            string nickName = "voltage";
            var client=proxy.Register(nickName);
            Console.WriteLine(client.Id);
            Console.WriteLine(proxy.GetConnectionsList().Count);
            Console.ReadLine();

        }
    }
}
