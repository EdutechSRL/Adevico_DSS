using lm.Comol.Core.DomainModel;
using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(), CLSCompliant(true)]
    public class ScormPackageRequestForRecalc : DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual long IdItem { get; set; }
        public virtual System.Guid UniqueIdItem { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }

        public virtual long IdSettings { get; set; }
        public virtual Boolean IsStarted { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual DateTime StartOn { get; set; }
        public virtual DateTime? StartedOn { get; set; }
        public virtual DateTime? CompletedOn { get; set; }
        public ScormPackageRequestForRecalc()
        {

        }
    }
}