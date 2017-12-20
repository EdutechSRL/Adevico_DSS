using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class dtoSubmissionDisplayItemPermission : dtoBase 
    {
        public virtual dtoSubmissionDisplayPermission Permission { get; set; }
        public virtual dtoSubmissionDisplay Submission { get; set; }

        public dtoSubmissionDisplayItemPermission()
        {
            Permission = new dtoSubmissionDisplayPermission();
        }
    }
    
}