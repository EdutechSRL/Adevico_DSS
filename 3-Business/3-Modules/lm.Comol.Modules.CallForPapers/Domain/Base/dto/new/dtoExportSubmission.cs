using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoExportSubmission
    {
        public virtual UserSubmission Submission {get;set;}
        public virtual String Filename {get;set;}
        public virtual String ClientFilename {get;set;}
        public virtual List<dtoCallSubmissionFile> RequiredFiles {get;set;}
        public virtual List<dtoCallSection<dtoSubmissionValueField>> Sections {get;set;}
        public virtual BaseForPaper Call {get;set;}
        public virtual SubmitterType Submitter {get;set;}
        public virtual litePerson PrintBy { get; set; }

        public virtual Boolean ForCompile {
            get {
                return (Call != null) && (Submitter != null);
            }
        }
        public virtual Boolean SaveToFile
        {
            get
            {
                return !String.IsNullOrEmpty(Filename);
            }
        }
        public virtual Boolean ForWebDownload
        {
            get
            {
                return !String.IsNullOrEmpty(ClientFilename);
            }
        }
        public dtoExportSubmission() { }

    }
}