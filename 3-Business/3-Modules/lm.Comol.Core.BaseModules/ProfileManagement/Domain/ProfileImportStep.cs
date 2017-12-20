using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum ProfileImportStep
    {
        None = 0,
        SelectSource = 1,
        SourceCSV = 2,
        SourceRequestForMembership = 4,
        SourceCallForPapers = 8,
        FieldsMatcher = 16,
        ItemsSelctor = 32,
        SelectOrganizations = 64,
        SelectCommunities = 128,
        SubscriptionsSettings = 256,
        MailTemplate = 512,
        Summary = 1024,
        Errors = 2048,
        ImportCompleted = 4096,
        ImportWithErrors = 8192
    }
}