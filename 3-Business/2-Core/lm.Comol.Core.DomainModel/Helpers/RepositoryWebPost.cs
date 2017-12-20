using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class RepositoryWebPost : RemoteWebPost
    {
        public RepositoryWebPost(String url) : base(url){}

        public virtual void AddItem(String key, String value) {
            base.ItemValues.Add(key, value);
        }
        public virtual void Send(String query)
        {
            if (!String.IsNullOrEmpty(query)) {
                if (query.StartsWith("?"))
                    query = query.Remove(0, 1);
                System.Web.HttpCookie cookie = new System.Web.HttpCookie("fileDownload", query);
                cookie.Expires = DateTime.Now.AddMinutes(10);
                System.Web.HttpContext.Current.Response.AppendCookie(cookie);

            }
            base.Send();
        }
    }
}
