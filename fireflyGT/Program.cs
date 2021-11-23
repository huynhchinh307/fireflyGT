using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fireflyGT
{
    class Program
    {
        static void Main(string[] args)
        {
            ADBHelper.Swipe("40", 91, 418, 265, 418, 500);
            Console.ReadLine();
        }
    }
}
