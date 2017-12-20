using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class dtoOwnership
    {
        public virtual OwnershipType Type { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual ModuleObject ModuleObject { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual Boolean IsPortal { get; set; }

        public dtoOwnership(){
        
        }
        public dtoOwnership(ModuleObject obj){
            Type = OwnershipType.Object;
        }
        public dtoOwnership(Boolean isPortal, Int32 idCommunity,Int32 idModule,String modulecode){
            Type = (isPortal && idCommunity <1) ? OwnershipType.Portal : OwnershipType.Community;
            IsPortal = isPortal || (idCommunity ==0);
            IdModule = idModule;
            ModuleCode = modulecode;
            IdCommunity = idCommunity;
        }
    }
}