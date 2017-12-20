
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class liteCommunityInfo : lm.Comol.Core.DomainModel.DomainObject<int>
	{
        public virtual int IdOrganization { get; set; }
        public virtual int IdFather { get; protected set; }
        public virtual int IdCreatedBy { get; protected set; }
        public virtual int IdTypeOfCommunity { get; protected set; }
        public virtual string Name { get; set; }
        public virtual Boolean isArchived { get; set; }
        public virtual Boolean isClosedByAdministrator { get; set; }
        public virtual int Level { get; set; }
        public virtual Boolean AllowPublicAccess { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? ClosedOn { get; set; }
        public virtual DateTime? SubscriptionStartOn { get; set; }
        public virtual DateTime? SubscriptionEndOn { get; set; }
        public virtual Int32 MaxUsersWithDefaultRole { get; set; }
        public virtual Int32 MaxOverDefaultSubscriptionsAllowed { get; set; }
        public virtual Boolean AllowUnsubscribe { get; set; }
        public virtual Boolean AllowSubscription { get; set; }
        public virtual Boolean ConfirmSubscription { get; set; }
         public virtual Boolean AlwaysAllowAccessToCopyCenter { get; set; }

		public liteCommunityInfo()
		{
		}
	}
}