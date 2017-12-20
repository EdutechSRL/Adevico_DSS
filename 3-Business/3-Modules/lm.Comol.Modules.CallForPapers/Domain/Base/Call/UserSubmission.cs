using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class UserSubmission : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual BaseForPaper Call { get; set; }
        //public virtual IList<FieldValue> FieldsValues { get; set; }
        public virtual SubmitterType Type { get; set; }
        public virtual SubmissionStatus Status { get; set; }
        public virtual litePerson Owner { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual litePerson SubmittedBy { get; set; }
        public virtual IList<SubmittedFile> Files { get; set; }
        public virtual liteCommunity Community { get; set; }
        public virtual DateTime? ExtensionDate { get; set; }

        public virtual Guid UserCode { get; set; }
        public virtual Boolean isAnonymous { get; set; }
        public virtual Boolean isComplete { get; set; }
        public virtual IList<Revision> Revisions { get; set; }

        public UserSubmission() {
            Revisions = new List<Revision>();
            Files = new List<SubmittedFile>();
            //FieldsValues = new List<FieldValue>();
            Status = SubmissionStatus.draft;
        }

        public virtual Boolean AllowEditSubmission()
        {
            return Call.AllowSubmission() && (Status == SubmissionStatus.none || Status == SubmissionStatus.draft);
        }
        public virtual Boolean AllowEditSubmission(DateTime initDate, DateTime clickDt)
        {
            return Call.AllowLateSubmission(initDate, ExtensionDate, clickDt) && (Status == SubmissionStatus.none || Status == SubmissionStatus.draft);
        }

        public virtual Boolean isOriginal()
        {
            return (Revisions.Count == 0 || !Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && r.Type != RevisionType.Original).Any());
        }
        public virtual long GetIdOriginal()
        {
            return (Revisions == null || Revisions.Count == 0) ? 0 : Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && r.Type == RevisionType.Original).OrderByDescending(r => r.Id).Select(r => r.Id).FirstOrDefault();
        }
        public virtual long GetIdLastActiveRevision()
        {
            return (Revisions == null || Revisions.Count == 0) ? 0 : Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && r.IsActive).OrderByDescending(r => r.Id).Select(r => r.Id).FirstOrDefault();
        }
        public virtual Boolean HasWorkingRevision()
        {
            return Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && (r.Status == RevisionStatus.Request || r.Status == RevisionStatus.RequestAccepted || r.Status == RevisionStatus.Required)).Any();
        }
        /// <summary>
        /// Restituisce l'id dell'ultima revisione su cui l'utente deve mettere mano o ha messo mano
        /// quindi:
        /// </summary>
        /// <returns></returns>
        public virtual long GetIdWorkingRevision()
        {
            return (Revisions == null || Revisions.Count == 0) ? 0 : Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && (!r.IsActive && (r.Status == RevisionStatus.Request || r.Status == RevisionStatus.RequestAccepted || r.Status == RevisionStatus.Required))).OrderByDescending(r => r.Id).Select(r => r.Id).FirstOrDefault();
        }
        public virtual Revision GetWorkingRevision()
        {
            return Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && !r.IsActive && (r.Status == RevisionStatus.Request || r.Status == RevisionStatus.RequestAccepted || r.Status == RevisionStatus.Required)).OrderByDescending(r => r.Id).FirstOrDefault();
        }
        public virtual List<Revision> GetAcceptedRevisions()
        {
            return Revisions.Where(r => (r.Deleted == BaseStatusDeleted.None && (r.IsActive || r.Type == RevisionType.Original
                                    || r.Status == RevisionStatus.Approved))).OrderByDescending(r => r.Id).ToList();
        }
    }
}
