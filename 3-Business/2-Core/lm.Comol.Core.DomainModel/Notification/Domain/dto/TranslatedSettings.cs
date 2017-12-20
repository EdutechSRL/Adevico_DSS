using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable,DataContract]
    [KnownType(typeof(TranslatedSettings))]
    public class TranslatedSettings
    {
        [DataMember]
        public virtual Int32 IdLanguage { get; set; }
        [DataMember]
        public virtual String CodeLanguage { get; set; }
        [DataMember]
        public virtual String Name { get; set; }
        [DataMember]
        public virtual Boolean IsDefault  { get; set; }

        public TranslatedSettings()
        {
        }
    }
}