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
    class RequestPlayerDetailsController
    {

        private ProtocolBuilder pBuilder;

        public void RequestPlayerDetails(string playerName, string nada, int clientId)
        {

            string RetVar = "";

            pBuilder = new ProtocolBuilder("RequestPlayerDetails");
            pBuilder.Add(playerName);
            pBuilder.Add(nada, true);

            using (TesteunityEntities contexto = new TesteunityEntities())
            {
                //StringBuilder sb = new StringBuilder();

                string details;

                //Verifica se já existe o username no BD
                var queryPlayer = (from p in contexto.Players
                                   where (p.Nome == playerName)
                                   select p).SingleOrDefault();
               

                details = queryPlayer.Nome + "|" + 
                    queryPlayer.Sexo + "|" +
                    queryPlayer.Nivel.ToString() + "|" + 
                    queryPlayer.Vida.ToString() + "|" + 
                    queryPlayer.Mana.ToString() + "|" +
                    queryPlayer.PosX.ToString() + "|" +
                    queryPlayer.PosY.ToString() + "|" +
                    queryPlayer.PosZ.ToString() + "|" +
                    queryPlayer.valorvermelhopelemasc.ToString() + "|" +
                    queryPlayer.valorverdepelemasc.ToString() + "|" +
                    queryPlayer.valorazulpelemasc.ToString() + "|" +
                    queryPlayer.valorvermelhocabelomasc.ToString() +"|" +
                    queryPlayer.valorverdecabelomasc + "|" +
                    queryPlayer.valorazulcabelomasc + "|" +
                    queryPlayer.valorvermelhoblusamasc + "|" +
                    queryPlayer.valorverdeblusamasc + "|" +
                    queryPlayer.valorazulblusamasc + "|" +
                    queryPlayer.valorvermelhocalcamasc + "|" +
                    queryPlayer.valorverdecalcamasc + "|" +
                    queryPlayer.valorazulcalcamasc;



                RetVar = "[Chardetails]" + details; //detalhes


            }

            Server._sProtocolResponse = RetVar;

        }

    }
}
