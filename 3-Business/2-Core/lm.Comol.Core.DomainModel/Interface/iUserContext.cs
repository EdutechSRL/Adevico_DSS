using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
	public interface iUserContext
	{
		bool isSameCommunity { get; }
		IList<int> RolesID { get; set; }
		iPerson CurrentUser { get; set; }
		int CurrentUserID { get; }
		int CurrentCommunityID { get; set; }
		int CurrentCommunityOrganizationID { get; set; }
		int UserDefaultOrganizationId { get; set; }
		int WorkingCommunityID { get; set; }
        iLanguage Language { get; set; }
		System.Guid WorkSessionID { get; set; }
		bool isAnonymous { get; set; }
		int UserTypeID { get; set; }
		string IpAddress { get; set; }
		string ProxyIpAddress { get; set; }

	}
}