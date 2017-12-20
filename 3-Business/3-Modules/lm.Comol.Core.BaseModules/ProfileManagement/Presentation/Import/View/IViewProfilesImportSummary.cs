using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.Mail;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewProfilesImportSummary : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isInitialized { get; set; }
        Boolean isCompleted { get; set; }
        void InitializeControl(String authenticationType, String profileType, Boolean sendMailToUsers, String primaryOrganizationName, Dictionary<Int32, String> otherOrganizationsToSubscribe,List<dtoNewProfileSubscription> subscriptions, Int32 profileToCreate);
        void DisplaySessionTimeout();
    }
}