using RpgProtocol.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Data.Connection
{
    public class Client
    {
        public bool ativo = false;
        public Socket socket = null;
        public const int BufferSize = 1024;
        public int clientId = 0;
        public Byte serverId = 0;
        public static string _sProtocolReceiver = string.Empty;
        public static string _sProtocolResponse = string.Empty;
        public EndPoint ipCliente;
        public string erroProt;

        public Byte[] buffer = new Byte[BufferSize];

        public StringBuilder sb = new StringBuilder();

        public Client(Socket socket, Byte serverId, int clientId, EndPoint ipCliente)
        {
            try
            {
                ativo = true;
                this.socket = socket;
                this.clientId = clientId;
                this.serverId = serverId;
                this.ipCliente = ipCliente;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"O Cliente Id{this.clientId} numero de ip {ipCliente.ToString()} se conectou ao Servidor!");

                //buffer = new Byte[4000];
                this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, WaitData, null);

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
            }
        }

        //recebe os dados
        private void WaitData(IAsyncResult ar)
        {
            try
            {
                if (ativo)
                {
                    int size = this.socket.EndReceive(ar);

                    // se o tamanho do buffer for maior que zero, escreve na tela os bytes recebidos
                    if (size > 0)
                    {
                        Array.Resize(ref this.buffer, size);
                        sb.Append(Encoding.UTF8.GetString(buffer, 0, size));
                        _sProtocolReceiver = sb.ToString();
                        sb.Clear();
                        //implementado controle de erros com o valor do _sProtocolReceiver recebido aqui
                        if (ProtocoloRecebidoOK(_sProtocolReceiver))
                        {
                            ProtocolResolver pResolver = new ProtocolResolver(_sProtocolReceiver);
                            ActionController actionController = new ActionController(pResolver, this.clientId);
                            actionController.ExecAction();
                        }
                        else
                        {                             
                            Server._sProtocolResponse = erroProt; 
                        }
                        

                        if (_sProtocolReceiver != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Receiver: {0}", _sProtocolReceiver);
                            Console.ResetColor();                            
                            Send(socket, Server._sProtocolResponse);
                        }
                        else
                        {
                            socket.BeginReceive(buffer, 0, BufferSize,
                                0, new AsyncCallback(WaitData), socket);
                        }

                        Console.WriteLine($"{_sProtocolReceiver}");                                          

                    }
                    else
                    {
                        //atualizar a lista do servidor retirando esse cliente da lista                        
                        //evento disparado sempre que um cliente disconecta
                                                
                        this.ClienteDisconecta(this, new EventArgs());
                        Close();
                        
                        
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
            }
            finally
            {
                if (ativo)
                {
                    try
                    {
                        buffer = new Byte[4000];
                        this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, WaitData, null);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }                   

                }

            }
        }

        private static void Send(Socket handler, string data)
        {            
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length,
                0, new AsyncCallback(SendCallBack), handler);
        }

        private static void SendCallBack(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Server Send: {0}", Server._sProtocolResponse);
            Console.ResetColor();    

        }

        public void Close()
        {
            try
            {
                if (ativo)
                {
                    ativo = false;

                    socket.Close();
                    socket = null;

                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
            }
        }

        

        public void Cliente_ClienteDisconecta()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"O cliente numero  ID{this.clientId} foi desconectado do servidor.");
        }

        public bool ProtocoloRecebidoOK(string protocolo)
        {
            if (protocolo == "")
            {
                erroProt = "Empty protocol!";
                return false;
            }

            if (protocolo.Substring(0, 1) != "[")
            {
                erroProt = "Protocol not é ã ô started with '['";
                return false;
            }

            if (!protocolo.Contains("|"))
            {
                erroProt = "not found '|' in the protocol";
                return false;
            }

            if (protocolo.Contains(" "))
            {
                erroProt = "space character is not allowd in the protocol !";
                return false;
            }

            return true;
        }


        //public delegate void EventoClienteClienteDisconecta(object sender, EventArgs e);
        public delegate void EventoClienteClienteDisconecta(object sender, EventArgs e);
        public event EventoClienteClienteDisconecta ClienteDisconecta;

    }
}
