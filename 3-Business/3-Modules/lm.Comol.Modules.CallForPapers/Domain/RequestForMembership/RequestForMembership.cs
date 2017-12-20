using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain //.RequestForMembership
{
    [Serializable()]
    public class RequestForMembership : BaseForPaper 
    {
        public virtual String StartMessage { get; set; }
        public virtual String EndMessage { get; set; }
       
        public RequestForMembership() :base()
        {
            Type = CallForPaperType.RequestForMembership;
        }
    }
}