using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoBaseTemplateOwner
    {
        public virtual Boolean IsPortal { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual long ModulePermission { get; set; }
        public virtual long  IdObject { get; set; }
        public virtual Int32 IdObjectType { get; set; }
        public virtual Int32 IdObjectModule { get; set; }
        public virtual Int32 IdObjectCommunity { get; set; }
        public virtual OwnerType Type { get; set; }
        public virtual Int32 IdPerson { get; set; }

        public dtoBaseTemplateOwner()
        { 
        }

        public dtoBaseTemplateOwner(TemplateOwner info)
        {
            IsPortal = info.IsPortal;
            IdCommunity = (info.Community==null) ? 0 : info.Community.Id;
            IdModule = info.IdModule;
            ModuleCode = info.ModuleCode;
            Type = info.Type;
            IdPerson = (info.Person==null) ? 0 : info.Person.Id;
            if (info.ModuleObject != null){
                IdObject = info.ModuleObject.ObjectLongID;
                IdObjectType = info.ModuleObject.ObjectTypeID;
                IdObjectModule = info.ModuleObject.ServiceID;
                IdObjectCommunity = info.ModuleObject.CommunityID;
            }
        }
    }
}