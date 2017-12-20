
using System;
using System.Runtime.Serialization;

namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true), DataContract]
    public class ModuleActionLink : iModuleActionLink 
    {
        public virtual iModuleObject ModuleObject { get; set; }
        public virtual string Description { get; set; }
        public virtual string Link { get; set; }
        public virtual int Action { get; set; }
        public virtual int Permission { get; set; }
        public virtual bool EditEnabled { get; set; }
        public virtual bool NotifyExecution { get; set; }


         public ModuleActionLink(): base(){
            Permission = 0;
            Link = "";
            Description = "";
            NotifyExecution = true;
        }


        public ModuleActionLink (String link, int action): this()
        {
            Link = link;
            Action = action;
        }
        public ModuleActionLink (int permission, int action): this()
        {
            Permission = permission;
            Action = action;
        }
        public ModuleActionLink(String link, int permission, int action) : this()
        {
            Permission = permission;
            Link = link;
            Action = action;
        }
    }
}