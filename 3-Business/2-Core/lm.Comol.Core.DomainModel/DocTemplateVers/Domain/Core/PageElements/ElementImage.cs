using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    [Serializable]
    public class ElementImage : PageElement
    {
        public virtual string Path { get; set; }
        public virtual Int16 Width { get; set; }
        public virtual Int16 Height { get; set; }

        public override PageElement Copy(TemplateVersion TemplateVersion, bool IsActive, Int64 SubVersion, Person Person, string ipAddrees, string IpProxyAddress)
        {
            ElementImage NewEl = new ElementImage();
            NewEl.CreateMetaInfo(Person, ipAddrees, IpProxyAddress);

            //Base
            NewEl.SubVersion = SubVersion;
            NewEl.IsActive = IsActive;
            NewEl.TemplateVersion = TemplateVersion;
            NewEl.Position = this.Position;
            NewEl.Alignment = this.Alignment;

            //Image
            NewEl.Path = this.Path;
            NewEl.Width  = this.Width;
            NewEl.Height = this.Height;

            return NewEl;
        }
    }
}
