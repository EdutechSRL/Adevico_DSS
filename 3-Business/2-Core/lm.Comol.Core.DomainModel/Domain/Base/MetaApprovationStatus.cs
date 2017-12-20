using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public enum MetaApprovationStatus
	{
		NotDefined = 0,
		Approved = 1,
		NotApproved = 2,
		Waiting = 3,
		Ignore = 4
	}
}