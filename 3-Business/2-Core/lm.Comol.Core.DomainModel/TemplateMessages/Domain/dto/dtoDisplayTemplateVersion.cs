using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoDisplayTemplateVersion : dtoBase
    {
        public virtual Int32 Number { get; set; }
        public virtual String Name { get; set; }
        public virtual String UserDisplayName { get; set; }
        public virtual String CreatorName { get; set; }
        public virtual String ModifiedByName { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual long IdTemplate { get; set; }
        public virtual dtoDisplayTemplateDefinition Template { get; set; }
        


        public virtual TemplateStatus Status { get; set; }
        public virtual litePerson Owner { get; set; }
        public virtual Boolean HasShortText { get; set; }
        public virtual Boolean OnlyShortText { get; set; }
        public virtual ItemDisplayAs DisplayAs { get; set; }
        public virtual dtoTemplatePermission Permission { get; set; }
        public dtoDisplayTemplateVersion()
        {
            DisplayAs = ItemDisplayAs.item;
            Permission = new dtoTemplatePermission();
        }
    }

    [Serializable, Flags]
    public enum ItemDisplayAs
    {
        first = 1,
        item = 2,
        last = 4
    }
}