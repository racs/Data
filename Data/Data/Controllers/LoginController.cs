using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RpgProtocol.Protocol;
using Data.Connection;

namespace Data
{
    class LoginController
    {

        private ProtocolBuilder pBuilder;

        public void LoginMember(string userParam, string PwdParam)
        {

            string RetVar = "";

            pBuilder = new ProtocolBuilder("LoginMemb");
            pBuilder.Add(userParam);
            pBuilder.Add(PwdParam, true);

            if (userParam == "Ricardo" && PwdParam == "123456")
            {
                RetVar = pBuilder.GetProtocol();
            }
            else
            {
                RetVar = "Login e Senha inválidos";
            }

            Server._sProtocolResponse = RetVar;

        }
    }
}
