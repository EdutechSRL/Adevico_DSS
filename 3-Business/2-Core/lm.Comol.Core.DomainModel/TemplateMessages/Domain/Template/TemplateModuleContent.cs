using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class TemplateModuleContent : DomainBaseObject<long> //, IDisposable 
    {
        public virtual Boolean IsActive { get; set; }
        public virtual TemplateDefinitionVersion Version { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }

        public TemplateModuleContent()
        { 
        
        }
        public virtual TemplateModuleContent Copy(TemplateDefinitionVersion version, Boolean isActive)
        {
            TemplateModuleContent cp = new TemplateModuleContent();

            cp.Version = version;
            cp.IsActive = isActive;

            cp.IdModule = this.IdModule;
            cp.ModuleCode = this.ModuleCode;
            return cp;
        }
        //public void Dispose()
        //{
        //}
    }
}