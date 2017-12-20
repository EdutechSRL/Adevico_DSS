using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    public class DTO_EditTemplateVersion
    {
        public virtual DTO_EditPermission Permissions { get; set; }
        //public virtual TemplateVersion TemplateData { get; set; }

        public virtual Int64 IdTemplate { get; set; }
        public virtual Int64 Id { get; set; }
        public virtual String TemplateName { get; set; }

        public virtual DTO_EditItem<Settings> Setting { get; set; }
        //public virtual IList<Signature> PreviousSignatures { get; set; }

        public virtual DTO_EditItem<PageElement> HeaderLeft { get; set; }
        public virtual DTO_EditItem<PageElement> HeaderCenter { get; set; }
        public virtual DTO_EditItem<PageElement> HeaderRight { get; set; }

        public virtual IList<DTO_EditItem<ServiceContent>> Services { get; set; }
        public virtual IList<ServiceContent> ServicesPrevious { get; set; }

        public virtual DTO_EditItem<ElementText> Body { get; set; }

        public virtual IList<DTO_EditItem<Signature>> Signatures { get; set; }
        public virtual IList<DTO_EditPreviousVersion> SignaturesPrevious { get; set; }

        public virtual DTO_EditItem<PageElement> FooterLeft { get; set; }
        public virtual DTO_EditItem<PageElement> FooterCenter { get; set; }
        public virtual DTO_EditItem<PageElement> FooterRight { get; set; }

        public virtual VersionEditError Error { get; set; }

        public DTO_EditTemplateVersion()
        {
            Error = VersionEditError.none;
        }
    }


}
