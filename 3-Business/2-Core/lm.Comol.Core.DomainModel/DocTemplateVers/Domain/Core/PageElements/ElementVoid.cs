using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    [Serializable]
    public class ElementVoid : PageElement
    {
        public override PageElement Copy(TemplateVersion TemplateVersion, bool IsActive, Int64 SubVersion, Person Person, string ipAddrees, string IpProxyAddress)
        {
            ElementVoid NewEl = new ElementVoid();
            NewEl.CreateMetaInfo(Person, ipAddrees, IpProxyAddress);

            //Base
            NewEl.SubVersion = SubVersion;
            NewEl.IsActive = IsActive;
            NewEl.TemplateVersion = TemplateVersion;
            NewEl.Position = this.Position;
            NewEl.Alignment = this.Alignment;

            //Void: no data!

            return NewEl;
        }
    }
}
