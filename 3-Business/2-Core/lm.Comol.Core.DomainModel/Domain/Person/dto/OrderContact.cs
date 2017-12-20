
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public enum OrderContact
	{
		None = 0,
		Name = 1,
		Surname = 2,
		DisplayName = 3,
		Mail = 5,
		OtherField = 6,
		SubscriptionDate = 7,
		LastAccessDate = 8,
		Role = 9
	}
}