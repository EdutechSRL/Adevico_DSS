using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public enum RevisionErrorView
    {
        None = 0,
        Deleted = 1,
        Unknown = 2,
        //SubmissionsClosed = 2,
        GenericError = 3,
        StringValueSaving = 4,
        FileSaving = 5,
        RequiredItems = 6,
        TimeExpired = 7,
        Unavailable = 8,
        InvalidFields = 9,
        DeadlineFieldsNotEditable = 10,
        DeadlineNotEditable  = 11,
        FieldsNotEditable = 12,
        NoFieldsToReview = 13,
        SavingSettings = 14
    }
}
