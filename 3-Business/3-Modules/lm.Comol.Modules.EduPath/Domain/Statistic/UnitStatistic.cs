using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
       
    [Serializable()]
    public class UnitStatistic:BaseStatistic
    {
        public virtual Unit Unit { get; set; }
        public virtual Int16 MandatoryPassedActivityCount { get; set; }
        public virtual Int16 MandatoryCompletedActivityCount { get; set; }
        public virtual Int16 MandatoryPassedCompletedActivityCount { get; set; }

        public virtual Int16 MandatoryPassedOnlyActivityCount { get { return (Int16)(MandatoryPassedActivityCount - MandatoryPassedCompletedActivityCount); } }
        public virtual Int16 MandatoryCompletedOnlyActivityCount { get { return (Int16)(MandatoryCompletedActivityCount - MandatoryPassedCompletedActivityCount); } }

        public virtual long IdPath { get; set; }

        /// <summary>
        /// VALIDO SOLO PER STATISTICHE UPDATE MODE
        /// </summary>
     //   public virtual PathStatistic ParentStat { get; set; }

        /// <summary>
        /// VALIDO SOLO PER STATISTICHE UPDATE MODE
        /// </summary>
  //      public virtual IList<ActivityStatistic> ChildrenStats { get; set; } 
         
        public UnitStatistic() { }
             
        public UnitStatistic(Unit oUnit, litePerson person, StatusStatistic Status, DateTime? StartDate, DateTime? EndDate, Int32 idPerson,
             DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress)
            : base(person, Status, StartDate, EndDate, idPerson, CreatedOn, CreatorIpAddress, CreatorProxyIpAddress)
         {
             this.Unit = oUnit;             
         }
    }
}
