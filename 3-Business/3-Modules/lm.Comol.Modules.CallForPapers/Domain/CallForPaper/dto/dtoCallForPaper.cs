using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoCallForPaper: dtoBase 
    {
        public virtual String Name { get; set; }
        public virtual String Edition { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual System.DateTime? EndDate { get; set; }
        public virtual String AwardDate { get; set; }
        public virtual Boolean SubmissionClosed { get; set; }
        public virtual CallForPaperType Type { get; set; }
        public virtual bool DisplayWinner { get; set; }
        public virtual liteCommunity Community { get; set; }
        public virtual Boolean IsPublic { get; set; }
        public virtual CallForPaperStatus Status { get; set; }
        public virtual int OverrideHours { get; set; }
        public virtual int OverrideMinutes { get; set; }
        public virtual litePerson Owner { get; set; }
        public virtual Boolean AllowSubmissionExtension { get; set; }
        public virtual String NotificationEmail { get; set; }
        public virtual System.DateTime? EndEvaluationOn { get; set; }
        public virtual EvaluationType EvaluationType { get; set; }
        public dtoCallForPaper() : base() { 
            OverrideHours = 0;
            OverrideMinutes = 0;
        }
          public dtoCallForPaper(long Id,String name, String edition, String description, DateTime startDate, System.DateTime? endDate, String awardDate,
            Boolean submissionClosed, CallForPaperType type, bool displayWinner, liteCommunity community, Boolean isPublic, CallForPaperStatus status, EvaluationType evaluationType, Boolean allowSubmissionExtension, string notificationMail)
            : base(Id)
        {
            Name = name;
            Edition = edition;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            AwardDate = awardDate;
            SubmissionClosed = submissionClosed;
            Type = type;
            DisplayWinner = displayWinner;
            Community = community;
            IsPublic = isPublic;
            Status = status;
            AllowSubmissionExtension = allowSubmissionExtension;
            NotificationEmail = notificationMail;
            EvaluationType = evaluationType;
        }

          public dtoCallForPaper(long Id, String name, String edition, String description, DateTime startDate, System.DateTime? endDate, String awardDate,
          Boolean submissionClosed, CallForPaperType type, bool displayWinner, Boolean isPublic, CallForPaperStatus status, EvaluationType evaluationType, BaseStatusDeleted deleted, Boolean allowSubmissionExtension, string notificationMail)
              : base(Id)
          {
              Name = name;
              Edition = edition;
              Description = description;
              StartDate = startDate;
              EndDate = endDate;
              AwardDate = awardDate;
              SubmissionClosed = submissionClosed;
              Type = type;
              DisplayWinner = displayWinner;
              IsPublic = isPublic;
              Status = status;
              Deleted = deleted;
              AllowSubmissionExtension = allowSubmissionExtension;
              NotificationEmail = notificationMail;
              EvaluationType = evaluationType;
          }

          public dtoCallForPaper(CallForPaper call)
              : base(call.Id )
          {
              Name = call.Name;
              Edition = call.Edition;
              Description = call.Edition;
              StartDate = call.StartDate;
              EndDate = call.EndDate;
              AwardDate = call.AwardDate;
              SubmissionClosed = call.SubmissionClosed;
              Type = call.Type;
              DisplayWinner = call.DisplayWinner;
              IsPublic = call.IsPublic;
              Status = call.Status;
              Deleted = call.Deleted;
              AllowSubmissionExtension = call.UseStartCompilationDate;
              NotificationEmail = call.NotificationEmail;
              EndEvaluationOn = call.EndEvaluationOn;
              EvaluationType = call.EvaluationType;
          }
          public Boolean AllowSubmission(DateTime initDate)
        {
            if (!EndDate.HasValue)
                return true;
            else { 
                DateTime expectedDate = EndDate.Value.AddHours(OverrideHours).AddMinutes(OverrideMinutes);
                return initDate <= expectedDate;
            }
        }
        public Boolean AllowLateSubmission(DateTime initDate, DateTime extensionDate)
        {
            if (!EndDate.HasValue)
                return true;
            else
            {
             //   DateTime expectedDate = EndDate.Value.AddHours(OverrideHours).AddMinutes(OverrideMinutes);
                return (initDate <= extensionDate && AllowSubmissionExtension);
            }
        }


    }
}
