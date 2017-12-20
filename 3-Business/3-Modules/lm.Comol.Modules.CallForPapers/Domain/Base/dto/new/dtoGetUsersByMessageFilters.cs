using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
 
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoUsersByMessageFilter
    {
        public virtual AccessTypeFilter Access { get; set; }
        public virtual UserByMessagesOrder OrderBy { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual SubmissionFilterStatus Status { get; set; }
        public virtual String StartWith { get; set; }
        public virtual lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy SearchBy { get; set; }
        public virtual String Value { get; set; }
        public virtual long IdSubmitterType { get; set; }
        public virtual long IdAgency { get; set; }
        public virtual List<long> IdMessages { get; set; }
        public virtual Dictionary<SubmissionStatus, String> StatusTranslations { get; set; }
        public dtoUsersByMessageFilter()
        {
            StatusTranslations = new Dictionary<SubmissionStatus, String>();
            IdSubmitterType = -1;
        }
    }  
}