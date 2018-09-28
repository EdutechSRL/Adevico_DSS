using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{

    public enum AssignmentDiscriminator
    { 
        BaseClass = 0,
        PathRole = 1,
        PathPerson = 2,
        UnitRole=3,
        UnitPerson=4,
        ActivityRole = 5,
        ActivityPerson = 6,
        SubActivityRole = 7,
        SubActivityPerson = 8
     }
    
     [Serializable()]
    public abstract class BaseAssignment : DomainBaseMetaInfoStatus
    {
         public virtual RoleEP RoleEP { get; set; }
         public virtual StatusAssignment StatusAssignment { get; set; }

        
         //+EP[Rob]
         public virtual Boolean Active { get; set; }
         public virtual DateTime? StartDate { get; set; }
         public virtual DateTime? EndDate { get; set; }

         /// <summary>
         /// -1 is default value= not assigned personally mincompletion
         /// </summary>
         public virtual Int64 MinCompletion { get; set; }
        
         /// <summary>
         /// unused
         /// </summary>
         public virtual Int64 Completion { get; set; }
        
         public virtual AssignmentDiscriminator  Discriminator { get; protected set; }

         public BaseAssignment(long Id, RoleEP RoleEP,  Int64 MinCompletion, Int32 idCreatedBy, 
             DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress)
         {
             this.Id = Id;
             CreateMetaInfo(idCreatedBy, CreatorIpAddress, CreatorProxyIpAddress, CreatedOn);
             this.RoleEP = RoleEP;            
             this.MinCompletion = MinCompletion;
             Status = Status.None;
         }
         //public BaseAssignment(long Id, RoleEP RoleEP, Int64 MinCompletion, Int32 idCreatedBy,
         //   DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress,
         //   Int32 idModifiedBy, DateTime? ModifiedOn, String ModifiedIpAddress, String ModifiedProxyIpAddress)
         //{
         //    this.Id = Id;
         //    CreateMetaInfo(idCreatedBy, CreatorIpAddress, CreatorProxyIpAddress, CreatedOn);
         //    UpdateMetaInfo(idModifiedBy, ModifiedIpAddress, ModifiedProxyIpAddress, ModifiedOn);
         //    this.RoleEP = RoleEP;
         //    this.MinCompletion = MinCompletion;
         //    Status = Status.None;
         //}
         public BaseAssignment()
         {
             Status = Status.None;

             //+EP[Rob]
             this.Active = true;
         }
     }



}
