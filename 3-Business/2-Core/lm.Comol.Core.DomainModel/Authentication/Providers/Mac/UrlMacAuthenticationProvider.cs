using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class MacUrlAuthenticationProvider : AuthenticationProvider
    {
        public virtual IList<BaseUrlMacAttribute> Attributes { get; set; }
        public virtual TimeSpan DeltaTime { get; set; }
        public virtual EncryptionInfo EncryptionInfo { get; set; }
        public virtual String SenderUrl { get; set; }
        public virtual String RemoteLoginUrl { get; set; }
        public virtual Boolean VerifyRemoteUrl { get; set; }
        public virtual String NotifySubscriptionTo { get; set; }
        public virtual Boolean NotifyTo { get { return !String.IsNullOrEmpty(GetNotificationAddresses()); } }
        public virtual Boolean AutoEnroll { get; set; }
        public virtual Boolean AutoAddAgency { get; set; }
        public virtual Boolean AllowTaxCodeDuplication { get; set; }
        public virtual String AllowRequestFromIpAddresses { get; set; }
        public virtual String DenyRequestFromIpAddresses { get; set; }
        public virtual Boolean HasCatalogues()
        {
            return Attributes != null && Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == UrlMacAttributeType.coursecatalogue).Any();
        }
        public virtual String GetNotificationAddresses()
        {
            if (String.IsNullOrEmpty(NotifySubscriptionTo))
                return "";
            else if (NotifySubscriptionTo.Contains(";") || NotifySubscriptionTo.Contains(","))
            {
                String result = "";
                Char c = (NotifySubscriptionTo.Contains(",") ? ',' : ';');
                List<String> addresses = NotifySubscriptionTo.Split(c).ToList();
                foreach (String address in addresses.Where(a => ValidationHelpers.Mail(a)).ToList()) {
                    result += address + ";";
                }
                return result;
            }
            else
                return (ValidationHelpers.Mail(NotifySubscriptionTo)) ? NotifySubscriptionTo : "";
        }
        public MacUrlAuthenticationProvider() {
            AutoEnroll = true;
            VerifyRemoteUrl = false;
            DeltaTime = new TimeSpan(0, 3, 0);
            Attributes = new List<BaseUrlMacAttribute>();
            LogoutMode = Authentication.LogoutMode.externalPage;
            ProviderType =  AuthenticationProviderType.UrlMacProvider;
        }

        public virtual List<dtoMacUrlUserAttribute> GetUserAttributes()
        {
            List<dtoMacUrlUserAttribute> attributes = new List<dtoMacUrlUserAttribute>();
            if (Attributes != null && Attributes.Where(a => a.Deleted == BaseStatusDeleted.None).Any()){
                attributes = Attributes.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => new dtoMacUrlUserAttribute()
                {
                    Id = a.Id,
                    QueryName = a.QueryStringName,
                    Type = a.Type
                }).ToList();

                foreach (CompositeProfileAttribute cAttribute in GetCompositeAttributes())
                {
                    dtoMacUrlUserAttribute tAttribute = attributes.Where(a=> a.Id==cAttribute.Id).FirstOrDefault();
                    tAttribute.CharComposer= cAttribute.MultipleValueSeparator;
                    tAttribute.ComposedBy = cAttribute.Items.Where(i => i.Deleted == BaseStatusDeleted.None).OrderBy(i => i.DisplayOrder).Select(i => new dtoComposerAttribute() { Attribute = attributes.Where(a => a.Id == i.Attribute.Id).FirstOrDefault(), DisplayOrder = i.DisplayOrder }).ToList();
                }
                long iAttribute = 0;
                UserProfileAttribute pAttribute = GetProfileAttributes().Where(a => a.Attribute == ProfileAttributeType.externalId).FirstOrDefault();
                if (pAttribute != null)
                    iAttribute = pAttribute.Id;
                else {
                    CompositeProfileAttribute cAttribute = GetCompositeAttributes().Where(a => a.Attribute == ProfileAttributeType.externalId).FirstOrDefault();
                    if (cAttribute != null)
                        iAttribute = cAttribute.Id;
                }

                foreach (dtoMacUrlUserAttribute a in attributes.Where(i => i.Id == iAttribute).ToList())
                {
                    a.isIdentifier = true;
                }
            }
            return attributes;
        }
        public virtual List<UserProfileAttribute> GetProfileAttributes()
        {
            return Attributes.Where(p => p.Deleted == BaseStatusDeleted.None && p.GetType() == typeof(UserProfileAttribute)).Select(p => (UserProfileAttribute)p).ToList();
        }
        public virtual List<CompositeProfileAttribute> GetCompositeAttributes()
        {
            return Attributes.Where(p => p.Deleted == BaseStatusDeleted.None && p.GetType() == typeof(CompositeProfileAttribute)).Select(p => (CompositeProfileAttribute)p).ToList();
        }
       

        protected virtual Boolean isValidIpAddress(String ipAddress, String proxyIpAddress)
        {
            return isValidIpAddress(ipAddress) || isValidIpAddress(proxyIpAddress);
        }
        protected virtual Boolean isValidIpAddress(String ipAddress)
        {
            Boolean result = String.IsNullOrEmpty(DenyRequestFromIpAddresses) && String.IsNullOrEmpty(AllowRequestFromIpAddresses);
            if (!result) {
                if (ipAddress == "::1")
                    ipAddress = "127.0.0.1";
                List<String> aRange= (String.IsNullOrEmpty(AllowRequestFromIpAddresses)) ? new List<String>() : AllowRequestFromIpAddresses.Split(',').ToList();
                List<String> dRange= (String.IsNullOrEmpty(DenyRequestFromIpAddresses)) ? new List<String>() : DenyRequestFromIpAddresses.Split(',').ToList();
                
                Boolean denyed = (dRange.Any() && (IpAdressHelpers.CheckIsInRange(dRange, ipAddress)));
                result = (!denyed && (!aRange.Any() || (aRange.Any() && IpAdressHelpers.CheckIsInRange(aRange, ipAddress))));
            }
            return result;
        }
        public virtual dtoMacUrlToken ValidateToken(List<dtoMacUrlUserAttribute> attributes, String fromUrl, String ipAddress, String proxyIpAddress)
        {
            if (!VerifyRemoteUrl || fromUrl.ToLower().StartsWith(SenderUrl.ToLower()))
                return ValidateToken(attributes, ipAddress, proxyIpAddress);
            else
                return new dtoMacUrlToken() { Evaluation = new dtoMacUrlTokenEvaluation() { Result = UrlProviderResult.InvalidToken } };
        }
        public virtual dtoMacUrlToken ValidateToken(List<dtoMacUrlUserAttribute> attributes,String ipAddress,String proxyIpAddress)
        {
            dtoMacUrlToken item = new dtoMacUrlToken() { Attributes = attributes };
            DateTime currentDate = DateTime.Now;
            DateTime? tokenTimeStamp = null;
            TimestampAttribute timeAttribute = null;
            String calculatedMac = "";
            try
            {
                if (isValidIpAddress(ipAddress,proxyIpAddress)){
                    if (ValidateRequiredAttributes(attributes) && ValidateMacAttribute(attributes, ref calculatedMac))
                    {
                        timeAttribute = (TimestampAttribute)Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == UrlMacAttributeType.timestamp).FirstOrDefault();
                        if (timeAttribute!=null){
                            tokenTimeStamp = timeAttribute.GetDate(attributes.Where(a => a.Id == timeAttribute.Id).Select(a => a.QueryValue).FirstOrDefault());
                            if (tokenTimeStamp.HasValue)
                            {
                                if (currentDate - tokenTimeStamp.Value <= DeltaTime)
                                    item.Evaluation.Result = UrlProviderResult.ValidToken;
                                else
                                    item.Evaluation.Result = UrlProviderResult.ExpiredToken;
                            }
                            else
                                item.Evaluation.Result = UrlProviderResult.InvalidToken;
                        }
                        else
                            item.Evaluation.Result = UrlProviderResult.ValidToken;
                        if (item.Evaluation.Result == UrlProviderResult.ValidToken)
                            item.InternalMac = GetInternalMac(attributes);
                    }
                    else
                        item.Evaluation.Result = UrlProviderResult.InvalidToken;
                }
                else{
                    item.Evaluation.Result = UrlProviderResult.InvalidIpAddress;
                }
            }
            catch (Exception ex)
            {
                item.Evaluation.Result = UrlProviderResult.InvalidToken;
                item.SetException(ex);
            }
            item.Evaluation.Mac = calculatedMac;
            if (item.Evaluation.Result == UrlProviderResult.ExpiredToken && timeAttribute !=null )
            {
                item.Evaluation.ExpiredMessage = (tokenTimeStamp.HasValue) ? "currentDate=" + currentDate.ToString() + "\n\rdto.Data= " + tokenTimeStamp.Value.ToString() 
                    + "\n\rDeltaTime (" + DeltaTime.Milliseconds + ") =" + DeltaTime.ToString() + "\n\r(currentDate - tokenTimeStamp.Value) =" + (currentDate - tokenTimeStamp.Value).ToString() : "currentDate=" + currentDate.ToString();
            }

            return item;
        }
        public virtual Boolean IsInternalToken(String internalMac, List<dtoMacUrlUserAttribute> attributes)
        {
            return (internalMac==GetInternalMac(attributes));
        }

        private Boolean ValidateMacAttribute(List<dtoMacUrlUserAttribute> attributes, ref String calculatedMac) { 
            Boolean isValid = false;
            String mac= "";
            MacAttribute macAttribute = (MacAttribute)Attributes.Where(a=> a.Deleted== BaseStatusDeleted.None && a.Type== UrlMacAttributeType.mac).FirstOrDefault();
            if (macAttribute != null && macAttribute.Items.Any())
            {
                mac = String.Join("",
                    macAttribute.Items.Where(i => i.Deleted == BaseStatusDeleted.None).OrderBy(i => i.DisplayOrder).ToList().Select(i => attributes.Where(a => a.Id == i.Attribute.Id).Select(a => a.QueryValue).FirstOrDefault()).ToArray());

                calculatedMac = CryptoUtils.Crypt(mac, EncryptionInfo);
                isValid = (calculatedMac == attributes.Where(a => a.Type == UrlMacAttributeType.mac).Select(a => a.QueryValue).FirstOrDefault());
            }
            else
                isValid = true;
            return isValid;
        }
        private Boolean ValidateRequiredAttributes(List<dtoMacUrlUserAttribute> attributes)
        {
            Boolean isValid = true;
            if (attributes.Where(a => a.Type == UrlMacAttributeType.compositeProfile && a.isIdentifier).Any()) {
                isValid = !attributes.Where(a => a.Type == UrlMacAttributeType.compositeProfile && a.isIdentifier && a.ComposedBy.Where(c => c.Attribute.isEmpty).Any()).Any();
            }
            if (isValid && attributes.Where(a=>a.Type== UrlMacAttributeType.profile).Any()) {
                isValid = ValidateBaseProfileAttributes(attributes);
            }
            
            return isValid;
        }
        private Boolean ValidateBaseProfileAttributes(List<dtoMacUrlUserAttribute> attributes)
        {
            Boolean isValid = true;
            List<UserProfileAttribute> items = GetProfileAttributes();
            isValid = ValidateProfileAttributes(items.Where(i => i.Attribute == ProfileAttributeType.name).ToList(), attributes)
                    && ValidateProfileAttributes(items.Where(i => i.Attribute == ProfileAttributeType.surname).ToList(), attributes);
            return isValid;
        }
        private Boolean ValidateProfileAttributes(List<UserProfileAttribute> pAttributes,List<dtoMacUrlUserAttribute> attributes)
        {
            Boolean isValid = true;
            foreach (UserProfileAttribute p in pAttributes) {
                isValid &= !attributes.Where(a => a.Id == p.Id && a.isEmpty).Any();
                if (!isValid)
                    return isValid;
            }
            return isValid;
        }
        private String GetInternalMac(List<dtoMacUrlUserAttribute> attributes)
        {
            String mac = mac = String.Join("",
                    Attributes.Where(i => i.Deleted == BaseStatusDeleted.None).OrderBy(i => i.Id).ToList().Select(i => attributes.Where(a => a.Id == i.Id).Select(a => a.QueryValue).FirstOrDefault()).ToArray()) + "InternalMac";

            return CryptoUtils.Crypt(mac, EncryptionInfo);
        }

        public virtual List<OrganizationAttributeItem> GetOrganizationsInfo(List<dtoMacUrlUserAttribute> attributes)
        {
            List<OrganizationAttributeItem> result = new List<OrganizationAttributeItem>();
            List<OrganizationAttribute> oAttributes = Attributes.Where(p => p.Deleted == BaseStatusDeleted.None && p.GetType() == typeof(OrganizationAttribute)).Select(p => (OrganizationAttribute)p).ToList();

            foreach (OrganizationAttribute att in oAttributes)
            {
                foreach (OrganizationAttributeItem item in att.Items.Where(i => i.Deleted == BaseStatusDeleted.None).ToList())
                {
                    if (attributes.Where(a => a.Id == att.Id && a.QueryValue == item.RemoteCode).Any())
                        result.Add(item);
                }
            }
            return result;
        }
        public virtual String GetAttributeValue(ProfileAttributeType att, List<UserProfileAttribute> pAttributes, List<dtoMacUrlUserAttribute> uAttributes)
        {
            return uAttributes.Where(a => a.Id == pAttributes.Where(p => p.Attribute == att).Select(p => p.Id).FirstOrDefault()).Select(a => a.QueryValue).FirstOrDefault();
        }
        public virtual String GetAttributeValue(ProfileAttributeType att, List<dtoMacUrlUserAttribute> uAttributes)
        {
            String result =  uAttributes.Where(a => a.Id == GetProfileAttributes().Where(p => p.Attribute == att).Select(p => p.Id).FirstOrDefault()).Select(a => a.QueryValue).FirstOrDefault();
            if (String.IsNullOrEmpty(result) && GetCompositeAttributes().Any())
                result = uAttributes.Where(a => a.Id == GetCompositeAttributes().Where(p => p.Attribute == att).Select(p => p.Id).FirstOrDefault()).Select(a => a.QueryValue).FirstOrDefault();
            return result;
        }
        public virtual List<lm.Comol.Core.Authentication.ProfileAttributeType> GetNotEditableAttributes(List<dtoMacUrlUserAttribute> uAttributes)
        {
            List<UserProfileAttribute> pAttributes = GetProfileAttributes();
            return pAttributes.Where(p => uAttributes.Where(u => u.Type == UrlMacAttributeType.profile && !String.IsNullOrEmpty(u.QueryValue)).Select(u => u.Id).ToList().Contains(p.Id)).Select(p => p.Attribute).ToList();
        }
    } 
}