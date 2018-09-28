using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
     [Serializable()]
    public class ActivityStatistic:BaseStatistic
    {
         public virtual Activity Activity { get; set; }
         public virtual Int16 MandatoryPassedSubActivityCount { get; set; }

         public virtual Int16 MandatoryCompletedSubActivityCount { get; set; }
         public virtual Int16 MandatoryPassedCompletedSubActivityCount { get; set; }

         public virtual Int16 MandatoryPassedOnlySubActivityCount { get { return (Int16)(MandatoryPassedSubActivityCount - MandatoryPassedCompletedSubActivityCount); } }
         public virtual Int16 MandatoryCompletedOnlySubActivityCount { get { return (Int16)(MandatoryCompletedSubActivityCount - MandatoryPassedCompletedSubActivityCount); } }
        
         /// <summary>
         /// VALIDO SOLO PER STATISTICHE UPDATE MODE
         /// </summary>
    //     public virtual UnitStatistic ParentStat { get; set; }
         
         /// <summary>
         /// VALIDO SOLO PER STATISTICHE UPDATE MODE
         /// </summary>
   //      public virtual IList<SubActivityStatistic> ChildrenStats { get; set; }

         public virtual long IdUnit { get; set; }
         public virtual long IdPath { get; set; }

         
         
         public ActivityStatistic() { }

         public ActivityStatistic(Activity oActivity, litePerson person, StatusStatistic Status, DateTime? StartDate, DateTime? EndDate, Int32 idCreatedBy,
             DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress)
             : base(person, Status, StartDate, EndDate, idCreatedBy, CreatedOn, CreatorIpAddress, CreatorProxyIpAddress)
         {
             this.Activity = oActivity;             
         }
                     
     }
}
