using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class PathStatistic : BaseStatistic
    {
        public virtual Path Path { get; set; }
        public virtual Int16 MandatoryPassedUnitCount { get; set; }
        public virtual Int16 MandatoryCompletedUnitCount { get; set; }
        public virtual Int16 MandatoryPassedCompletedUnitCount { get; set; }

        public virtual Int16 MandatoryPassedOnlyUnitCount { get { return (Int16)(MandatoryPassedUnitCount - MandatoryPassedCompletedUnitCount); } }
        public virtual Int16 MandatoryCompletedOnlyUnitCount { get { return (Int16)(MandatoryCompletedUnitCount - MandatoryPassedCompletedUnitCount); } }
     //   public virtual IList<UnitStatistic> ChildrenStats { get; set; } 

        public PathStatistic() { }

        public PathStatistic(Path oPath, litePerson person, StatusStatistic Status, DateTime? StartDate, DateTime? EndDate, Int32 idCreatedBy,
            DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress)
            : base(person, Status, StartDate, EndDate, idCreatedBy, CreatedOn, CreatorIpAddress, CreatorProxyIpAddress)
        {
            this.Path = oPath;
        }

    

    }

   
}
