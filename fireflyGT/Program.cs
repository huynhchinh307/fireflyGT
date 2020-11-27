using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fireflyGT
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var a in ADBHelper.GetMemus())
            {
                Console.WriteLine(a.name+" - "+a.status);
            }
            ADBHelper.ScreenShoot("31");
            Console.ReadKey();
        }
    }
}
