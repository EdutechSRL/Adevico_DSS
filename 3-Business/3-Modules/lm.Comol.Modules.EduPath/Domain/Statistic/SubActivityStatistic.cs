using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class SubActivityStatistic:BaseStatistic
    {
        public virtual SubActivity SubActivity { get; set; }
        public virtual long IdActivity { get; set; }
        public virtual long IdUnit { get; set; }
        public virtual long IdPath { get; set; }

   //     public virtual ActivityStatistic ParentStat { get; set; }
       

        public SubActivityStatistic()
             :base()
             { }


        public SubActivityStatistic(SubActivity oSubActivity, litePerson person, StatusStatistic Status, DateTime? StartDate, DateTime? EndDate, Int32 idCreatedBy,
             DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress)
            : base(person, Status, StartDate, EndDate, idCreatedBy, CreatedOn, CreatorIpAddress, CreatorProxyIpAddress)
         {
             this.SubActivity = oSubActivity;             
         }
        
    }


    public class OldStat
    {
      
        public virtual Int64 Id { get; protected set; }              

        /// <summary>
        /// Delete status
        /// </summary>      
        /// 
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual litePerson Person { get; set; }
        public virtual StatusStatistic Status { get; set; }
        public virtual DateTime? StartDate { get; set; }
     
        public virtual Int16 Mark { get; set; }
        public virtual Int64 Completion { get; set; }
        public virtual SubActivity SubActivity { get; set; }
    
    }

}
