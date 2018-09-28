using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.DTO
{
    public class DTO_MailTagSettings
    {
        public DTO_MailTagSettings()
        {
            this.Baseurl = "";
            WebSiteUrlNoSsl = "";
            this.DateTimeFormat = "";
            //this.OpenTag = "[";
            //this.CloseTag = "]";
            VoidDateTime = "--";
            PortalName = "Comol";
            SmtpConfig = new lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig();
        }

        public string Baseurl { get; set; }
        public string WebSiteUrlNoSsl { get; set; }
        public string DateTimeFormat { get; set; }
        //public string OpenTag { get; set; }
        //public string CloseTag { get; set; }
        public string VoidDateTime { get; set; }
        public string PortalName { get; set; }

        public lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig SmtpConfig { get; set; }
    }
}
