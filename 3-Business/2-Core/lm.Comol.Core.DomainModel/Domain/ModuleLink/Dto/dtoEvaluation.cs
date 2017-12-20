using System;
using System.Runtime.Serialization;

namespace lm.Comol.Core.DomainModel
{
    [CLSCompliant(true), Serializable(), DataContract]
    public class dtoEvaluation
    {
        [DataMember]
        public virtual long IdLink { get; set; }
        [DataMember]
        public virtual short Completion { get; set; }
        [DataMember]
        public virtual Boolean isCompleted { get; set; }
        [DataMember]
        public virtual Boolean isStarted { get; set; }
        [DataMember]
        public virtual Boolean isPassed { get; set; }
        [DataMember]
        public virtual Int16 Mark { get; set; }
        [DataMember]
        public virtual bool AlreadyCompleted { get; set; }
        public dtoEvaluation()
        {
            isStarted = false;
            isPassed = false;
            isCompleted = false;
            Completion = 0;
            Mark = 0;
            AlreadyCompleted = false;
        }
    }
}