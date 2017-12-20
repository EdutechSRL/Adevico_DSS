
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class ExternalCommunityInfo : DomainBaseObjectMetaInfo<long>
	{
        public virtual int IdCommunity { get; set; }
        public virtual long IdExternalLong { get; set; }
        public virtual String IdExternalString { get; set; }
        public virtual String ExternalIdentifier { get; set; }
        public virtual Boolean AllowInternalEdit { get; set; }
        public virtual long IdDataProvider { get; set; }
	}
}