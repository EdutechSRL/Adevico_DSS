using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoSubmissionDisplay : dtoBase 
    {
        public virtual long IdCall { get; set; }
        public virtual long IdRevision { get; set; }
        public virtual SubmissionStatus Status { get; set; }

        public virtual dtoSubmitterType Type { get; set; }
        public virtual litePerson Owner { get; set; }
        public virtual int IdOwner { get; set; }
        //public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual litePerson ModifiedBy { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual litePerson SubmittedBy { get; set; }
        public virtual DateTime? ExtensionDate { get; set; }
        public virtual List<dtoSubmissionItem> Revisions { get; set; }
        public virtual System.Guid UniqueID { get; set; }
        public virtual Boolean IsAnonymous { get; set; }

        public virtual ModuleLink SignLink { get; set; }

        public virtual int RevisionSended { get; set; }
        public virtual int RevisionAswered { get; set; }

        public dtoSubmissionDisplay():base()
        {
            Revisions = new List<dtoSubmissionItem>();
        }
    }

    [Serializable]
    public class dtoSubmissionDataDisplay : dtoSubmissionDisplay 
    {
        public virtual List<dtoCallSection<dtoSubmissionValueField>> Sections { get; set; }
    }


    [Serializable]
    public class dtoSubmissionItem : dtoBase {
        public virtual String DisplayNumber { get; set; }
        public virtual dtoSubmissionDisplay Submission { get; set; }
        public virtual Boolean isActive { get; set; }
        public virtual RevisionStatus RevisionStatus { get; set; }
        public virtual RevisionType RevisionType { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual displayAs Display { get; set; }

        public virtual ModuleLink SignLink { get; set; }
    }
    
    [Serializable,Flags]
    public enum displayAs 
    {
       first = 1,
       item = 2,
       last = 4
    }
}