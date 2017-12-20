using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable,DataContract]
    [KnownType(typeof(ModuleKeyTranslations))]
    public class ModuleKeyTranslations
    {
        [DataMember]
        public virtual String Key { get; set; }
        [DataMember]
        public virtual List<TranslatedSettings> Translations { get; set; }

        public ModuleKeyTranslations()
        {
            Translations = new List<TranslatedSettings>();
        }
    }
}