using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public enum AuthenticationProviderType
    {
        /// <summary>
        /// Unknow
        /// </summary>
        None = 0,
        /// <summary>
        /// Login e pwd gestite dal sistema
        /// </summary>
        Internal = 1,
        /// <summary>
        /// LDAP: VERIFICARE
        /// </summary>
        Ldap = 2,
        /// <summary>
        /// Url providere (url con dati utente)
        /// </summary>
        Url = 3,
        /// <summary>
        /// AD: probabilmente non implementato
        /// </summary>
        ActiveDirectory= 4,
        /// <summary>
        /// WebService: probabilmente non implementato
        /// </summary>
        WebService = 5,
        /// <summary>
        /// API: probabilmente non implementato
        /// </summary>
        ExternalAPI = 6,
        /// <summary>
        /// Shibboleth: da verificare
        /// </summary>
        Shibboleth = 7,
        /// <summary>
        /// URL con validazione MAC
        /// </summary>
        UrlMacProvider = 8
    };
}
