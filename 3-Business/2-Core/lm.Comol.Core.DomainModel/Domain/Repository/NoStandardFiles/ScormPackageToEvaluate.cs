using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class ScormPackageToEvaluate : DomainBaseObject<long>
	{
        public virtual System.Guid FileUniqueID { get; set; }
        public virtual int IdPerson { get; set; }

        public virtual long IdLink { get; set; }
        public virtual long IdFile { get; set; }
        public virtual bool IsPlaying { get; set; }
        public virtual bool ToUpdate { get; set; }
        public virtual DateTime ModifiedOn { get; set; }
        public virtual Boolean? Transferred { get; set; }
        public ScormPackageToEvaluate()
		{
			ToUpdate = false;
			IsPlaying = false;
		}
	}
}