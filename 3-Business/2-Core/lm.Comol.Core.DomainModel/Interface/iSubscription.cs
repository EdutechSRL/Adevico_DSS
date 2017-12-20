using System;
using lm.Comol.Core.Subscriptions;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
    public interface iSubscription : iDomainObject<int>
	{
		string Name { get; set; }
		iCommunity Community { get; set; }
        iRole Role { get; set; }
        iPerson Person { get; set; }
        DateTime? SubscriptedOn { get; set; }
        DateTime? LastAccessOn { get; set; }
        Boolean Accepted { get; set; }
        Boolean Enabled { get; set; }
        Boolean isResponsabile { get; set; }
        Boolean DisplayMail { get; set; }
        SubscriptionStatus Status { get; }
	}
}