using Data.Connection;
using Data.Database;
using RpgProtocol.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class RequestListOfPlayersController
    {

        private ProtocolBuilder pBuilder;

        public void RequestListOfPlayers(string userNameParam, string nada, int clientId)
        {

            string RetVar = "";

            pBuilder = new ProtocolBuilder("RequestListOfPlayers");
            pBuilder.Add(userNameParam);
            pBuilder.Add(nada, true);
            
            using (TesteunityEntities contexto = new TesteunityEntities())
            {
                StringBuilder sb = new StringBuilder();

                //Verifica se já existe o username no BD
                var queryuserName = (from u in contexto.Users
                             where (u.nome == userNameParam)
                             select u).SingleOrDefault();


                var queryPlayers = (from p in contexto.Players
                                    where (p.ID_USER_PLAYERS == queryuserName.ID)
                                    select p).ToList();

                
                foreach (var p in queryPlayers)
                {
                    if (queryPlayers.IndexOf(p) == queryPlayers.Count - 1)
                    {
                        sb.Append(p.Nome);                        
                    }
                    else
                    {
                        sb.Append(p.Nome);
                        sb.Append("|");
                    }
                    
                }
                
                if (queryPlayers.Count == 0)
                {

                    RetVar = "[Charsinexistents]"; //no players registered for this account                   

                    
                }
                else
                {                                        
                    RetVar = "[Charsexistents]" + sb.ToString(); //Players registered!
                }

            }

            Server._sProtocolResponse = RetVar;

        }
    }
}
