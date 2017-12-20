using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class CallForPaper : BaseForPaper
    {
        public virtual String AwardDate { get; set; }
        public virtual Boolean DisplayWinner { get; set; }
        public virtual Boolean OneCommitteeMembership { get; set; }
        public virtual System.DateTime? EndEvaluationOn { get; set; }
        public virtual EvaluationType EvaluationType { get; set; }

        

        public CallForPaper() {
            IsPublic = false;
            DisplayWinner = false;
            OneCommitteeMembership = true;
            Description = "";
            Name = "";
            Edition = "";
            Sections = new List<FieldsSection>();
            SubmissionClosed = false;
            Type = CallForPaperType.CallForBids;
            DisplayWinner = false;
            SubmittersType = new List<SubmitterType>();
            Status = CallForPaperStatus.Published;
            Attachments = new List<AttachmentFile>();
            Deleted = BaseStatusDeleted.None;
            OverrideHours = 0;
            OverrideMinutes = 0;
            EvaluationType = Domain.EvaluationType.Average;
            AdvacedEvaluation = false;
        }

        //private Boolean AllowSubmission(DateTime initDate)
        //{
        //    if (!EndDate.HasValue)
        //        return true;
        //    else { 
        //        DateTime expectedDate = EndDate.Value.AddHours(OverrideHours).AddMinutes(OverrideMinutes);
        //        return initDate <= expectedDate;
        //    }
        //}
        //public virtual Boolean AllowSubmission()
        //{
        //    return AllowSubmission(DateTime.Now) && Status== CallForPaperStatus.SubmissionOpened;
        //}
        //public virtual Boolean AllowLateSubmission(DateTime initDate, DateTime? ExtensionDate)
        //{
        //    if (!EndDate.HasValue)
        //        return Status == CallForPaperStatus.SubmissionOpened;
        //    else if (!AllowSubmissionExtension)
        //        return AllowSubmission(initDate);
        //    else
        //    {
        //        DateTime expectedDate;
        //        if (ExtensionDate.HasValue)
        //        {
        //            expectedDate = ExtensionDate.Value;
        //            return (Status == CallForPaperStatus.SubmissionOpened) && (initDate <= expectedDate);
        //        }
        //        else{
        //            expectedDate = EndDate.Value.AddHours(OverrideHours).AddMinutes(OverrideMinutes);
        //            return (initDate <= expectedDate);
        //        }
        //    }
        //}
        //public virtual Boolean AllowLateSubmission(DateTime initDate, UserSubmission submission)
        //{
        //    if (!EndDate.HasValue)
        //        return true; 
        //    else if (!AllowSubmissionExtension) 
        //        return AllowSubmission(initDate);
        //    else{
        //        DateTime expectedDate;
        //        if (submission.ExtensionDate.HasValue)
        //            expectedDate = submission.ExtensionDate.Value;
        //        else
        //            expectedDate = EndDate.Value.AddHours(OverrideHours).AddMinutes(OverrideMinutes);
        //        return (initDate <= expectedDate);
        //    }
        //}
    }
}
