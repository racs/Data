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
        public int qtd = 0;
        
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
                    Console.WriteLine("DataServer ID: {0}", serverId);
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

        //quando recebe a conexão
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
                        EndPoint ipCliente = newClientSocket.RemoteEndPoint;
                        Client cliente = new Client(newClientSocket, this.serverId, newClientId, ipCliente);
                        clientes.Add(cliente);
                        // evento disparado toda vez que um cliente se conecta
                        this.NovaConexao();
                        cliente.ClienteDisconecta += new Client.EventoClienteClienteDisconecta(Server_ClienteDisconecta);
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

                if (clientes.Count == 0)
                {
                    qtd = 1;
                    return 1;                    
                }

                for (int i = 1; i>qtd+1; i++)
                {
                    //if (i > clientes.Count)
                    //{
                    //    qtd = i;
                    //    return i;
                    //}
                    qtd = i;

                    return i;
                    
                }


            }
            catch (Exception)
            {

                throw;
            }

            return 0;

        }

        public void Server_ListaClientes()
        {
            Console.WriteLine("Lista de Clientes conectados ao servidor");
            foreach (Client c in clientes)
            {
                Console.WriteLine($"Cliente numero  ID{c.clientId} numero de endereco ip {c.ipCliente.ToString()} ");
            }
        }

        //public void TestaEvento(object sender, EventArgs e)
        //{
        //    Console.WriteLine("funcionou");
        //}

        public void Server_ClienteDisconecta(object sender, EventArgs e)
        {

            foreach (Client c in clientes.ToList())
            {
                if (c == sender )
                {
                    Console.WriteLine($"O cliente numero  ID{c.clientId} foi desconectado do servidor.");
                    clientes.Remove(c);
                }

            }

            
        }

        public delegate void EventoServidorClienteConecta();
        public event EventoServidorClienteConecta NovaConexao;

        


    }
}
