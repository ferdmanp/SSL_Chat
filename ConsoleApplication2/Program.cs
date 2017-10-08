using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {

        

        static void Main(string[] args)
        {
            Encoding ByteConverter = new UTF8Encoding();
            string msg = "1";
            byte[] b = ByteConverter.GetBytes(msg);
            
            string c = ByteConverter.GetString(b);
            Console.WriteLine(c);
            Console.ReadKey();
        }
    }
}
