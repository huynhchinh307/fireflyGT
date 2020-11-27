using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fireflyGT
{
    public class deviceInfo
    {
        public string index { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public override string ToString()
        {
            return name;
        }
    }
}
