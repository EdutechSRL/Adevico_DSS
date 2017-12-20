
using System;
using System.Runtime.Serialization;

namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true), DataContract]
    public class ModuleLink : DomainBaseObjectMetaInfo<long>, iModuleLink
    { 
        public virtual ModuleObject SourceItem { get; set; }
        public virtual ModuleObject DestinationItem { get; set; }
        public virtual string Description { get; set; }
        public virtual string Link { get; set; }
        public virtual int Action { get; set; }
        public virtual long Permission { get; set; }
        public virtual bool EditEnabled { get; set; }
        public virtual bool NotifyExecution { get; set; }
        public virtual bool AutoEvaluable { get; set; }
    
        public ModuleLink(): base(){
            Permission = 0;
            Link = "";
            Description = "";
            Deleted = BaseStatusDeleted.None;
            SourceItem = new ModuleObject();
            NotifyExecution = true;
            AutoEvaluable = true;
        }
        public ModuleLink (String link, int action): this()
        {
            Link = link;
            Action = action;
        }
        public ModuleLink (int permission, int action): this()
        {
            Permission = permission;
            Action = action;
        }
        public ModuleLink(String link, int permission, int action)
            : this()
        {
            Permission = permission;
            Link = link;
            Action = action;
        }
        public ModuleLink(String link, long permission, int action)
            : this()
        {
            Permission = permission;
            Link = link;
            Action = action;
        }
    }
}