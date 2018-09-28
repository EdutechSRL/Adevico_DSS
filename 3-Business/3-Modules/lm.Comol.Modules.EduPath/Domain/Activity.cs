using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class Activity : DomainBaseMetaInfoStatus, IEduPathItem//, IRuleElement 
    {

        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Int16 DisplayOrder { get; set; }
        public virtual Unit ParentUnit { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        public virtual Int64 MinCompletion { get; set; }
        public virtual Int16 MinMark { get; set; }
        public virtual IList<SubActivity> SubActivityList { get; set; }
        public virtual Int64 Duration { get; set; }
        public virtual Boolean isQuiz { get; set; }
        public virtual liteCommunity Community { get; set; }
        public virtual Path Path{ get; set; }
        private Int64 _weight;
        public virtual Int64 Weight
        { 
            get{return _weight;}
            set { _weight = value; }
        }

        //+EP[Rob]
        public virtual TimeSpan? StartSpan { get; set; }
        public virtual TimeSpan? EndSpan { get; set; }


        public Activity()
        {
            //Id = 0;
            Name = "";
            Description = "";
            //MinCompletion = 0;
           // MinMark = 0;
            SubActivityList = new List<SubActivity>();
            //DisplayOrder = 0;
            //Duration = 0;
            isQuiz = false;
            //Weight = 1;
        }
    }


}
