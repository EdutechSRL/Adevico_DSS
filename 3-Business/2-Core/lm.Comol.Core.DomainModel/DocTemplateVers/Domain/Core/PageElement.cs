using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(ElementText))]
    [System.Xml.Serialization.XmlInclude(typeof(ElementImage))]
    [System.Xml.Serialization.XmlInclude(typeof(ElementVoid))]
    public abstract class PageElement : lm.Comol.Core.DomainModel.DomainBaseObjectMetaInfo<Int64>
    {
        //public virtual int Version { get; set; }
        public virtual Int64 SubVersion { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual TemplateVersion TemplateVersion { get; set; }

        public virtual ElementPosition Position { get; set; }
        public virtual ElementAlignment Alignment { get; set; }

        public abstract PageElement Copy(
   TemplateVersion TemplateVersion, Boolean IsActive, Int64 SubVersion
   , Person Person, String ipAddrees, String IpProxyAddress);

        //{
        //    PageElement NewEl = new ElementVoid();
        //    NewEl.CreateMetaInfo(Person, ipAddrees, IpProxyAddress);

        //    //Base
        //    NewEl.Version = Version;
        //    NewEl.IsActive = IsActive;
        //    NewEl.TemplateVersion = TemplateVersion;
        //    NewEl.Position = this.Position;
        //    NewEl.Alignment = this.Alignment;

        //    return NewEl;
        //}
    }

    
}
