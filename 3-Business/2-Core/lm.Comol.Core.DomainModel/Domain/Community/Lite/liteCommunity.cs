
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class liteCommunity : lm.Comol.Core.DomainModel.DomainObject<int>
	{
        public virtual string Name { get; set; }
        public virtual Int32  IdType { get; set; }
        public virtual Int32 IdFather { get; set; }
        public virtual Int32 IdOrganization { get; set; }
        public virtual Int32 IdCreatedBy { get; protected set; }
        public virtual Boolean ConfirmSubscription { get; set; }
        
        public liteCommunity()
		{
		}
	}
}