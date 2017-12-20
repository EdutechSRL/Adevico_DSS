using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum WizardCallStep
    {
        none = -1,
        ImportUsersFrom = 0,
        GeneralSettings = 1,
        RequestMessages = 2,
        CallAvailability = 3,
        SubmittersType = 4,
        Attachments = 5,
        SubmissionEditor = 6,
        FileToSubmit = 7,
        FieldsAssociation = 8,
        SubmitterTemplateMail = 9,
        NotificationTemplateMail = 10
    }
}
