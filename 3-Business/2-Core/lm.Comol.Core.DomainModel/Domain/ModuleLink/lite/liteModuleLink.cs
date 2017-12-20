
using System;
using System.Runtime.Serialization;

namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true), DataContract]
    public class liteModuleLink : DomainBaseObjectIdLiteMetaInfo<long>
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

        public liteModuleLink()
            : base()
        {
            Permission = 0;
            Link = "";
            Description = "";
            Deleted = BaseStatusDeleted.None;
            SourceItem = new ModuleObject();
            NotifyExecution = true;
            AutoEvaluable = true;
        }
        public liteModuleLink(String link, int action)
            : this()
        {
            Link = link;
            Action = action;
        }
        public liteModuleLink(int permission, int action)
            : this()
        {
            Permission = permission;
            Action = action;
        }
        public liteModuleLink(String link, int permission, int action)
            : this()
        {
            Permission = permission;
            Link = link;
            Action = action;
        }
        public liteModuleLink(String link, long permission, int action)
            : this()
        {
            Permission = permission;
            Link = link;
            Action = action;
        }
        public liteModuleLink(ModuleLink link)
            : this()
        {
            SourceItem = link.SourceItem;
            DestinationItem = link.DestinationItem;
            Description = link.Description;
            Link = link.Link;
            Action = link.Action;
            Permission = link.Permission;
            EditEnabled = link.EditEnabled;
            NotifyExecution = link.NotifyExecution;
            AutoEvaluable = link.AutoEvaluable;
            Id = link.Id;
            IdCreatedBy = (link.CreatedBy != null ? link.CreatedBy.Id : 0);
            IdModifiedBy = (link.CreatedBy != null ? link.ModifiedBy.Id : 0);
            CreatedOn = link.CreatedOn;
            ModifiedOn = link.ModifiedOn;

        }
        public virtual String ToString()
        {
            return "Id:" + Id.ToString() + " NotifyExecution:" + NotifyExecution.ToString() + " AutoEvaluable:" + AutoEvaluable.ToString() + " IdAction:" + Action.ToString() + " SourceItem:" + SourceItem.ToString() + " DestinationItem:" + DestinationItem.ToString();
        }
    }
}