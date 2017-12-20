using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    [Serializable]
    public class ElementText : PageElement
    {
        public virtual String Text { get; set; }
        public virtual Boolean IsHTML { get; set; }

        public override PageElement Copy(TemplateVersion TemplateVersion, bool IsActive, Int64 SubVersion, Person Person, string ipAddrees, string IpProxyAddress)
        {
            ElementText NewEl = new ElementText();
            NewEl.CreateMetaInfo(Person, ipAddrees, IpProxyAddress);

            //Base
            NewEl.SubVersion = SubVersion;
            NewEl.IsActive = IsActive;
            NewEl.TemplateVersion = TemplateVersion;
            NewEl.Position = this.Position;
            NewEl.Alignment = this.Alignment;

            //Text
            NewEl.Text = this.Text;
            NewEl.IsHTML = this.IsHTML;

            return NewEl;
        }
    }
}
