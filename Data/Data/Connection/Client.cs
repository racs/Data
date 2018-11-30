using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Data.Connection
{
    public class Client
    {
        public bool ativo = false;
        public Socket socket = null;
        public int clientId = 0;
        public Byte serverId = 0;

        public Byte[] buffer = null;

        public Client(Socket socket, Byte serverId, int clientId)
        {
            try
            {
                ativo = true;
                this.socket = socket;

                this.clientId = clientId;

                this.serverId = serverId;

                Console.WriteLine($"O Cliente {this.clientId} se conectou ao canal!");

                buffer = new Byte[4000];
                this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, WaitData, null);

            }
            catch (Exception ex)
            {

                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
            }
        }

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

                        Console.WriteLine($"{string.Join(", ", this.buffer)}");
                    }
                    else
                    {
                        Close();
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
