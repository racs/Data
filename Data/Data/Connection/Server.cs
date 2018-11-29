using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Connection
{
    class Server
    {
        public Boolean Ativo = false;
        public Socket Socket = null;
        public Byte ServerId = 0;

        public static string _sProtocolReceiver = string.Empty;
        public static string _sProtocolResponse = string.Empty;

        public static ManualResetEvent allDone = new ManualResetEvent(false);


        public Server(byte serverId, string ip, int port)
        {
            try
            {
                //IPAddress ipAdr = null;
                if (IPAddress.TryParse(ip, out IPAddress ipAdr))
                {
                    IPEndPoint ipEnd = new IPEndPoint(ipAdr, port);

                    ServerId = serverId;

                    Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Socket.Bind(ipEnd);
                    Socket.Listen(0);
                    Console.WriteLine("DataServer Iniciado [{0}:{1}]", ip, port);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Aguardando conexões");
                    Console.ResetColor();

                    Ativo = true;

                    Socket.BeginAccept(WaitConnection, null);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
            }
        }

        public void WaitConnection(IAsyncResult ar)
        {
            try
            {
                if (Ativo)
                {
                    Socket newClient = Socket.EndAccept(ar);
                }
                
            }
            catch (Exception ex)
            {

                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
            }
            finally
            {
                if (Ativo)
                {
                    Socket.BeginAccept(WaitConnection, null);
                }
            }
        }
    }
}
