using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RpgProtocol.Protocol;
using Data.Connection;
using Data.Database;

namespace Data
{
    class LoginController
    {

        private ProtocolBuilder pBuilder;

        public void LoginMember(string userParam, string PwdParam, int clientId)
        {

            string RetVar = "";

            pBuilder = new ProtocolBuilder("LoginMemb");
            pBuilder.Add(userParam);
            pBuilder.Add(PwdParam, true);

            using (TesteunityEntities contexto = new TesteunityEntities())
            {
                                
                //Verifica se já existe o username no BD
                var query = (from u in contexto.Users
                             where (u.nome==userParam) && (u.senha==PwdParam)
                             select u).SingleOrDefault();

                if (query != null)
                {
                    RetVar = "Login succesful!";
                }
                else
                {
                    RetVar = "Login Failed!";
                }

            }
            //if (userParam == "Ricardo" && PwdParam == "123456")
            //{
            //    RetVar = "O Cliente " + clientId + " enviou os dados: " + pBuilder.GetProtocol();
            //}
            //else
            //{
            //    RetVar = "O Cliente " + clientId + " digitou: Login e Senha inválidos";
            //}

            Server._sProtocolResponse = RetVar;

        }
        
    }
}
