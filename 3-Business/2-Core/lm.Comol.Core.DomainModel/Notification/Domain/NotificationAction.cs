using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable, DataContract]
    public class NotificationAction
    {
        [DataMember]
        public Int32 IdCommunity { get; set; }
        [DataMember]
        public String ModuleCode{get;set;}
        [DataMember]
        public long IdModuleAction{get;set;}
        [DataMember]
        public long IdObject{get;set;}
        [DataMember]
        public long IdObjectType{get;set;}
        [DataMember]
        public List<Int32> IdInternalUsers { get; set; }
        [DataMember]
        public List<long> IdModuleUsers { get; set; }

        public NotificationAction()
        {
            IdModuleUsers = new List<long>();
            IdInternalUsers = new List<Int32>();
        }
    }
}
