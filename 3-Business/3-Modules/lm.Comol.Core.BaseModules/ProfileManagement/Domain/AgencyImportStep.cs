using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum AgencyImportStep
    {
        None = 0,
        SourceCSV = 1,
        FieldsMatcher = 2,
        ItemsSelctor = 4,
        OrganizationAvailability = 8,
        Summary = 16,
        Errors = 32,
        ImportCompleted = 64,
        ImportWithErrors = 128
    }
}