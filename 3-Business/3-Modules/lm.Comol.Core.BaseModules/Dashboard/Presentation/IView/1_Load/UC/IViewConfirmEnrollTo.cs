using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.BaseModules.Dashboard.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewConfirmEnrollTo : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Guid UniqueIdentifier { get; set; }
        Boolean isInitialized { get; set; }
        Boolean DisplayDescription { get; set; }
        Boolean AllowEnroll { get; set; }
        List<dtoCommunityInfoForEnroll> CurrentItems { get; set; }
        void InitializeControl(dtoCommunityInfoForEnroll item, String description = "");
        void InitializeControl(List<dtoEnrollment> enrolledItems, List<dtoEnrollment> notEnrolledItems, List<dtoCommunityInfoForEnroll> enrollingCommunities, String description = "");
        void ClearSession();
    }
}