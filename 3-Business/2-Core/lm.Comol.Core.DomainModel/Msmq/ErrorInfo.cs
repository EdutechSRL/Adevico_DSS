using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.DataContract
{
    [Serializable, DataContract]
    public class ErrorInfo
    {
        [DataMember]
        public String InnerException { get; set; }
        [DataMember]
        public String Message { get; set; }
        [DataMember]
        public String Source { get; set; }
        [DataMember]
        public String StackTrace { get; set; }
        [DataMember]
        public String HelpLink { get; set; }
        [DataMember]
        public String Type { get; set; }
    }
}