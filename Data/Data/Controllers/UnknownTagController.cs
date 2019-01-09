using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RpgProtocol.Protocol;
using Data.Connection;

namespace Data
{
    class UnknownTagController
    {
        private ProtocolBuilder pBuilder;

        public void UnknownTag(int clientId)
        {

            string RetVar = "";

            pBuilder = new ProtocolBuilder("UnknTag");


            RetVar = "Protocolo desconhecido";


            Server._sProtocolResponse = RetVar;

        }
    }
}
