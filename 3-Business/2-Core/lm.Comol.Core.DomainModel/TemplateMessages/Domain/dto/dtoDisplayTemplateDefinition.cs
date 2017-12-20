using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoDisplayTemplateDefinition : dtoBase
    {
        public virtual String Name { get; set; }
        public virtual String UserDisplayName { get; set; }
        public virtual String CreatorName { get; set; }
        public virtual String ModifiedByName { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }


        public virtual long IdLastVersion { get; set; }
        public virtual dtoBaseTemplateOwner OwnerInfo { get; set; }
        public virtual TemplateType Type { get; set; }
        public virtual Boolean IsEnabled {get;set;}
        public virtual dtoTemplatePermission Permission { get; set; }
        public virtual List<dtoDisplayTemplateVersion> Versions { get; set; }
        public virtual Boolean FromOther { get; set; }

        public dtoDisplayTemplateDefinition()
        {
            Type = TemplateType.None;
            IsEnabled = true;
            OwnerInfo = new dtoBaseTemplateOwner();
            Versions = new List<dtoDisplayTemplateVersion>();
            FromOther = false;
        }

        public dtoDisplayTemplateVersion GetActiveVersion()
        {
            return (Versions == null) ? null : Versions.Where(v => v.Deleted == BaseStatusDeleted.None && v.Status == TemplateStatus.Active).OrderByDescending(v=>v.Number).FirstOrDefault();
        }
        public long GetActiveIdVersion()
        {
            return (Versions == null) ? 0 : Versions.Where(v => v.Deleted == BaseStatusDeleted.None && v.Status == TemplateStatus.Active).OrderByDescending(v => v.Number).FirstOrDefault().Id;
        }
    }
}