using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
 
namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoModuleContext
    {
        public virtual ModuleGenericTemplateMessages Permissions { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual long ModulePermissions { get; set; }
        public virtual TemplateLoaderType LoaderType { get; set; }
        public virtual ModuleObject ModuleObject { get; set; }
        public virtual String BackUrl { get; set; }
        public virtual long IdAction { get; set; }
        public virtual Boolean AlsoEmptyActions { get; set; }
        public dtoModuleContext()
        {

        }
        //public static dtoModuleContext CreateContext(TemplateLoaderType type, ModuleGenericTemplateMessages p, Int32 idCommunity, Int32 idModule, String moduleCode = "", long permissions = 0,ModuleObject obj=null)
        //{
        //    return new dtoModuleContext() { IdCommunity = idCommunity, LoaderType = type, IdModule = idModule, ModuleCode = moduleCode, ModulePermissions = permissions, ModuleObject = obj };
        //}
        public virtual TemplateType GetStandardType() {
            switch (LoaderType) {
                case TemplateLoaderType.OtherModule:
                case TemplateLoaderType.Module:
                    return TemplateType.Module;
                case  TemplateLoaderType.System:
                    return TemplateType.System;
                case  TemplateLoaderType.User:
                    return TemplateType.User;
                default:
                    return TemplateType.None;
            }
        }
    }

    [Serializable]
    public enum TemplateLoaderType
    {
        None = 0,
        System = 1,
        Module = 2,
        User = 4,
        OtherModule =8
    }
}