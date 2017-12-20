using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
 
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoFilterSubmissions
    {
        public virtual SubmissionsOrder OrderBy { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual SubmissionFilterStatus Status { get; set; }
        public virtual String SearchForName { get; set; }
        public virtual long  IdSubmitterType { get; set; }
        public virtual Dictionary<SubmissionStatus, String> TranslationsSubmission { get; set; }
        public dtoFilterSubmissions()
        {
            TranslationsSubmission = new Dictionary<SubmissionStatus, String>();
            IdSubmitterType = -1;
        }
    }  
}