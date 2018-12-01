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
    public class Server
    {
        public Boolean Ativo = false;
        public Socket socket = null;
        public Byte serverId = 0;

        //public Client[] clientes = new Client[10];
        public List<Client> clientes = new List<Client>();

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

                    this.serverId = serverId;

                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Bind(ipEnd);
                    socket.Listen(0);
                    Console.WriteLine("DataServer Iniciado [{0}:{1}]", ip, port);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Aguardando conexões");
                    Console.ResetColor();

                    Ativo = true;

                    socket.BeginAccept(WaitConnection, null);

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
                    //socket do cliente
                    Socket newClientSocket = socket.EndAccept(ar);

                    int newClientId = GetFreeClientId();

                    if (newClientId > 0)
                    {
                        clientes.Add(new Client(newClientSocket, this.serverId, newClientId));
                    }
                    else
                    {
                        newClientSocket.Close();
                        newClientSocket = null;
                    }
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
                    socket.BeginAccept(WaitConnection, null);
                }
            }
        }

        private int GetFreeClientId()
        {

            try
            {
                //alterar para trabalhar com lista

                if (clientes.Count == 0)
                {
                    return 1;
                }

                for (int i = 1; i <= clientes.Count+1; i++)
                {
                    if (i > clientes.Count)
                    {
                        return i;
                    }
                    
                }


            }
            catch (Exception)
            {

                throw;
            }

            return 0;

        }

    }
}
