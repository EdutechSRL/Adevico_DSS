using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoBaseForPaper: dtoBase 
    {
        public virtual NotifyAcceptRefusePolicy AcceptRefusePolicy { get; set; }
        public virtual RevisionMode RevisionSettings { get; set; }
        public virtual String Name { get; set; }
        public virtual String Edition { get; set; }
        public virtual String Summary { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual System.DateTime? EndDate { get; set; }
        public virtual CallForPaperType Type { get; set; }
        public virtual Boolean SubmissionClosed { get; set; }
        public virtual litePerson Owner { get; set; }
        public virtual liteCommunity Community { get; set; }
        public virtual Boolean IsPublic { get; set; }
        public virtual Boolean IsPortal { get; set; }
        public virtual Boolean ForSubscribedUsers { get; set; }
        public virtual CallForPaperStatus Status { get; set; } //published, draft,
        public virtual int OverrideHours { get; set; }
        public virtual int OverrideMinutes { get; set; }
        public virtual Boolean AllowSubmissionExtension { get; set; }
        public virtual long IdDssMethod { get; set; }
        public virtual long IdDssRatingSet { get; set; }
        public virtual Boolean IsDssMethodFuzzy { get; set; }
        public virtual String FuzzyMeWeights { get; set; }
        public virtual Boolean UseManualWeights { get; set; }
        public virtual Boolean OrderedWeights { get; set; }
        public virtual Boolean IsValidFuzzyMeWeights { get; set; }

        public virtual Boolean AttachSign { get; set; }
        public virtual Boolean AllowDraft { get; set; }

        public virtual String Tags { get; set; }

        public virtual bool AdvacedEvaluation { get; set; }

        public dtoBaseForPaper() : base() { 
            OverrideHours = 0;
            OverrideMinutes = 0;
        }
      //  public dtoBaseForPaper(long Id, String name, String edition, String shorDescription, String description, DateTime startDate, System.DateTime? endDate,
      //Boolean submissionClosed, CallForPaperType type, Boolean isPublic, Boolean isPortal, CallForPaperStatus status, Boolean allowSubmissionExtension)
      //      : base(Id)
      //  {
      //      Name = name;
      //      Edition = edition;
      //      ShortDescription = description;
      //      Description = description;
      //      StartDate = startDate;
      //      EndDate = endDate;
      //      SubmissionClosed = submissionClosed;
      //      Type = type;
      //      IsPublic = isPublic;
      //      IsPortal = isPortal;
      //      Status = status;
      //      AllowSubmissionExtension = allowSubmissionExtension;
      //  }
      //  public dtoBaseForPaper(long Id, String name, String edition, String shorDescription, String description, DateTime startDate, System.DateTime? endDate,
      //  Boolean submissionClosed, CallForPaperType type, Boolean isPublic, Boolean isPortal, CallForPaperStatus status, BaseStatusDeleted deleted, Boolean allowSubmissionExtension)
      //      : this(Id, name, shorDescription, description, startDate, endDate, submissionClosed, type, isPublic, isPortal, status, allowSubmissionExtension)
      //  {
      //      Deleted = deleted;
      //  }

      //  public dtoBaseForPaper(long Id,String name, String edition, String shorDescription, String description, DateTime startDate, System.DateTime? endDate,
      //      Boolean submissionClosed, CallForPaperType type, Community community, Boolean isPublic, Boolean isPortal, CallForPaperStatus status, Boolean allowSubmissionExtension)
      //      : this(Id, name, shorDescription, description, startDate, endDate, submissionClosed, type, isPublic, isPortal, status, allowSubmissionExtension)
      //  {
      //      Community = community;
      //  }

      //  public dtoBaseForPaper(BaseForPaper call)
      //        : this(call.Id, call.Name, call.Edition, call.ShortDescription )
      //    {
      //        Name =;
      //        Edition =;
      //        Description = call.Edition;
      //        StartDate = call.StartDate;
      //        EndDate = call.EndDate;
      //        SubmissionClosed = call.SubmissionClosed;
      //        Type = call.Type;
      //        IsPublic = call.IsPublic;
      //        Status = call.Status;
      //        Deleted = call.Deleted;
      //        AllowSubmissionExtension = call.AllowSubmissionExtension;
      //        Community = call.Community;
      //        Owner = call.CreatedBy;
      //        IsPortal = call.IsPortal;
      //        IsPublic = call.IsPublic;
      //    }
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
                return (initDate <= extensionDate && AllowSubmissionExtension);
        }

        public lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier GetRepositoryIdentifier()
        {
            if (IsPortal)
                return lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(Core.FileRepository.Domain.RepositoryType.Portal, 0);
            else if (Community !=null)
                return lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(Core.FileRepository.Domain.RepositoryType.Community, Community.Id);
            else
                return lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(Core.FileRepository.Domain.RepositoryType.Community, 0);
        }
    }
}