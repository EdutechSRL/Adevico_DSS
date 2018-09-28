using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.HTML
{
    [Serializable()]

    public class HTMLSkin
    {
        public string HtmlHeadLogo { get; set; }
        public IList<string> HtmlFooterLogos { get; set; }
        public string FooterText { get; set; }
        public string HeaderTemplate { get; set; }
        public string FooterTemplate { get; set; }

        public HTMLSkin()
        {
            HtmlHeadLogo = "";
            HtmlFooterLogos = new List<string>();
            FooterText = "";

            HeaderTemplate = "";
            FooterTemplate = "";

        }
    }
}
