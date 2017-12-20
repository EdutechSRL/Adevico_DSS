using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.toRemove
{
    [Serializable]
    public class TransferVersion
    {
        public virtual long Id { get; set; }
        public virtual long IdItem { get; set; }
        public virtual System.Guid UniqueIdItem { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual long IdPlayer { get; set; }
        
        public virtual Int32 IdPerson {get; set;}
        public virtual String Name {get; set;}
        public virtual String Extension {get; set;}
        public virtual String Url { get; set; }
        
        public virtual String Description {get; set;}
        public virtual String ContentType {get; set;}
        public virtual long Size {get; set;}
        public virtual long Downloaded {get; set;}

        public virtual ItemAvailability Availability { get; set; }
        public virtual ItemStatus Status { get; set; }
          public virtual Int32 ItemType {get; set;}
  public virtual Int32 DisplayMode {get; set;}
        public virtual String Thumbnail { get; set; }
        public virtual Boolean AutoThumbnail { get; set; }
        public virtual long PreviewTime { get; set; }
        public virtual long Time { get; set; }
        public virtual long Number {get; set;}
        public virtual Int32 _Deleted { get; set; }
        public virtual DateTime _CreatedOn { get; set; }
        public virtual Int32 _CreatedBy { get; set; }
        public virtual DateTime _ModifiedOn { get; set; }
        public virtual Int32 _ModifiedBy { get; set; }
        public virtual String _CreatedProxyIPaddress { get; set; }
        public virtual String _CreatedIPaddress { get; set; }
        public virtual String _ModifiedIPaddress { get; set; }
        public virtual String _ModifiedProxyIPaddress { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }
    }
}