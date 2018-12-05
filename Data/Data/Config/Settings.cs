using Data.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Config
{
    public static class Settings
    {
        public static string[] ips =
        {
            //"192.168.0.23"
            "10.3.52.25"
            

        };

        public static Server[] servers = null;

        public static int MaxConnection = 1000;
    }
}

