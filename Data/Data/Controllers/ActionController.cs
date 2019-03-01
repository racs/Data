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

        }
        
    }
}
