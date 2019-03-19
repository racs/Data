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
    class CreateCharacterController
    {

        private ProtocolBuilder pBuilder;

        public void CreateCharacter(string nameUser, string namePlayer, string genre, string level, string life,            
            string mana, string posx, string posy, string posz,
            string sredvalueskintmale, string sgreenvalueskinmale, string sbluevalueskinmale, string sredvaluehairtmale,
            string sgreenvaluehairtmale, string sbluevaluehairtmale, string sredvalueshirtmale, string sgreenvalueshirtmale,
            string sbluevalueshirtmale, string sredvaluepantsmale, string sgreenvaluepantsmale, string sbluevaluepantsmale, 
            int clientId)
        {

            string RetVar = "";

            pBuilder = new ProtocolBuilder("CreateCharacter");
            pBuilder.Add(nameUser);
            pBuilder.Add(namePlayer);
            pBuilder.Add(genre);
            pBuilder.Add(life);
            pBuilder.Add(mana);
            pBuilder.Add(posx);
            pBuilder.Add(posy);
            pBuilder.Add(posz);
            pBuilder.Add(sredvalueskintmale);
            pBuilder.Add(sgreenvalueskinmale);
            pBuilder.Add(sbluevalueskinmale);
            pBuilder.Add(sredvaluehairtmale);
            pBuilder.Add(sgreenvaluehairtmale);
            pBuilder.Add(sbluevaluehairtmale);
            pBuilder.Add(sredvalueshirtmale);
            pBuilder.Add(sgreenvalueshirtmale);
            pBuilder.Add(sbluevalueshirtmale);
            pBuilder.Add(sredvaluepantsmale);
            pBuilder.Add(sgreenvaluepantsmale);
            pBuilder.Add(sbluevaluepantsmale, true);
            

            using (TesteunityEntities contexto = new TesteunityEntities())
            {
                
                var queryUserName = (from u in contexto.Users
                                     where u.nome == nameUser
                                     select u).SingleOrDefault();

                var queryNamePlayer = (from p in contexto.Players
                                       where p.Nome == namePlayer
                                       select p).SingleOrDefault();

                if (queryNamePlayer != null)
                {
                    RetVar = "[Charnameexistent]"; //Name already chosen, try another one!
                }
                else
                {
                    if (queryUserName != null)
                    {

                        Player player = new Player();
                        player.ID_USER_PLAYERS = queryUserName.ID;
                        player.Nome = namePlayer;
                        player.Sexo = genre;
                        player.Nivel = Convert.ToInt32(level);
                        player.Vida = Convert.ToInt32(life);
                        player.Mana = Convert.ToInt32(mana);
                        player.PosX = Convert.ToDouble(posx);
                        player.PosY = Convert.ToDouble(posy);
                        player.PosZ = Convert.ToDouble(posz);
                        player.valorvermelhopelemasc = Convert.ToInt32(sredvalueskintmale);
                        player.valorverdepelemasc = Convert.ToInt32(sgreenvalueskinmale);
                        player.valorazulpelemasc = Convert.ToInt32(sbluevalueskinmale);
                        player.valorvermelhocabelomasc = Convert.ToInt32(sredvaluehairtmale);
                        player.valorverdecabelomasc = Convert.ToInt32(sgreenvaluehairtmale);
                        player.valorazulcabelomasc = Convert.ToInt32(sbluevaluehairtmale);
                        player.valorvermelhoblusamasc = Convert.ToDouble(sredvalueshirtmale);
                        player.valorverdeblusamasc = Convert.ToDouble(sgreenvalueshirtmale);
                        player.valorazulblusamasc = Convert.ToDouble(sbluevalueshirtmale);
                        player.valorvermelhocalcamasc = Convert.ToDouble(sredvaluepantsmale);
                        player.valorverdecalcamasc = Convert.ToDouble(sgreenvaluepantsmale);
                        player.valorazulcalcamasc = Convert.ToDouble(sbluevaluepantsmale);

                        try
                        {
                            contexto.Players.Add(player);
                            contexto.SaveChanges();
                            RetVar = "[Charsaved]"; //Character Saved!
                        }
                        catch (Exception)
                        {

                            RetVar = "[Charnotsaved]";


                        }
                    }

                }

                              

            }            

            Server._sProtocolResponse = RetVar;

        }
    }
}
