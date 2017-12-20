using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoLazySubmission : dtoBaseLazySubmission
    {
        public virtual dtoSubmitterType Type { get; set; }
        public virtual long IdCall { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual Boolean IsAnonymous { get; set; }

        public dtoLazySubmission()
            : base()
        {

        }

        public dtoLazySubmission(long id)
            : base(id)
        {

        }
    } 
}