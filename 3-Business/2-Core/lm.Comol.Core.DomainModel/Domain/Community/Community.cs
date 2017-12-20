
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class Community : lm.Comol.Core.DomainModel.DomainObject<int>, iCommunity
	{

        public virtual int IdOrganization { get; set; }
        public virtual int IdFather { get; protected set; }
        public virtual int IdTypeOfCommunity { get; protected set; }
        public virtual iPerson Creator { get; set; }
        public virtual iCommunity Father { get; set; }
        public virtual string Name { get; set; }
        public virtual CommunityType TypeOfCommunity { get; set; }
        public virtual Boolean isArchived { get; set; }
        public virtual Boolean isClosedByAdministrator { get; set; }
        public virtual int Level { get; set; }
        public virtual Boolean AllowPublicAccess { get; set; }
        
		public Community()
		{
		}

		public Community(int iCommunityId)
		{
			base.Id = iCommunityId;
		}
	}
}