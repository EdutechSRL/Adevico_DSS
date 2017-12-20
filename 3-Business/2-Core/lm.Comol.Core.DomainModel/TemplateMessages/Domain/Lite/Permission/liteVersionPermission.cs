using System;
using System.Linq;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable()]
    public class liteVersionPermission : DomainBaseObject<long> 
    {
        public virtual Boolean Clone {get;set;}
        public virtual Boolean Edit { get; set; }
        public virtual Boolean See { get; set; }
        public virtual Boolean ChangePermission { get; set; }
        public virtual PermissionType Type { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual long  IdTemplate { get; set; }
        public virtual Boolean ToApply { get; set; }

        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual Int32 IdProfileType { get; set; }

    }
}