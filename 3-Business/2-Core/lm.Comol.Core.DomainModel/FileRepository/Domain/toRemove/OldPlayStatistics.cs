using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.toRemove
{
    [Serializable]
    public class OldPlayStatistics
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdPerson {get; set;}
        public virtual DateTime CreatedOn { get; set; }

        public virtual long IdFile { get; set; }
        public virtual System.Guid FileUniqueID { get; set; }
      
      
        public virtual Int32 IdRepositoryItemType { get; set; }
        public virtual Int32 IdCommunity {get; set;}
        public virtual Int32 DateZone {get; set;}
        public virtual Int32 IdAction {get; set;}
        public virtual String WorkingSessionID {get; set;}
        public virtual Boolean Transferred { get; set; }
        public virtual Boolean Removed { get; set; } 
    }
    [Serializable]
    public class NewPlayStatistics
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdPerson { get; set; }

        public virtual long IdItem { get; set; }
        public virtual System.Guid UniqueIdItem { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }
        public virtual Int32 ItemType { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual Int32 DateZone { get; set; }
        public virtual long IdAction { get; set; }
        public virtual String WorkingSessionID { get; set; }
        public virtual String CreatedIPaddress { get; set; }
        public virtual String CreatedProxyIPaddress { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }

    }
}