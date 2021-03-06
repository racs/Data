﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RpgProtocol.Protocol;
using Data.Connection;
using Data.Database;

namespace Data
{
    class RegisterController
    {

        private ProtocolBuilder pBuilder;

        public void RegisterLogin(string userParam, string PwdParam, string emailParam, string firsLogin, int clientId)
        {

            string RetVar = "";

            pBuilder = new ProtocolBuilder("RegisterLogin");
            pBuilder.Add(userParam);
            pBuilder.Add(PwdParam);
            pBuilder.Add(emailParam);
            pBuilder.Add(firsLogin, true);

            using (TesteunityEntities contexto = new TesteunityEntities())
            {

                //Verifica se já existe o username no BD
                var queryUserName = (from u in contexto.Users
                             where u.nome == userParam
                             select u).ToList();

                var queryEmail = (from u in contexto.Users
                             where u.email == emailParam
                                  select u).ToList();

                if (queryUserName.Count == 0)
                {
                    if (queryEmail.Count == 0)
                    {
                        User user = new User();
                        user.nome = userParam;
                        user.senha = PwdParam;
                        user.email = emailParam;
                        if (firsLogin == "1")
                        {
                            user.primeirologin = true;
                        }
                        
                        try
                        {
                            contexto.Users.Add(user);
                            contexto.SaveChanges();
                            RetVar = "[Regok]"; //Cadastro realizado com sucesso!
                        }
                        catch (Exception e)
                        {
                            RetVar = "[Regnotsaved]"; //User not saved, try again later!
                        }
                    }
                    else
                    {
                        RetVar = "[Regemailexistent]"; //E-mail já utilizado, tente cadastrar outro
                    }
                    
                }
                else
                {
                    RetVar = "[Regnameexistent]"; //Username já existe, tente cadastrar outro!
                }

            }

            

            Server._sProtocolResponse = RetVar;

        }
    }
}
