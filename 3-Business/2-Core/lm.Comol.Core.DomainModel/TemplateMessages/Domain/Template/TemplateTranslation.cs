using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{

    [Serializable]
    public class TemplateTranslation : DomainBaseObjectLiteMetaInfo<long>, IDisposable
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual String LanguageName { get; set; }
        public virtual TemplateDefinitionVersion Version { get; set; }
        public virtual Boolean IsEmpty { get { return Translation.IsEmpty(); } }
        public virtual Boolean IsValid { get { return Translation.IsValid(); } }
        public virtual lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation Translation { get; set; }

        public TemplateTranslation()
        {
            Translation = new lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation();
        }

        public virtual TemplateTranslation Copy(TemplateDefinitionVersion version, litePerson person, String ipAddress, String ipProxyAddress, String namePrefix = "")
        {
            TemplateTranslation t = new TemplateTranslation();
            t.CreateMetaInfo(person, ipAddress, ipProxyAddress);

            t.Version = version;
            t.IdLanguage = IdLanguage;
            t.LanguageCode = LanguageCode;
            t.Translation = Translation.Copy();
            if (!String.IsNullOrEmpty(namePrefix))
                t.Translation.Name = namePrefix + t.Translation.Name;
            return t;
        }

        public void Dispose()
        {
        }
    }
}