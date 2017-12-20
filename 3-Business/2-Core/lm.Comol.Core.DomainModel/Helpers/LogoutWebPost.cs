using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class LogoutWebPost :RemoteWebPost
    {
        public LogoutWebPost(String url)
            : base(url)
        {
        }
        public void Send(dtoExpiredAccessUrl item){
            base.ItemValues.Add("DestinationUrl",item.DestinationUrl);
            base.ItemValues.Add("Display",item.Display.ToString());
            base.ItemValues.Add("CodeLanguage",item.CodeLanguage);
            base.ItemValues.Add("IdCommunity",item.IdCommunity.ToString());
            base.ItemValues.Add("IdLanguage",item.IdLanguage.ToString());
            base.ItemValues.Add("IdPerson",item.IdPerson.ToString());
            base.ItemValues.Add("Preserve", item.Preserve.ToString());
            base.ItemValues.Add("GetType", item.GetType().ToString());
            
            base.Send();
        }

        public void Redirect(dtoExpiredAccessUrl item)
        {
            Redirect(item, (long)5);
        }

        public void Redirect(dtoExpiredAccessUrl item, long minutes) { 
            System.Web.HttpCookie cookie = new System.Web.HttpCookie("LogoutAccess");
            cookie.Expires = DateTime.Now.AddMinutes(minutes);
            cookie.Values.Add("Display", item.Display.ToString());
            cookie.Values.Add("CodeLanguage", item.CodeLanguage);
            cookie.Values.Add("IdCommunity", item.IdCommunity.ToString());
            cookie.Values.Add("IdLanguage", item.IdLanguage.ToString());
            cookie.Values.Add("IdPerson", item.IdPerson.ToString());
            cookie.Values.Add("Preserve", item.Preserve.ToString());
            cookie.Values.Add("GetType", item.GetType().ToString());
            if (!String.IsNullOrEmpty(item.DestinationUrl))
                cookie.Values.Add("DestinationUrl", item.DestinationUrl.Replace("&", "#_#"));
            else
                cookie.Values.Add("DestinationUrl", "");
            cookie.Values.Add("IsForDownload", item.IsForDownload.ToString());
            if (!String.IsNullOrEmpty(item.PreviousUrl))
                cookie.Values.Add("PreviousUrl", item.PreviousUrl.Replace("&", "#_#"));
            else
                cookie.Values.Add("PreviousUrl", "");
            cookie.Values.Add("SendToHomeDashboard", item.SendToHomeDashboard.ToString());
            if (System.Web.HttpContext.Current.Response.Cookies.AllKeys.Contains("LogoutAccess"))
                System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
            else
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            System.Web.HttpContext.Current.Response.Redirect(PostUrl);
        }
    }
}