using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    [Serializable]
    public class ServiceContent : lm.Comol.Core.DomainModel.DomainBaseObjectMetaInfo<Int64>
    {
        public virtual int Version { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual Template Template { get; set; }

        public virtual Int64 ModuleId { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual String ModuleName { get; set; }
        
        public virtual ServiceContent Copy(
    Template Template, Boolean IsActive, int Version
    , Person Person, String ipAddrees, String IpProxyAddress)
        {
            ServiceContent NewServiceContent = new ServiceContent();
            NewServiceContent.CreateMetaInfo(Person, ipAddrees, IpProxyAddress);

            NewServiceContent.Template = Template;
            NewServiceContent.Version = Version;
            NewServiceContent.IsActive = IsActive;

            NewServiceContent.ModuleId = this.ModuleId;
            NewServiceContent.ModuleCode = this.ModuleCode;
            NewServiceContent.ModuleName = this.ModuleName;
            
            return NewServiceContent;
        }
    }
}
