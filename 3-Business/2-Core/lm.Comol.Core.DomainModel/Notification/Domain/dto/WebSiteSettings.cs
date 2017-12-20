using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable,DataContract]
    [KnownType(typeof(WebSiteSettings))]
    public class WebSiteSettings
    {

#region "Property"
        [DataMember]
        public virtual String Baseurl { get; set; }
        [DataMember]
        public virtual String WebSiteUrlNoSsl { get; set; }
        [DataMember]
        public virtual List<TranslatedSettings> VoidDateTime { get; set; }
        [DataMember]
        public virtual List<TranslatedSettings> Settings { get; set; }
        [DataMember]
        public virtual List<DateTimeFormat> DateTimeFormat { get; set; }
        [DataMember]
        public virtual List<ModuleTranslationSettings> Modules { get; set; }
        
#endregion

        public WebSiteSettings()
        {
            DateTimeFormat = new List<DateTimeFormat>();
            VoidDateTime = new List<TranslatedSettings>();
            Settings = new List<TranslatedSettings>();
            Modules = new List<ModuleTranslationSettings>();
        }

        public String GetPortalName(String languageCode)
        {
            TranslatedSettings t = (Settings==null) ? null : (Settings.Any(s=> s.CodeLanguage== languageCode) ? Settings.Where(s=> s.CodeLanguage== languageCode).FirstOrDefault() : Settings.Where(s=> s.IsDefault).FirstOrDefault());
            return (t == null ? "" : t.Name);
        }
        public String GetVoidDateTime(String languageCode)
        {
            TranslatedSettings t = (Settings == null) ? null : (VoidDateTime.Any(s => s.CodeLanguage == languageCode) ? VoidDateTime.Where(s => s.CodeLanguage == languageCode).FirstOrDefault() : VoidDateTime.Where(s => s.IsDefault).FirstOrDefault());

            return (t == null ? "" : t.Name);
        }

        public List<ModuleTranslationSettings> GetModuleTranslations(String moduleCode)
        {
            return (Modules == null || String.IsNullOrWhiteSpace(moduleCode)) ? new List<ModuleTranslationSettings>() : Modules.Where(m => !String.IsNullOrWhiteSpace(m.ModuleCode) && m.ModuleCode.ToLower() == moduleCode.ToLower()).ToList();
        }
        public Dictionary<String, String> GetModuleTranslationKeys(String moduleCode, String languageCode)
        {
            ModuleTranslationSettings module = GetModuleTranslations(moduleCode).FirstOrDefault();
            return (module==null) ?  new Dictionary<String, String>() : module.GetModuleTranslationKeys(languageCode);
        }

        public void GenerateDateTimeFormat(List<String> codes)
        {
            DateTimeFormat = codes.Select(c => new DateTimeFormat() { CodeLanguage = c, Format = GenerateDateTimeFormat(c) }).ToList();
        }
        public String GenerateDateTimeFormat(String languageCode)
        {
            String format = "dd/MM/yyyy HH:mm:ss";
            try{
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(languageCode);
                format = culture.DateTimeFormat.FullDateTimePattern;
                culture = null;
            }
            catch(Exception ex){

            }
            return format;
        }
        public String GetDateTimeFormat(String languageCode)
        {
            String format = DateTimeFormat.Where(f => f.CodeLanguage == languageCode).Select(f=> f.Format).FirstOrDefault();
            if (format == "")
                format = GenerateDateTimeFormat(languageCode);
            return format;
        }
    }
}