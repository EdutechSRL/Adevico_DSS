using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport
{
    [Serializable]
    public class DTO_Template
    {
        //public DTO_Template()
        //{
        //    this.Body = new DTO_ElementText();
        //}

        public virtual Int64 TemplateId { get; set; }
        public virtual Int64 VersionId { get; set; }
        public virtual int Version { get; set; }

        public virtual string Name { get; set; }

        public virtual Boolean UseSkinHeaderFooter { get; set; }
        public virtual Boolean IsSystem { get; set; }

        public virtual DTO_Settings Settings { get; set; }

        public virtual DTO_HeaderFooter Header { get; set; }

        public virtual IList<DTO_Modules> Modules { get; set; }

        public virtual DTO_ElementText Body { get; set; }

        public virtual DTO_HeaderFooter Footer { get; set; }

        public virtual IList<DTO_Signature> Signatures { get; set; }

        public virtual Boolean IsEditable { get; set; }

    }
}
