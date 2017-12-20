using System;
using System.Runtime.Serialization;

namespace lm.Comol.Core.DomainModel
{
    [CLSCompliant(true), Serializable(), DataContract]
    public class dtoItemEvaluation<T> : dtoEvaluation 
    {
        [DataMember]
        public virtual T Item { get; set; }
        //[DataMember]
        //public virtual short Completion { get; set; }
        //[DataMember]
        //public virtual Boolean isCompleted { get; set; }
        //[DataMember]
        //public virtual Boolean isStarted { get; set; }
        //[DataMember]
        //public virtual Boolean isPassed { get; set; }

        public dtoItemEvaluation() : base(){
        }
    }
}