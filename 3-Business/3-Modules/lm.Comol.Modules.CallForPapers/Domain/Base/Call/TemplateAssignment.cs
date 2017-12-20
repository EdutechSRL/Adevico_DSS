using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class TemplateAssignment : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual BaseForPaper Call { get; set; }
        public virtual SubmitterType SubmitterType { get; set; }
        public virtual SubmitterTemplateMail Template { get; set; }

        public TemplateAssignment()
        {
            Deleted = BaseStatusDeleted.None;
        }
        public TemplateAssignment(SubmitterTemplateMail template, SubmitterType submitterType)
        {
            SubmitterType = submitterType;
            Template = template;
            if (template != null)
                Call = template.Call;
            Deleted = BaseStatusDeleted.None;
        }
    }
}
