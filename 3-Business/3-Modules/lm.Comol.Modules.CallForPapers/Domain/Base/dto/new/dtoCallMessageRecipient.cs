using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoCallMessageRecipient : lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessages
	{
        public virtual long IdSubmitterType { get; set; }
        public virtual String SubmitterType { get; set; }
        public virtual Boolean HasMultiSubmissions { get; set; }
        public virtual SubmissionStatus Status { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual DateTime? LastActionOn { 
            get {
                if (SubmittedOn.HasValue)
                    return SubmittedOn.Value;
                else if (ModifiedOn.HasValue)
                    return ModifiedOn.Value;
                else if (CreatedOn.HasValue)
                    return CreatedOn.Value;
                else
                    return null;
            } 
        }
        public virtual SubmissionStatus LastActionStatus
        {
            get
            {
                return (SubmittedOn.HasValue && (Status == SubmissionStatus.valuated || Status == SubmissionStatus.valuating || Status == SubmissionStatus.waitingValuation)) ? SubmissionStatus.submitted : Status;
            }
        }
        public virtual double SumRating { get; set; }
        public virtual Boolean HasEvaluations { get; set; }
        public virtual Boolean IsSubmitter { get { return IdSubmitterType > 0; } }

        public dtoCallMessageRecipient()
            : base()
        {
            Status = SubmissionStatus.none;
        }

        public dtoCallMessageRecipient(long id)
            : base(id)
        {
            Status = SubmissionStatus.none;
        }
        public dtoCallMessageRecipient(lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessages obj)
            : base(obj.Id)
        {
            Status = SubmissionStatus.none;
            Deleted = obj.Deleted;
            MessageNumber = obj.MessageNumber;
            IdAgency = obj.IdAgency;
            AgencyName = obj.AgencyName;
            Messages = obj.Messages;
            IdLanguage = obj.IdLanguage;
            CodeLanguage = obj.CodeLanguage;
            Type = obj.Type;
            IdPerson = obj.IdPerson;
            ModuleCode = obj.ModuleCode;
            IdUserModule = obj.IdUserModule;
            MailAddress = obj.MailAddress;
            if (String.IsNullOrEmpty(obj.DisplayName))
                DisplayName = obj.MailAddress;
            else
                DisplayName = obj.DisplayName;
        }
        public dtoCallMessageRecipient(litePerson p, String moduleCode = "", String anonymousUser = "")
        {
            Status = SubmissionStatus.none;
            MessageNumber = 0;
            IdLanguage = p.LanguageID;
            Type = lm.Comol.Core.MailCommons.Domain.RecipientType.BCC;
            IdPerson = p.Id;
            ModuleCode = moduleCode;
            if (p.TypeID == (int)UserTypeStandard.Guest || p.TypeID == (int)UserTypeStandard.PublicUser)
            {
                MailAddress = "";
                DisplayName = anonymousUser;
            }
            else {
                MailAddress = p.Mail;
                DisplayName = p.SurnameAndName;
            }
            Name = p.Name;
            Surname = p.Surname;
            IdProfileType = p.TypeID;
        }
        public dtoCallMessageRecipient(UserSubmission s, String moduleCode, String unknownUser, String anonymousUser)
            : base((long)0)
        {
            Status = s.Status;
            MessageNumber = 0;
            
            Type = lm.Comol.Core.MailCommons.Domain.RecipientType.BCC;
            if (s.Owner != null)
            {
                IdPerson = s.Owner.Id;
                IdLanguage = s.Owner.LanguageID;
                MailAddress = (s.Owner.TypeID == (int)UserTypeStandard.Guest || s.Owner.TypeID == (int)UserTypeStandard.PublicUser) ? "" : s.Owner.Mail;
                DisplayName = (s.Owner.TypeID == (int)UserTypeStandard.Guest || s.Owner.TypeID == (int)UserTypeStandard.PublicUser) ? anonymousUser : s.Owner.SurnameAndName;
                Name = s.Owner.Name;
                Surname = s.Owner.Surname;
                IdProfileType = s.Owner.TypeID;
            }
            else {
                MailAddress = "";
                DisplayName = anonymousUser;
                IdPerson = -1;
                IdLanguage = 0;
                IdProfileType = (int)UserTypeStandard.Guest;
            }
            ModuleCode = moduleCode;
            if (s.Type!=null){
                IdSubmitterType = s.Type.Id;
                SubmitterType = s.Type.Name;
            }
            CreatedOn = s.CreatedOn;
            ModifiedOn = s.ModifiedOn;
            SubmittedOn = s.SubmittedOn;
            IdModuleObject = s.Id;
            IdModuleType = (s.Call != null && s.Call.Type == CallForPaperType.CallForBids) ? (int)ModuleCallForPaper.ObjectType.UserSubmission : (int)ModuleRequestForMembership.ObjectType.UserSubmission;
        }


        //public lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient ToBaseMessageRecipient() { 
        //    new lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient() { 
        //                    Type= r.Type,
        //                    ModuleCode= r.ModuleCode,
        //                    IdUserModule= r.IdUserModule,
        //                    IdPerson= r.IdPerson,
        //                    IdModuleType= r.IdModuleType,
        //                    IdModuleObject= r.IdModuleObject,
        //                    CodeLanguage= r.CodeLanguage,
        //                    IdLanguage= r.IdLanguage 
        // Type= r.Type,
        //                    ModuleCode= r.ModuleCode,
        //                    IdUserModule= r.IdUserModule,
        //                    IdPerson= r.IdPerson,
        //                    IdModuleType= r.IdModuleType,
        //                    IdModuleObject= r.IdModuleObject,
        //                    CodeLanguage= r.CodeLanguage,
        //                    IdLanguage= r.IdLanguage 
        //}
	}
}