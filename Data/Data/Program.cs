using Data.Config;
using Data.Connection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Data";

            Settings.servers = new Server[Settings.ips.Length];
            

            for (Byte i = 0; i < Settings.servers.Length; i++)
            {
                Settings.servers[i] = new Server(i, Settings.ips[i], 5060);
            }

            // evento disparado toda vez que um cliente se conecta
            Settings.servers[0].NovaConexao += new Server.EventoServidorClienteConecta(Settings.servers[0].Server_ListaClientes);
            

            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
