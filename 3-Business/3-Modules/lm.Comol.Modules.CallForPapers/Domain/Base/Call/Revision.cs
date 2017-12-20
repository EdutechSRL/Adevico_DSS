using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    
    [Serializable()]
    public enum RevisionMode
    {
        None = 0,
        OnlyManager = 1,
        ManagerSubmitter = 2,
        OnlySubmitter = 3
    }
    [Serializable()]
    public enum FieldRevisionType{
        Optional = 0,
        Required = 1
    }
    [Serializable()]
    public enum RevisionStatus{
        None = 0,
        Request = 1,
        RequestAccepted = 2,
        Required = 3,
        Submitted = 4,
        Approved = 5,
        Refused = 6,
        Cancelled = 7
    }
    [Serializable()]
    public enum RevisionType{
        None = 0,
        Original = 1,
        UserRequired = 2,
        Manager = 3
    }
    [Serializable()]
    public enum RevisionOrder
    {
        None = 0,
        ByCall = 1,
        ByStatus = 2,
        ByDeadline = 3,
        ByType = 4,
        ByUser = 5,
        ByDate = 6
    }
    [Serializable()]
    public class Revision : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Int32 Number { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual UserSubmission Submission { get; set; }
        public virtual RevisionType Type { get; set; }
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem FileZip { get; set; }
        public virtual liteModuleLink LinkZip { get; set; }
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem FilePDF { get; set; }
        public virtual liteModuleLink LinkPDF { get; set; }
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem FileRTF { get; set; }
        public virtual liteModuleLink LinkRTF { get; set; }
        public virtual RevisionStatus Status { get; set; }
        public virtual Boolean AllowSave { 
            get{
                return (Type == RevisionType.Original) || (Type == RevisionType.UserRequired && Status == RevisionStatus.RequestAccepted)
                    || (Type == RevisionType.Manager && Status == RevisionStatus.Required);
            }
        }
        public Revision()
        {
        }
    }
    [Serializable()]
    public class OriginalRevision : Revision
    {
        public OriginalRevision()
        {
            Type = RevisionType.Original;
            Number = 0;
        }
    }
    [Serializable()]
    public class RevisionRequest : Revision
    {
        public virtual IList<RevisionItem> ItemsToReview { get; set; }
        public virtual String Reason { get; set; }
        public virtual String Feedback { get; set; }
        public virtual Boolean ForAllFields { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual litePerson RequiredBy { get; set; }
        public virtual litePerson RequiredTo { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual litePerson SubmittedBy { get; set; }
        public RevisionRequest()
        {
            ItemsToReview = new List<RevisionItem>();
        }
        public virtual Boolean AllowSubmission(DateTime initDate)
        {
            if (!EndDate.HasValue)
                return true;
            else
                return (initDate <= EndDate.Value) && AllowSave;
        }
    }
    [Serializable()]
    public class RevisionItem : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Revision Revision { get; set; }
        public virtual FieldDefinition Field { get; set; }
        public virtual FieldRevisionType Type { get; set; }

        public RevisionItem()
        {
            Type = FieldRevisionType.Optional;
        }
    }
}
