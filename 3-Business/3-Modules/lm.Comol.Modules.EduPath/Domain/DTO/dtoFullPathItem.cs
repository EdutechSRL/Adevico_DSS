using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoFullPathItem : dtoGenericItem, IComparable, IEquatable<dtoFullPathItem>   
   {
       public virtual bool isDefault { get; set; }
       public virtual DateTime? StartDate { get; set; }
       public virtual DateTime? EndDate { get; set; }
       public virtual DateTime? EndDateOverflow { get; set; }
       public virtual Int64 MinCompletion { get; set; }
       public virtual Int16 MinMark { get; set; }
       public virtual Int16 DisplayOrder { get; set; }
       public virtual Int64 Duration { get; set; }
       public virtual EPType EPType { get; set; }
       public virtual Boolean IsMooc { get; set; }
       public virtual Int64 WeightAuto { get; set; }

       // +EP[Rob]
       public virtual Boolean FloatingDeadlines { get; set; }
       public virtual Boolean SingleAction { get; set; }

       public virtual TimeSpan? StartSpan { get; set; }
       public virtual TimeSpan? EndSpan { get; set; }

       // Moocs status
       public virtual MoocType MoocType { get; set; }

        public dtoFullPathItem()
            :base()
        {            
           
        }
        public dtoFullPathItem(Path path, Status personalStatus, RoleEP role)
            : base(path, personalStatus,role)
        {
            StartDate = path.StartDate;
            EndDate = path.EndDate;
            EndDateOverflow = path.EndDateOverflow;
            MinCompletion = path.MinCompletion;
            MinMark = path.MinMark;
            DisplayOrder = path.DisplayOrder;
            isDefault = path.isDefault;
            Duration = path.Duration;
            EPType = path.EPType;
            Weight = path.Weight;
            WeightAuto = path.WeightAuto;
            Name = path.Name;
            IsMooc = path.IsMooc;
            MoocType = path.MoocType;
        }

        public int CompareTo(object obj)
        {
            dtoFullPathItem Obj = (dtoFullPathItem)obj;

            return this.Id.CompareTo(Obj.Id);
        }

        public bool Equals(dtoFullPathItem other)
        {
            return this.Id.Equals(other.Id);
        }
   }
}
