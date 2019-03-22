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
    class SavePlayerPositionController
    {
        private ProtocolBuilder pBuilder;

        public void SavePlayerPosition(string charname, string x, string y, string z, int clientId)
        {

            string RetVar = "";

            pBuilder = new ProtocolBuilder("SavePlayerPosition");
            pBuilder.Add(charname);
            pBuilder.Add(x);
            pBuilder.Add(y);
            pBuilder.Add(z, true);

            using (TesteunityEntities contexto = new TesteunityEntities())
            {

                //Verifica se já existe o username no BD
                var queryPlayer = (from p in contexto.Players
                                   where p.Nome == charname
                                   select p).SingleOrDefault();

                Player player = new Player();
                player = queryPlayer;
                player.PosX = Convert.ToDouble(x);
                player.PosY = Convert.ToDouble(y);
                player.PosZ = Convert.ToDouble(z);

                contexto.SaveChanges();
                                                             

            }


            Server._sProtocolResponse = RetVar;


        }

    }
}
