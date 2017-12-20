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
    public class dtoMacUrlProvider : dtoProvider
    {
        public virtual TimeSpan DeltaTime { get; set; }
        public virtual EncryptionInfo EncryptionInfo { get; set; }
        public virtual String SenderUrl { get; set; }
        public virtual String RemoteLoginUrl { get; set; }
        public virtual Boolean VerifyRemoteUrl { get; set; }
        public virtual String NotifySubscriptionTo { get; set; }
        public virtual Boolean NotifyTo { get { return !String.IsNullOrEmpty(NotifySubscriptionTo); } }

        public virtual IList<BaseUrlMacAttribute> Attributes { get; set; }
        public virtual Boolean AutoEnroll { get; set; }
        public virtual Boolean AutoAddAgency { get; set; }
        public virtual Boolean AllowTaxCodeDuplication { get; set; }
        public virtual String AllowRequestFromIpAddresses { get; set; }
        public virtual String DenyRequestFromIpAddresses { get; set; }

        public dtoMacUrlProvider()
        {
            EncryptionInfo = new EncryptionInfo();
            this.Type = Authentication.AuthenticationProviderType.UrlMacProvider;
            Attributes = new List<BaseUrlMacAttribute>();
        }
    }
}