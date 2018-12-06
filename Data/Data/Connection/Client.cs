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

                Console.WriteLine($"O Cliente Id{this.clientId} numero de ip {ipCliente.ToString()} se conectou ao Servidor!");

                //buffer = new Byte[4000];
                this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, WaitData, null);

            }
            catch (Exception ex)
            {

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
                        sb.Append(Encoding.ASCII.GetString(buffer, 0, size));
                        _sProtocolReceiver = sb.ToString();
                        sb.Clear();
                        ProtocolResolver pResolver = new ProtocolResolver(_sProtocolReceiver);
                        ActionController actionController = new ActionController(pResolver, this.clientId);
                        actionController.ExecAction();

                        if (_sProtocolReceiver != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Receiver: {0}", _sProtocolReceiver);
                            Console.ResetColor();
                            //_sProtocolResponse = "teste";
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
                        Close();

                        //atualizar a lista do servidor retirando esse cliente da lista
                    }
                    
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
            }
            finally
            {
                if (ativo)
                {
                    buffer = new Byte[4000];
                    this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, WaitData, null);

                }

            }
        }

        private static void Send(Socket handler, string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length,
                0, new AsyncCallback(SendCallBack), handler);
        }

        private static void SendCallBack(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;

            Console.ForegroundColor = ConsoleColor.Red;
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

                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
            }
        }

    }
}
