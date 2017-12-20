using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoBaseSubmission : dtoLazySubmission
    {
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual litePerson SubmittedBy { get; set; }
        public virtual List<dtoRevision> Revisions { get; set; }

        public dtoBaseSubmission()
            : base()
        {
            Revisions = new List<dtoRevision>();
        }

        public dtoBaseSubmission(long id)
            : base(id)
        {
            Revisions = new List<dtoRevision>();
        }
        public dtoBaseSubmission(long id, List<Revision> revisions, Boolean full)
                    : base(id)
        {
            Revisions = new List<dtoRevision>();
            foreach (Revision rev in revisions)
            {
                if (rev != null)
                {
                    if (rev is RevisionRequest)
                        Revisions.Add(dtoRevisionRequest.Initialize((RevisionRequest)rev, full));
                    else
                        Revisions.Add(dtoRevision.Initialize(rev));
                }
            }
        }

        public Boolean isOriginal()
        {
            return (Revisions.Count == 0 || !Revisions.Where(r => r.Deleted== BaseStatusDeleted.None && r.Type != RevisionType.Original).Any());
        }
        public long GetIdOriginal()
        {
            return (Revisions == null || Revisions.Count == 0) ? 0 : Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && r.Type== RevisionType.Original).OrderByDescending(r => r.Id).Select(r => r.Id).FirstOrDefault();
        }
        public long GetIdLastActiveRevision()
        {
            return (Revisions == null || Revisions.Count == 0) ? 0 : Revisions.Where(r => r.Deleted== BaseStatusDeleted.None && r.IsActive).OrderByDescending(r => r.Id).Select(r => r.Id).FirstOrDefault();
        }
        public Boolean HasWorkingRevision()
        {
            return Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && (r.Status == RevisionStatus.Request || r.Status == RevisionStatus.RequestAccepted || r.Status == RevisionStatus.Required || r.Status == RevisionStatus.Submitted)).Any();
        }
        /// <summary>
        /// Restituisce l'id dell'ultima revisione su cui l'utente deve mettere mano o ha messo mano
        /// quindi:
        /// </summary>
        /// <returns></returns>
        public long GetIdWorkingRevision()
        {
            return (Revisions == null || Revisions.Count == 0) ? 0 : Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && (!r.IsActive && (r.Status == RevisionStatus.Request || r.Status == RevisionStatus.RequestAccepted || r.Status == RevisionStatus.Required || r.Status == RevisionStatus.Submitted))).OrderByDescending(r => r.Id).Select(r => r.Id).FirstOrDefault();
        }
        public dtoRevisionRequest GetWorkingRevision()
        {
            return Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && (r.Type == RevisionType.UserRequired || r.Type == RevisionType.Manager) && !r.IsActive && (r.Status == RevisionStatus.Request || r.Status == RevisionStatus.RequestAccepted || r.Status == RevisionStatus.Required || r.Status == RevisionStatus.Submitted)).OrderByDescending(r => r.Id).Select(r => (dtoRevisionRequest)r).FirstOrDefault();
        }
        public List<dtoRevision> GetAcceptedRevisions()
        {
            return Revisions.Where(r => (r.Deleted == BaseStatusDeleted.None && (r.IsActive || r.Type == RevisionType.Original
                                    || r.Status == RevisionStatus.Approved))).OrderByDescending(r => r.Id).ToList();
        }
        
    }
}