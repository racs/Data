using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class Action
    {
        public string Controller { get; set; }
        public string Method { get; set; }
        public object[] ObjParam { get; set; }

        public Action() { }
    }
}
