using lm.Comol.Core.DomainModel;
using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(), CLSCompliant(true)]
    public class ScormPackageWithVersionToEvaluate : DomainBaseObject<long>
    {
        public virtual long IdItem { get; set; }
        public virtual System.Guid UniqueIdItem { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }

        public virtual Int32 IdPerson { get; set; }
        public virtual long IdLink { get; set; }
        public virtual Boolean IsPlaying { get; set; }
        public virtual Boolean ToUpdate { get; set; }
        public virtual DateTime ModifiedOn { get; set; }
        public ScormPackageWithVersionToEvaluate()
        {
            ToUpdate = false;
            IsPlaying = false;
        }
    }
}