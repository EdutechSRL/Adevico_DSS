using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport
{
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(DTO_ElementImage))]
    [System.Xml.Serialization.XmlInclude(typeof(DTO_ElementImageMulti))]
    [System.Xml.Serialization.XmlInclude(typeof(DTO_ElementText))]
    public class DTO_Element
    {
        public virtual Int64 Id { get; set; }
        public virtual ElementAlignment Alignment { get; set; }
        //public virtual ElementPosition Position { get; set; }
    }
}

