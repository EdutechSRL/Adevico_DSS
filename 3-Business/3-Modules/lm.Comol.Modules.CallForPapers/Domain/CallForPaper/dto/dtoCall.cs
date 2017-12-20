using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoCall: dtoBaseForPaper 
    {
        public virtual String AwardDate { get; set; }
        public virtual bool DisplayWinner { get; set; }
        
        public virtual System.DateTime? EndEvaluationOn { get; set; }
        public virtual EvaluationType EvaluationType { get; set; }
        

        public dtoCall()
        { 
            OverrideHours = 0;
            OverrideMinutes = 0;
            Deleted = BaseStatusDeleted.None;
            Type = CallForPaperType.CallForBids;
            AdvacedEvaluation = false;
        }
        //public dtoCall(long Id,String name, String edition, String description, DateTime startDate, System.DateTime? endDate, String awardDate,
        //    Boolean submissionClosed, CallForPaperType type, bool displayWinner, Community community, Boolean isPublic, Boolean isPortal, CallForPaperStatus status, EvaluationType evaluationType, Boolean allowSubmissionExtension, string notificationMail)
        //    : base(Id, name, edition, description,)
        //{
        //    Name = name;
        //    Edition = edition;
        //    Description = description;
        //    StartDate = startDate;
        //    EndDate = endDate;
        //    AwardDate = awardDate;
        //    SubmissionClosed = submissionClosed;
        //    Type = type;
        //    DisplayWinner = displayWinner;
        //    Community = community;
        //    IsPublic = isPublic;
        //    Status = status;
        //    AllowSubmissionExtension = allowSubmissionExtension;
        //    NotificationEmail = notificationMail;
        //    EvaluationType = evaluationType;
        //}

        //  public dtoCall(long Id, String name, String edition, String description, DateTime startDate, System.DateTime? endDate, String awardDate,
        //  Boolean submissionClosed, CallForPaperType type, bool displayWinner, Boolean isPublic, CallForPaperStatus status, EvaluationType evaluationType, BaseStatusDeleted deleted, Boolean allowSubmissionExtension, string notificationMail)
        //      : base(Id)
        //  {
        //      Name = name;
        //      Edition = edition;
        //      Description = description;
        //      StartDate = startDate;
        //      EndDate = endDate;
        //      AwardDate = awardDate;
        //      SubmissionClosed = submissionClosed;
        //      Type = type;
        //      DisplayWinner = displayWinner;
        //      IsPublic = isPublic;
        //      Status = status;
        //      Deleted = deleted;
        //      AllowSubmissionExtension = allowSubmissionExtension;
        //      NotificationEmail = notificationMail;
        //      EvaluationType = evaluationType;
        //  }

        //  public dtoCall(CallForPaper call)
        //      : base(call.Id )
        //  {
        //      Name = call.Name;
        //      Edition = call.Edition;
        //      Description = call.Edition;
        //      StartDate = call.StartDate;
        //      EndDate = call.EndDate;
        //      AwardDate = call.AwardDate;
        //      SubmissionClosed = call.SubmissionClosed;
        //      Type = call.Type;
        //      DisplayWinner = call.DisplayWinner;
        //      IsPublic = call.IsPublic;
        //      Status = call.Status;
        //      Deleted = call.Deleted;
        //      AllowSubmissionExtension = call.AllowSubmissionExtension;
        //      NotificationEmail = call.NotificationEmail;
        //      EndEvaluationOn = call.EndEvaluationOn;
        //      EvaluationType = call.EvaluationType;
        //  }
    }
}