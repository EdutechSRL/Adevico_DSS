using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class Unit : DomainBaseMetaInfoStatus, IEduPathItem//,IRuleElement
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Int16 DisplayOrder { get; set; }
        public virtual Path ParentPath { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }  
        public virtual IList<Activity> ActivityList { get; set; }
        public virtual Int64 MinCompletion { get; set; }
        public virtual Int16 MinMark { get; set; }
        public virtual Int64 Duration { get; set; }
        public virtual liteCommunity Community { get; set; }
        public virtual Int64 Weight { get; set; }

        //+EP[Rob]
        public virtual TimeSpan? StartSpan { get; set; }
        public virtual TimeSpan? EndSpan { get; set; }

        public Unit()
        {
            Id = 0;   
            MinCompletion = 0;
            MinMark = 0;
            Name = "";
            Description = "";
            ActivityList = new List<Activity>();
            DisplayOrder = 0;
            Duration = 0;
            Weight = 1;
        }
    } 
}
