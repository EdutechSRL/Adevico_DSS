using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class liteTemplateDefinition: DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual TemplateOwner OwnerInfo { get; set; }
        public virtual TemplateType Type { get; set; }
        public virtual Boolean IsEnabled {get;set;}
        public virtual String CurrentModulesContent { get; set; }
        public virtual liteTemplateDefinitionVersion LastVersion { get; set; }
        public virtual IList<liteTemplateDefinitionVersion> Versions { get; set; }
        //public virtual IList<VersionPermission> ActivePermissions { get; set; }

        public liteTemplateDefinition()
        {
            Type = TemplateType.None;
            IsEnabled = true;
            OwnerInfo = new TemplateOwner();
            Versions = new List<liteTemplateDefinitionVersion>();
           }

        public liteTemplateDefinitionVersion GetActiveVersion()
        {
            return (Versions == null) ? null : Versions.Where(v => v.Deleted == BaseStatusDeleted.None && v.Status == TemplateStatus.Active).FirstOrDefault();
        }
    }
}