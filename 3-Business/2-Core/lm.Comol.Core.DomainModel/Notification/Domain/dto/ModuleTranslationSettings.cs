using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable,DataContract]
    [KnownType(typeof(ModuleTranslationSettings))]
    public class ModuleTranslationSettings
    {
        [DataMember]
        public virtual String ModuleCode { get; set; }
        [DataMember]
        public virtual List<ModuleKeyTranslations> Keys { get; set; }

        public ModuleTranslationSettings()
        {
            Keys = new List<ModuleKeyTranslations>();
        }


        public Dictionary<String, String> GetModuleTranslationKeys(String languageCode)
        {
            Dictionary<String,String> results = new Dictionary<string,string>();

            foreach (var item in Keys.Where(k=> k.Translations.Any()).GroupBy(i => i.Key))
            {
                var query = item.SelectMany(t => t.Translations).ToList();
                String value = query.Where(v => v.CodeLanguage == languageCode).Select(v => v.Name).FirstOrDefault();
                if (String.IsNullOrWhiteSpace(value))
                    value = query.Where(v => v.IsDefault).Select(v => v.Name).FirstOrDefault();
                if (String.IsNullOrWhiteSpace(value))
                    value = query.Where(v => v.CodeLanguage == "multi").Select(v => v.Name).FirstOrDefault();
                results.Add(item.Key, value);
            }
            return results;
        }
    }
}