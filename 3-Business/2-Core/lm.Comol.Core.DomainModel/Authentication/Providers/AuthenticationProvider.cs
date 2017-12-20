using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class AuthenticationProvider : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual String UniqueCode { get; set; }
        public virtual AuthenticationProviderType ProviderType { get; set; }
        public virtual int IdOldAuthentication { get; set; }
        public virtual Boolean DisplayToUser { get; set; }
        public virtual Boolean AllowAdminProfileInsert { get; set; }
        public virtual Boolean AllowMultipleInsert { get; set; }
        public virtual IdentifierField IdentifierFields { get; set; }
        public virtual Boolean MultipleItemsForRecord { get; set; }
        public virtual String MultipleItemsSeparator { get; set; }
        public virtual Boolean IsEnabled { get; set; }
        public virtual LogoutMode LogoutMode { get; set; }
        
        public virtual IList<AuthenticationProviderTranslation> Translations { get; set; }
       // public virtual IList<IpAddressPolicy> IpAddresses { get; set; }
        /// <summary>
        ///         /// 
        /// </summary>
        /// <param name="uLanguage"> user language</param>
        /// <param name="dLanguage"> default language</param>
        public virtual AuthenticationProviderTranslation GetTranslated(Language uLanguage, Language dLanguage)
        {
            if (Translations.Where(t => t.Language == uLanguage).Any())
                return Translations.Where(t => t.Language == uLanguage).FirstOrDefault();
            else
                return Translations.Where(t => t.Language == dLanguage).FirstOrDefault();
        }
        public AuthenticationProvider() {
            Translations = new List<AuthenticationProviderTranslation>();
            //IpAddresses = new List<IpAddressPolicy>();
        }

    } 




    [Serializable]
    public class TempAuthenticationProvider : AuthenticationProvider
    {

    }
}