using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class RemoteWebPost
    {
        private const String formName = "Copy";
        private const String method = "POST";
        private const String inputHidden = "<input name=\"{0}\" type=\"hidden\" value=\"{1}\">";

        public virtual String PostUrl {get;set;}
        protected virtual Dictionary<String,String> ItemValues {get;set;}


        public RemoteWebPost(String url): base(){
            ItemValues = new Dictionary<String, String>();
            PostUrl = url;
        }

        public virtual void Send(Dictionary<String,String> itemvalues){
            ItemValues = itemvalues;
            Send();
        }

        protected void Send()
        {
            String content = "<html><head>\n</head>\n<body onload=\"document.{0}.submit()\">\n<form name=\"{0}\" method=\"{1}\" action=\"{2}\">\n{3}\n</form>\n</body>\n</html>";
            String values ="";

            foreach (KeyValuePair<String,String> item in ItemValues){
                values += String.Format(inputHidden, item.Key, item.Value);
            }
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write(string.Format(content, formName, method, PostUrl, values));
            System.Web.HttpContext.Current.Response.End();
        }
    }
}
