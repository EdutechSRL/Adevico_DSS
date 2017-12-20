
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public enum CommunityRelationType
	{
		FatherOf = 1,
		AdministeredBy = 2,
		SubscribeFrom = 3,
		ConfirmSubscribeFrom = 4,
		NotSubscribeFrom = 5,
		Parent = 6,
		SonOf = 7
	}
}