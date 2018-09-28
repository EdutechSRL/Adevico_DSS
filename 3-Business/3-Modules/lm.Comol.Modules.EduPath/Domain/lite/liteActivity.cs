using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class liteActivity : DomainBaseObject<long>
    {
        public virtual long IdUnit { get; set; }
        public virtual long IdPath { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Int16 DisplayOrder { get; set; }
        
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        public virtual Int64 MinCompletion { get; set; }
        public virtual Int16 MinMark { get; set; }
        public virtual Int64 Duration { get; set; }
        public virtual Boolean isQuiz { get; set; }
        public virtual Status Status { get; set; }
        private Int64 _weight;
        public virtual Int64 Weight
        { 
            get{return _weight;}
            set { _weight = value; }
        }

        //+EP[Rob]
        public virtual TimeSpan? StartSpan { get; set; }
        public virtual TimeSpan? EndSpan { get; set; }


        public liteActivity()
        {
            Name = "";
            Description = "";
            isQuiz = false;
        }
    }
}