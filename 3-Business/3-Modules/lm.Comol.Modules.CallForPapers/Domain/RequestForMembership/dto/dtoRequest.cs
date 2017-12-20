using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    public class dtoRequest : dtoBaseForPaper
    {
        public virtual String StartMessage { get; set; }
        public virtual String EndMessage { get; set; }

        public dtoRequest()
            : base()
        {
            OverrideHours = 0;
            OverrideMinutes = 0;
            Type = CallForPaperType.RequestForMembership;
        }
        //public dtoRequest(long Id, String name, String edition, String description, DateTime startDate, System.DateTime? endDate, 
        //  Boolean submissionClosed, CallForPaperType type,  Community community, Boolean isPublic, Boolean isPortal, CallForPaperStatus status,  Boolean allowSubmissionExtension, string notificationMail)
        //    : base(Id)
        //{
        //    Name = name;
        //    Edition = edition;
        //    Description = description;
        //    StartDate = startDate;
        //    EndDate = endDate;
        //    //AwardDate = awardDate;
        //    SubmissionClosed = submissionClosed;
        //    Type = type;
        //    //DisplayWinner = displayWinner;
        //    Community = community;
        //    IsPublic = isPublic;
        //    IsPortal = isPortal;
        //    Status = status;
        //    AllowSubmissionExtension = allowSubmissionExtension;
        //    NotificationEmail = notificationMail;
        //    //EvaluationType = evaluationType;
        //}

        //public dtoRequest(long Id, String name, String edition, String description, DateTime startDate, System.DateTime? endDate,
        //Boolean submissionClosed, CallForPaperType type, Boolean isPublic,Boolean isPortal, CallForPaperStatus status, BaseStatusDeleted deleted, Boolean allowSubmissionExtension, string notificationMail)
        //    : base(Id)
        //{
        //    Name = name;
        //    Edition = edition;
        //    Description = description;
        //    StartDate = startDate;
        //    EndDate = endDate;
        //    SubmissionClosed = submissionClosed;
        //    Type = type;
        //    IsPublic = isPublic;
        //    IsPortal = isPortal;
        //    Status = status;
        //    Deleted = deleted;
        //    AllowSubmissionExtension = allowSubmissionExtension;
        //    NotificationEmail = notificationMail;
        //    //-----CFP-------
        //    //AwardDate = awardDate;
        //    //DisplayWinner = displayWinner;
        //    //EvaluationType = evaluationType;
        //    //-----RFM-------
        //    //StartMessage
        //    //EndMessage
        //    //isPortal
        //    //SubscriberMailtext
        //}

        //public dtoRequest(RequestForMembership call)
        //    : base(call.Id)
        //{
        //    Name = call.Name;
        //    Edition = call.Edition;
        //    Description = call.Edition;
        //    StartDate = call.StartDate;
        //    EndDate = call.EndDate;
        //    SubmissionClosed = call.SubmissionClosed;
        //    Type = call.Type;
        //    IsPublic = call.IsPublic;
        //    IsPortal = call.IsPortal ;
        //    Status = call.Status;
        //    Deleted = call.Deleted;
        //    AllowSubmissionExtension = call.AllowSubmissionExtension;
        //    NotificationEmail = call.NotificationEmail;
        //    //EndEvaluationOn = call.EndEvaluationOn;
        //    //EvaluationType = call.EvaluationType;
        //    //AwardDate = call.AwardDate;
        //    //DisplayWinner = call.DisplayWinner;
        //}
    }
}