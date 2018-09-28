using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class Path:DomainBaseMetaInfoStatus , IEduPathItem
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual bool isDefault { get; set; }
        public virtual liteCommunity  Community{ get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? EndDateOverflow { get; set; }
        public virtual Int64 MinCompletion { get; set; }
        public virtual Int16 MinMark { get; set; }
        public virtual IList<Unit> UnitList { get; set; }
        public virtual Int16 DisplayOrder { get; set; }
        public virtual Int64 Duration { get; set; }
        public virtual EPType EPType { get; set; }
        public virtual Int64 Weight { get; set; }
        public virtual Int64 WeightAuto { get; set; }
        public virtual Boolean IsMooc { get; set; }
        
        public virtual PolicySettings Policy { get; set; }

        public virtual string PathCode { get; set; }
        
        // +EP[Rob]
        public virtual Boolean FloatingDeadlines { get; set; }
        public virtual Boolean SingleAction { get; set; } 
        public virtual TimeSpan? StartSpan { get; set; }
        public virtual TimeSpan? EndSpan { get; set; }

        //Mooc
        public virtual MoocType MoocType { get; set; }

        public Path()
        {
            Id = 0;
            Name = "";
            Description = "";
            isDefault=false;
            MinMark = 0;
            MinCompletion = 0;
            UnitList = new List<Unit>();
            DisplayOrder = 0;
            Duration = 0;
            Weight = 0;
            Status = Domain.Status.Locked;
            SingleAction = true;
            StartSpan = new TimeSpan(0);
            EndSpan = new TimeSpan(0);
            Policy = new PolicySettings();
            IsMooc = false;
        }
    }
}