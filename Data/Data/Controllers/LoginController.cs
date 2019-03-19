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
               

                if (query != null)
                {
                    if (query.senha != PwdParam)
                    {
                        RetVar = "[Loginfail]"; //Login failed!
                    }
                    else
                    {
                        if (query.primeirologin == true)
                        {
                            RetVar = "[Loginok1]"; //Login ok! Welcome to your first time here.
                            query.primeirologin = false;
                            contexto.SaveChanges();
                        }
                        else
                        {
                            RetVar = "[Loginok]"; //Login ok!
                        }
                        
                    }
                }
                else
                {
                    RetVar = "[Loginfail]"; //Login failed!
                }

            }
            
            Server._sProtocolResponse = RetVar;

        }
        
    }
}
