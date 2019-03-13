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

        public void LoginMember(string userParam, string PwdParam, string firstlogin, int clientId)
        {

            string RetVar = "";

            pBuilder = new ProtocolBuilder("LoginMemb");
            pBuilder.Add(userParam);
            pBuilder.Add(PwdParam);
            pBuilder.Add(firstlogin, true);

            using (TesteunityEntities contexto = new TesteunityEntities())
            {
                                
                //Verifica se já existe o username no BD
                var query = (from u in contexto.Users
                             where (u.nome==userParam) && (u.senha == PwdParam)
                             select u).SingleOrDefault();
                

                //if (queryUsername != null)
                //{
                //    var queryPassword = (from p in queryUsername.nome
                //                         where (p.senha == PwdParam)
                //                         select p).SingleOrDefault();
                //}
                               
               

                if (query != null)
                {
                    if (query.senha != PwdParam)
                    {
                        RetVar = "Login failed!";
                    }
                    else
                    {
                        if (query.primeirologin == true)
                        {
                            RetVar = "Login successful! Welcome to the Magic Land, I see it' s your first time here.";
                            query.primeirologin = false;
                            contexto.SaveChanges();
                        }
                        else
                        {
                            RetVar = "Login successful!";
                        }
                        
                    }
                }
                else
                {
                    RetVar = "Login not found!";
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
