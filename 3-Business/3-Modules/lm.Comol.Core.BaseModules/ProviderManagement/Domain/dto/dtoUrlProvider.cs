using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    [Serializable]
    public class dtoUrlProvider : dtoProvider
    {
        public virtual String UrlIdentifier { get; set; }
        public virtual TimeSpan DeltaTime { get; set; }
        public virtual UrlUserTokenFormat TokenFormat { get; set; }
        public virtual EncryptionInfo EncryptionInfo { get; set; }
        public virtual String SenderUrl { get; set; }
        public virtual String RemoteLoginUrl { get; set; }
        public virtual Boolean VerifyRemoteUrl { get; set; }
        public virtual String NotifySubscriptionTo { get; set; }
        public virtual Boolean NotifyTo { get { return !String.IsNullOrEmpty(NotifySubscriptionTo); } }
        public virtual List<dtoLoginFormat> LoginFormats { get; set; }
        public dtoUrlProvider() {
            EncryptionInfo = new EncryptionInfo();
            LoginFormats = new List<dtoLoginFormat>();
            this.Type = Authentication.AuthenticationProviderType.Url;
        }
    }
}