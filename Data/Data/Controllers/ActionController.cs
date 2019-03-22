using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RpgProtocol.Protocol;

namespace Data
{
    class ActionController
    {

        private Dictionary<string, Action> listActions;
        private ProtocolResolver pResolver;
        public Action action { get; private set; }
        private string TagAction { get; set; }
        private int clientId { get; set; }

        public ActionController(ProtocolResolver pResolverParam, int clienteId)
        {
            pResolver = pResolverParam;
            this.clientId = clienteId;
            listActions = new Dictionary<string, Action>();
            CreateActions();
            TagAction = pResolver.GetTagProtocol();
            action = new Action();
            action = GetActionByTag();
            
        }

        public void add(string tagParam, Action actionParam)
        {
            listActions.Add(tagParam, actionParam);            
        }

        private Action GetActionByTag()
        {
            if (listActions.ContainsKey(TagAction))
            {
                return listActions[TagAction];
                
            }
            return listActions["UnknTag"];

        }

        public void ExecAction()
        {
            //Typename é composto por namespace. NomedaClasse exemplo: DataServer.LoginController
            Type type = Type.GetType("Data." + action.Controller);

            object obj = Activator.CreateInstance(type);

            MethodInfo info = obj.GetType().GetMethod(action.Method);

            int size = 1;
            var obj1 = action.ObjParam;

            if (action.ObjParam != null)
            {
                if (type.FullName == "Data.UnknownTagController")
                {
                    
                    Array.Resize(ref obj1, size);
                    action.ObjParam = obj1;                    
                    action.ObjParam[0] = 1;
                    
                }

                info.Invoke(obj, action.ObjParam);
                
            }
            else
            {
                
                info.Invoke(obj, null);
                
            }                
                

        }
        private void CreateActions()
        {
            Action a;
            
            a = new Action();
            a.Controller = "LoginController";
            a.Method = "LoginMember";            
            
            a.ObjParam = pResolver.ObjectFields;
            
            var objparamlistaLoginController = a.ObjParam.ToList();
            objparamlistaLoginController.Add(this.clientId);
            a.ObjParam = objparamlistaLoginController.ToArray();            

            add("loginMemb", a);


            Action b;

            b = new Action();
            b.Controller = "UnknownTagController";
            b.Method = "UnknownTag";
            
            b.ObjParam = pResolver.ObjectFields;

            var objparamlistaUnknownTagController = b.ObjParam.ToList();
            objparamlistaUnknownTagController.Add(this.clientId);
            b.ObjParam = objparamlistaUnknownTagController.ToArray();           

            add("UnknTag", b);


            Action c;

            c = new Action();
            c.Controller = "RegisterController";
            c.Method = "RegisterLogin";

            c.ObjParam = pResolver.ObjectFields;

            var objparamlistaRegisterController = c.ObjParam.ToList();
            objparamlistaRegisterController.Add(this.clientId);
            c.ObjParam = objparamlistaRegisterController.ToArray();            

            add("RegisterLogin", c);


            Action d;

            d = new Action();
            d.Controller = "CreateCharacterController";
            d.Method = "CreateCharacter";

            d.ObjParam = pResolver.ObjectFields;

            var objparamlistaCreateCharacterController = d.ObjParam.ToList();
            objparamlistaCreateCharacterController.Add(this.clientId);
            d.ObjParam = objparamlistaCreateCharacterController.ToArray();

            add("CreateCharacter", d);


            Action e;

            e = new Action();
            e.Controller = "RequestListOfPlayersController";
            e.Method = "RequestListOfPlayers";

            e.ObjParam = pResolver.ObjectFields;

            var objparamlistaRequestListOfPlayersController = e.ObjParam.ToList();
            objparamlistaRequestListOfPlayersController.Add(this.clientId);
            e.ObjParam = objparamlistaRequestListOfPlayersController.ToArray();

            add("RequestListOfPlayers", e);



            Action f;

            f = new Action();
            f.Controller = "RequestPlayerDetailsController";
            f.Method = "RequestPlayerDetails";

            f.ObjParam = pResolver.ObjectFields;

            var objparamRequestPlayerDetailsController = f.ObjParam.ToList();
            objparamRequestPlayerDetailsController.Add(this.clientId);
            f.ObjParam = objparamRequestPlayerDetailsController.ToArray();

            add("RequestPlayerDetails", f);

            Action g;

            g = new Action();
            g.Controller = "SavePlayerPositionController";
            g.Method = "SavePlayerPosition";

            g.ObjParam = pResolver.ObjectFields;

            var objparamSavePlayerPositionController = g.ObjParam.ToList();
            objparamSavePlayerPositionController.Add(this.clientId);
            g.ObjParam = objparamSavePlayerPositionController.ToArray();

            add("SavePlayerPosition", g);

        }
        
    }
}
