using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public class RenderedSkin
    {
        //public String LangCode { get; set; }

        //public String PortalCss { get; set; }
        //public String OrganizatonCss { get; set; }
        //public String ComCss { get; set; }
        //public String PortalIECss { get; set; }
        //public String OrganizatonIECss { get; set; }
        //public String ComIECss { get; set; }
        
        /// <summary>
        /// L'HTML che contiene il caricamento del CSS di una data skin
        /// </summary>
        public String HTMLCss { get; set; }

        public String HeaderLogo { get; set; }

        public String FooterHtml { get; set; }


    }
}
