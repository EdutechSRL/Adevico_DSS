using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class litePath : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Boolean isDefault { get; set; }
        public virtual Int32 IdCommunity{ get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? EndDateOverflow { get; set; }
        public virtual Int64 MinCompletion { get; set; }
        public virtual Int16 MinMark { get; set; }
        public virtual Int16 DisplayOrder { get; set; }
        public virtual Int64 Duration { get; set; }
        public virtual Status Status { get; set; }
        public virtual EPType EPType { get; set; }
        public virtual Int64 Weight { get; set; }
        public virtual Int64 WeightAuto { get; set; }
        public virtual PolicySettings Policy { get; set; }
        
        // +EP[Rob]
        public virtual Boolean FloatingDeadlines { get; set; }
        public virtual Boolean SingleAction { get; set; } 
        public virtual TimeSpan? StartSpan { get; set; }
        public virtual TimeSpan? EndSpan { get; set; }

        public litePath()
        {
            Policy = new PolicySettings();
        }
    }
}