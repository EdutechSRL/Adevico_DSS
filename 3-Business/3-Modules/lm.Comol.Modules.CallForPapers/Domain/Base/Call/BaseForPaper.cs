using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain //.RequestForMembership
{
    [Serializable()]
    public abstract class BaseForPaper : DomainBaseObjectLiteMetaInfo<long>
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
        public virtual IList<FieldsSection> Sections { get; set; }
        public virtual String NotificationEmail { get; set; }
        public virtual Boolean SubmissionClosed { get; set; }
        public virtual long IdDssMethod { get; set; }
        public virtual long IdDssRatingSet { get; set; }
        public virtual Boolean IsDssMethodFuzzy { get; set; }
        public virtual String FuzzyMeWeights { get; set; }
        public virtual Boolean UseManualWeights { get; set; }
        public virtual Boolean UseOrderedWeights { get; set; }
        public virtual Boolean IsValidFuzzyMeWeights { get; set; }

        public virtual IList<SubmitterType> SubmittersType { get; set; }
        public virtual liteCommunity Community { get; set; }
        public virtual Boolean IsPublic { get; set; }
        public virtual Boolean IsPortal { get; set; }
        public virtual CallForPaperStatus Status { get; set; } //published, draft,
        public virtual IList<AttachmentFile> Attachments { get; set; }
        public virtual int OverrideHours { get; set; }
        public virtual int OverrideMinutes { get; set; }
        public virtual Boolean UseStartCompilationDate { get; set; }
        public virtual Boolean ForSubscribedUsers { get; set; }


        public virtual Boolean AttachSign { get; set; }

        public virtual Boolean AllowPrintDraft { get; set; }

        public virtual Boolean AdvacedEvaluation { get; set; }

        public virtual string Tags { get; set; }

        public BaseForPaper() {
            IsPublic = false;
            //DisplayWinner = false;
            Description = "";
            Name = "";
            Edition = "";
            Sections = new List<FieldsSection>();
            SubmissionClosed = false;
            Type = CallForPaperType.None ;
            //DisplayWinner = false;
            SubmittersType = new List<SubmitterType>();
            Status = CallForPaperStatus.Published;
            Attachments = new List<AttachmentFile>();
            Deleted = BaseStatusDeleted.None;
            OverrideHours = 0;
            OverrideMinutes = 0;
            RevisionSettings = RevisionMode.None;
            AcceptRefusePolicy = NotifyAcceptRefusePolicy.None;
        }

        private Boolean AllowSubmission(DateTime refernceDateTime)
        {
            if (!EndDate.HasValue)
                return true;
            else
            {
                DateTime expectedDate = EndDate.Value.AddHours(OverrideHours).AddMinutes(OverrideMinutes);
                return refernceDateTime <= expectedDate;
            }
        }
        public virtual Boolean AllowSubmission()
        {
            return AllowSubmission(DateTime.Now) && Status == CallForPaperStatus.SubmissionOpened;
        }
        public virtual Boolean AllowLateSubmission(DateTime initDate, DateTime? extensionDate, DateTime clickDt)
        {
            bool canByStatus = Status == CallForPaperStatus.SubmissionOpened;

            //Non ho data di sottomissione
            if (!EndDate.HasValue)
                return canByStatus;

            //Secondo il parametro uso come riferimento la data di INIZIO compilazione (initDate) o di Sottomissione (clickDt)
            DateTime referenceDateTime = (UseStartCompilationDate) ? initDate : clickDt;

            //SE ho un extensionDate, uso quello, altrimenti aggiungo l'eventuale estensione del tempo limite
            DateTime maxTime = (extensionDate.HasValue) ? extensionDate.Value : EndDate.Value.AddHours(OverrideHours).AddMinutes(OverrideMinutes);

            //Consento la sottomissione SOLO se il bando è SBLOCCATO e non ho superato il tempo limite secondo le logiche impostate.
            return canByStatus && (referenceDateTime <= maxTime);

            //if (!EndDate.HasValue)
            //    return Status == CallForPaperStatus.SubmissionOpened;
            //else if (!AllowSubmissionExtension)
            //    return AllowSubmission(initDate);
            //else
            //{
            //    DateTime expectedDate;
            //    if (ExtensionDate.HasValue)
            //    {
            //        expectedDate = ExtensionDate.Value;
            //        return (Status == CallForPaperStatus.SubmissionOpened) && (initDate <= expectedDate);
            //    }
            //    else
            //    {
            //        expectedDate = EndDate.Value.AddHours(OverrideHours).AddMinutes(OverrideMinutes);
            //        return (initDate <= expectedDate);
            //    }
            //}
        }
        public virtual Boolean AllowLateSubmission(DateTime initDate, UserSubmission submission, DateTime clickDt)
        {
            return AllowLateSubmission(initDate, submission.ExtensionDate, clickDt);

            //Ignoro vecchie logiche
            //if (!EndDate.HasValue)
            //    return true;
            //else if (!AllowSubmissionExtension)
            //    return AllowSubmission(initDate);
            //else
            //{
            //    DateTime expectedDate;
            //    if (submission.ExtensionDate.HasValue)
            //        expectedDate = submission.ExtensionDate.Value;
            //    else
            //        expectedDate = EndDate.Value.AddHours(OverrideHours).AddMinutes(OverrideMinutes);
            //    return (initDate <= expectedDate);
            //}
        }

        public virtual Dictionary<long, String> GetFuzzyMeItems()
        {
            Dictionary<long, String> results = new Dictionary<long, String>();
            if (!String.IsNullOrWhiteSpace(FuzzyMeWeights))
            {
                List<String> gItems = FuzzyMeWeights.Split('#').ToList();
                foreach (String g in gItems)
                {
                    List<String> values = g.Split(':').ToList();
                    if (values.Any())
                    {
                        long id = 0;
                        if (long.TryParse(values[0], out id))
                        {
                            if (results.ContainsKey(id))
                                results[id] = (values.Count()== 2 ? values[1] : results[id]);
                            else
                                results.Add(id, (values.Count()== 2 ? values[1] : ""));
                        }
                    }
                }
            }
           return results;
        }
    }


    [Serializable()]
    public enum NotifyAcceptRefusePolicy
    {
        None = 0,
        Accept = 1,
        Refuse = 2,
        All = 3
    }
}