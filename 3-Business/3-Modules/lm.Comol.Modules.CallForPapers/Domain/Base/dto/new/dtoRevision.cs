using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class dtoRevision : dtoBase 
    {
        public virtual Int32 SubVersion { get; set; }
        public virtual Int32 Number { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual litePerson CreatedBy { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual RevisionType Type { get; set; }
        public virtual dtoRevisionFiles Files { get; set; }
        public virtual RevisionStatus Status { get; set; }
        public virtual Boolean AllowSave { 
            get{
                return ( Deleted == BaseStatusDeleted.None && ( (Type == RevisionType.Original) || (Type == RevisionType.UserRequired && Status == RevisionStatus.RequestAccepted)
                    || (Type == RevisionType.Manager && Status == RevisionStatus.Required)));
            }
        }
        public dtoRevision()
        {
            Files = new dtoRevisionFiles();
        }
        public static dtoRevision Initialize(Revision rev)
        {
            dtoRevision dto = new dtoRevision();
            dto.Id = rev.Id;
            dto.Deleted = rev.Deleted;
            dto.IsActive = rev.IsActive;
            dto.Type = rev.Type;
            dto.Status = rev.Status;
            dto.Files = new dtoRevisionFiles();
            dto.Files.FilePDF = rev.FilePDF;
            dto.Files.FileRTF = rev.FileRTF;
            dto.Files.FileZip = rev.FileZip;
            dto.Files.LinkPDF = rev.LinkPDF;
            dto.Files.LinkRTF = rev.LinkRTF;
            dto.Files.LinkZip = rev.LinkZip;
            dto.CreatedOn = rev.CreatedOn;
            dto.Number = rev.Number;
            dto.CreatedBy = rev.CreatedBy;
            return dto;
        }
        public List<dtoSubmissionAttachment> SubmissionFiles()
        {
            List<dtoSubmissionAttachment> files = new List<dtoSubmissionAttachment>();
            if (Files != null)
            {
                if (Files.LinkPDF != null && Files.FilePDF != null)
                    files.Add(new dtoSubmissionAttachment() { Item = Files.FilePDF, Link = Files.LinkPDF, ModuleLinkId = Files.LinkPDF.Id, Type = lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.pdf });
                //if (Files.LinkRTF != null && Files.FileRTF != null)
                //    files.Add(new dtoSubmissionAttachment() { Item = Files.FileRTF, Link = Files.LinkRTF, ModuleLinkId = Files.LinkRTF.Id, Type = lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.rtf });
                if (Files.LinkZip != null && Files.FileZip != null)
                    files.Add(new dtoSubmissionAttachment() { Item = Files.FileZip, Link = Files.LinkZip, ModuleLinkId = Files.LinkZip.Id, Type = lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.zip });
            }
            return files;
        }
    }
    [Serializable()]
    public class dtoRevisionRequest : dtoRevision
    {
        public virtual List<dtoRevisionItem> ItemsToReview { get; set; }
        public virtual String Reason { get; set; }
        public virtual String Feedback { get; set; }
        
        public virtual Boolean ForAllFields { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual litePerson ModifiedBy { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual litePerson RequiredBy { get; set; }
        public virtual litePerson RequiredTo { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual litePerson SubmittedBy { get; set; }
     

        public dtoRevisionRequest()
        {
            ItemsToReview = new List<dtoRevisionItem>();
        }
        public static dtoRevisionRequest Initialize(RevisionRequest rev, Boolean full)
        {
            dtoRevisionRequest dto = new dtoRevisionRequest();
            dto.Id = rev.Id;
            dto.Deleted = rev.Deleted;
            dto.IsActive = rev.IsActive;
            dto.Type = rev.Type;
            dto.Status = rev.Status;
            dto.Files = new dtoRevisionFiles();
            dto.Files.FilePDF = rev.FilePDF;
            dto.Files.FileRTF = rev.FileRTF;
            dto.Files.FileZip = rev.FileZip;
            dto.Files.LinkPDF = rev.LinkPDF;
            dto.Files.LinkRTF = rev.LinkRTF;
            dto.Files.LinkZip = rev.LinkZip;
            dto.EndDate = rev.EndDate;
            dto.CreatedOn = rev.CreatedOn;
            dto.CreatedBy = rev.CreatedBy;
            dto.RequiredTo = rev.RequiredTo;
            dto.RequiredBy = rev.RequiredBy;
            dto.Number = rev.Number;
            if (full) {
                dto.Reason = rev.Reason;
                dto.Feedback = rev.Feedback;
                dto.ModifiedOn = rev.ModifiedOn;
                dto.ModifiedBy = rev.ModifiedBy;
                dto.SubmittedBy = rev.SubmittedBy;
                dto.SubmittedOn = rev.SubmittedOn;
                dto.ItemsToReview = (from i in rev.ItemsToReview where i.Deleted== BaseStatusDeleted.None select new dtoRevisionItem(i)).ToList();
            }
            return dto;
        }
        public Boolean AllowSubmission(DateTime initDate)
        {
            if (!EndDate.HasValue)
                return true;
            else
                return (initDate <= EndDate.Value); // && AllowSave;
        }
    }
    [Serializable()]
    public class dtoRevisionItem : dtoBase
    {
        public virtual long IdField { get; set; }
        public virtual FieldRevisionType Type { get; set; }

        public dtoRevisionItem()
        {
            Type = FieldRevisionType.Optional;
        }
        public dtoRevisionItem(RevisionItem item)
        {
            Type = item.Type;
            Id = item.Id;
            Deleted = item.Deleted;
            IdField = item.Field.Id;
        }
    }
    [Serializable()]
    public class dtoRevisionFiles 
    {
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem FileZip { get; set; }
        public virtual liteModuleLink LinkZip { get; set; }
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem FilePDF { get; set; }
        public virtual liteModuleLink LinkPDF { get; set; }
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem FileRTF { get; set; }
        public virtual liteModuleLink LinkRTF { get; set; }

        public dtoRevisionFiles()
        {

        }
    }
    [Serializable()]
    public class dtoRevisionMessage 
    {
        public virtual lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig SmtpConfig { get; set; }
        public virtual String Subject { get; set; }
        public virtual String Body { get; set; }
        public dtoRevisionMessage()
        {

        }
        public static String GetPlaceHolder(RevisionPlaceHoldersType type)
        {
            return "[" + type.ToString() + "]";
        }


        [Serializable()]
        public enum RevisionPlaceHoldersType
        {
            None = 0,
            CallName = 1,
            CallEdition = 2,
            RequestOn = 3,
            RequestTime = 4,
            CallCommunity = 5,
            SubmitterName = 6,
            SubmitterSurname = 7,
            SubmitterType = 8,
            Reason = 9,
            DeadlineOn = 10,
            DeadlineTime = 11,
            RequiredBy = 12,
            LinkUrl = 13,
            RequiredTo = 14,
            Fields = 15
        }
    }

    public class dtoRevisionDisplay : dtoBase
    {
        public virtual Int32 Number { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual litePerson CreatedBy { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual RevisionType Type { get; set; }
        public virtual RevisionStatus Status { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual litePerson RequiredBy { get; set; }
        public virtual litePerson RequiredTo { get; set; }
        public virtual long IdCall { get; set; }
        public virtual long IdSubmission { get; set; }
        public virtual String CallName { get; set; }
        public virtual litePerson Submitter
        {
            get
            {
                switch (Type) { 
                    case RevisionType.Original:
                        return CreatedBy;
                    case  RevisionType.UserRequired:
                        return RequiredBy;
                    case RevisionType.Manager:
                        return RequiredTo;
                    default:
                        return CreatedBy;
                }
            }
        }
        public virtual litePerson Manager
        {
            get
            {
                switch (Type)
                {
                    case RevisionType.UserRequired:
                        return RequiredTo;
                    case RevisionType.Manager:
                        return RequiredBy;
                    default:
                        return null;
                }
            }
        }
        public virtual Boolean AllowSave
        {
            get
            {
                return (Deleted == BaseStatusDeleted.None && ((Type == RevisionType.Original) || (Type == RevisionType.UserRequired && Status == RevisionStatus.RequestAccepted)
                    || (Type == RevisionType.Manager && Status == RevisionStatus.Required)));
            }
        }

        public dtoRevisionDisplay()
        {
        }
        public static dtoRevisionDisplay Initialize(RevisionRequest rev)
        {
            dtoRevisionDisplay dto = new dtoRevisionDisplay();
            dto.Id = rev.Id;
            dto.Deleted = rev.Deleted;
            dto.IsActive = rev.IsActive;
            dto.Type = rev.Type;
            dto.Status = rev.Status;
            dto.EndDate = rev.EndDate;
            dto.CreatedOn = rev.CreatedOn;
            dto.CreatedBy = rev.CreatedBy;
            dto.RequiredTo = rev.RequiredTo;
            dto.RequiredBy = rev.RequiredBy;
            dto.Number = rev.Number;

            return dto;
        }
        public Boolean AllowSubmission(DateTime initDate)
        {
            if (!EndDate.HasValue)
                return true;
            else
                return (initDate <= EndDate.Value); // && AllowSave;
        }
    }
    
}