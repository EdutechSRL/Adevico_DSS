using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class liteTemplateModuleContent : DomainBaseObject<long> //, IDisposable 
    {
        public virtual Boolean IsActive { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }

        public liteTemplateModuleContent()
        { 
        
        }
        public virtual liteTemplateModuleContent Copy(liteTemplateDefinitionVersion version, Boolean isActive)
        {
            liteTemplateModuleContent cp = new liteTemplateModuleContent();

            cp.IdVersion = IdVersion;
            cp.IsActive = isActive;

            cp.IdModule = IdModule;
            cp.ModuleCode = ModuleCode;
            return cp;
        }
        //public void Dispose()
        //{
        //}
    }
}