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
            return listActions[TagAction];
        }

        public void ExecAction()
        {
            //Typename é composto por namespace. NomedaClasse exemplo: DataServer.LoginController
            Type type = Type.GetType("Data." + action.Controller);

            object obj = Activator.CreateInstance(type);

            MethodInfo info = obj.GetType().GetMethod(action.Method);

            if (action.ObjParam != null)
            {
                
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
            
            var objparamlista = a.ObjParam.ToList();
            objparamlista.Add(this.clientId);
            a.ObjParam = objparamlista.ToArray();            

            add("loginMemb", a);
        }
        
    }
}
