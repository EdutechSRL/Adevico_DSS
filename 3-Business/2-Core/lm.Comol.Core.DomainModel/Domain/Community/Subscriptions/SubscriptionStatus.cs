
namespace lm.Comol.Core.DomainModel
{
	public enum SubscriptionStatus
	{
		none = 0,
		newUser = 1,
		waiting = 2,
		blocked = 4,
		responsible = 8,
		activemember = 16,
		all = 32,
		communityblocked = 64,
		notFederated = 128
	}
}