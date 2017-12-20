using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    public enum SubmissionTranslations
    {
        None = -1,
        StatusNone = 0,
        StatusDraft = 1,
        StatusSubmitted = 2,
        StatusAccepted = 3,
        StatusRejected = 4,
        StatusWaitingValuation = 5,
        StatusValuating = 6,
        StatusValuated = 7,
        CallTitle = 8,
        PrintInfo = 9,
        SubmittedByTitle = 10,
        SubmittedByInfo = 11,
        SubmittedForInfo = 12,
        SubmittedTypeTitle = 13,
        SubmissionStatusTitle = 14,
        SubmissionStatus = 15,
        EmptyItem=16,
        SubmittedFilesTitle = 17,
        SubmissionInfo = 18,

        CommunityNameTitle = 20,
        PortalCommunityName = 21,
        CreatedByInfo = 22,
        CallTitleAndEdition = 23,
        FileSubmitted = 24,
        FileNotSubmitted = 25,
        ItemMandatory = 26,
        MaxCharInfo = 27,
        DisclaimerAccept = 28,
        DisclaimerReject = 29,
        FileCreationError = 30,
        Pager = 31,
        FilesSubmitted = 32,
        SubmissionCallFileName = 33,
        SubmissionRequestFileName = 34,
        AnonymousUser = 35,
        MinOption = 36,
        MaxOption = 37,
        MinOptionMaxOption = 38,
        RequestTitle = 39,
        SubmitToCallFileName = 40,
        SubmitToRequestFileName = 41,
        DisclaimerRead = 42,
        OtherOption = 43,
        OtherOptionValue = 44,
        CellMultipleFieldName = 45,
        CellMultipleFieldValues = 46,
        CellMultipleFieldFreeText = 47,


        StatusWaitForSign = 48,
    }
}
