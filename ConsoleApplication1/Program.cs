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
            Console.WriteLine("Sum of two numbers... 5+5 =" + proxy.Register("Test", "Test Me"));
            Console.ReadLine();

        }
    }
}
