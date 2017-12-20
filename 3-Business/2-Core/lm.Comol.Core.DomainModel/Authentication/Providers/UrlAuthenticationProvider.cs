using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class UrlAuthenticationProvider : AuthenticationProvider
    {
        public virtual String UrlIdentifier {get;set;}
        public virtual TimeSpan DeltaTime { get; set; }
        public virtual UrlUserTokenFormat TokenFormat { get; set; }
        public virtual EncryptionInfo EncryptionInfo { get; set; }
        public virtual String SenderUrl { get; set; }
        public virtual String RemoteLoginUrl { get; set; }
        public virtual IList<LoginFormat> LoginFormats { get; set; }
        public virtual Boolean VerifyRemoteUrl { get; set; }
        public virtual String NotifySubscriptionTo { get; set; }
        public virtual Boolean NotifyTo { get { return !String.IsNullOrEmpty(GetNotificationAddresses()); } }
        public virtual String GetNotificationAddresses()
        {
            if (String.IsNullOrEmpty(NotifySubscriptionTo))
                return "";
            else if (NotifySubscriptionTo.Contains(";") || NotifySubscriptionTo.Contains(","))
            {
                String result = "";
                Char c = (NotifySubscriptionTo.Contains(",") ? ',' : ';');
                List<String> addresses = NotifySubscriptionTo.Split(c).ToList();
                foreach (String address in addresses.Where(a => ValidationHelpers.Mail(a)).ToList())
                {
                    result += address + ";";
                }
                return result;
            }
            else
                return (ValidationHelpers.Mail(NotifySubscriptionTo)) ? NotifySubscriptionTo : "";
        }

        public virtual UrlProviderResult ValidateToken(String token, String fromUrl )
        {
            if (!VerifyRemoteUrl)
                return ValidateToken(token);
            else if (fromUrl.ToLower().StartsWith(SenderUrl.ToLower()))
                return ValidateToken(token);
            else
                return UrlProviderResult.InvalidToken;
        }
        public virtual UrlProviderResult ValidateToken(String token, String fromUrl, DateTime dateTimeToVerify)
        {
            if (!VerifyRemoteUrl)
                return ValidateToken(token, dateTimeToVerify);
            else if (fromUrl.ToLower().StartsWith(SenderUrl.ToLower()))
                return ValidateToken(token, dateTimeToVerify);
            else
                return UrlProviderResult.InvalidToken;
        }
        public virtual UrlProviderResult ValidateToken(String token)
        {
            UrlProviderResult result = UrlProviderResult.NotEvaluatedToken;
            try
            {
                dtoUrlUserDateToken dto = DecryptToken(token);
                if (dto != null)
                {
                    if (DateTime.Now - dto.Data <= DeltaTime)
                        result = UrlProviderResult.ValidToken;
                    else
                        result = UrlProviderResult.ExpiredToken;
                }
                else
                    result = UrlProviderResult.InvalidToken;
            }
            catch (Exception ex)
            {
                result = UrlProviderResult.InvalidToken;

            }
            return result;
        }
        public virtual dtoUrlToken ValidateToken(dtoUrlToken urlToken)
        {
            dtoUrlToken item = new dtoUrlToken() { Identifier = urlToken.Identifier, Value = urlToken.Value };
            dtoUrlUserDateToken dto = null;
            DateTime currentDate = DateTime.Now;
            try
            {
                item.DecriptedValue = GetTokenIdentifier(urlToken.Value);
                dto = DecryptToken(urlToken.Value);
                if (dto != null)
                {
                    if (currentDate - dto.Data <= DeltaTime)
                        item.Evaluation.Result = UrlProviderResult.ValidToken;
                    else
                        item.Evaluation.Result = UrlProviderResult.ExpiredToken;
                }
                else
                    item.Evaluation.Result = UrlProviderResult.InvalidToken;
            }
            catch (Exception ex)
            {
                item.Evaluation.Result = UrlProviderResult.InvalidToken;
                if (dto != null)
                    item.Evaluation.TokenException = dto.ExceptionString;
                item.SetException(ex);
            }
            if (item.Evaluation.Result != UrlProviderResult.ValidToken)
                item.Evaluation.FullDecriptedValue = lm.Comol.Core.Authentication.Helpers.CryptoUtils.DecryptValue(urlToken.Value, EncryptionInfo);
            if (item.Evaluation.Result == UrlProviderResult.ExpiredToken) {
                item.Evaluation.ExpiredMessage = "currentDate=" + currentDate.ToString() + "\n\rdto.Data= " + dto.Data.ToString()
                    + "\n\rDeltaTime (" + DeltaTime.Milliseconds + ") =" + DeltaTime.ToString() + "\n\r(currentDate - dto.Data) =" + (currentDate - dto.Data).ToString();
            }
                
            return item;
        }
        public virtual UrlProviderResult ValidateToken(String token, DateTime dateTimeToVerify)
        {
            UrlProviderResult result = UrlProviderResult.NotEvaluatedToken;
            try
            {
                dtoUrlUserDateToken dto = DecryptToken(token);
                if (dto != null)
                {
                    TimeSpan dateDifference = dateTimeToVerify.Subtract(dto.Data);
                    if (dateDifference <= DeltaTime)
                        result = UrlProviderResult.ValidToken;
                    else
                        result = UrlProviderResult.ExpiredToken;
                }
                else
                    result = UrlProviderResult.InvalidToken;
            }
            catch (Exception ex)
            {
                result = UrlProviderResult.InvalidToken;
            }
            return result;
        }
        public virtual String GetTokenIdentifier(String token)
        {
            String result = "";
            try
            {
                dtoUrlUserDateToken dto = DecryptToken(token);
                if (dto != null && (LoginFormats == null || LoginFormats.Count==0))
                    result = dto.Login;
                else if (dto != null) {
                    LoginFormat format = (from f in LoginFormats
                                          where f.Deleted == BaseStatusDeleted.None && ((!string.IsNullOrEmpty(f.Before) && dto.Login.StartsWith(f.Before)) || (!string.IsNullOrEmpty(f.After) && dto.Login.EndsWith(f.After)))
                                          select f).FirstOrDefault();
                    result = dto.Login;
                    if (format != null && !string.IsNullOrEmpty(format.Before))
                        result = result.Replace(format.Before, "");
                    if (format != null && !string.IsNullOrEmpty(format.After))
                        result = result.Replace(format.After, "");
                }
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }
        //public virtual String FullDecryptToken(String token)
        //{
        //    String result = "";
        //    try
        //    {
        //        dtoUrlUserDateToken dto = DecryptToken(token);
        //        if (dto != null && EncryptionInfo != null)
        //            result = lm.Comol.Core.Authentication.Helpers.CryptoUtils.DecryptValue(token, EncryptionInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = "";
        //    }
        //    return result;
        //}
        private dtoUrlUserDateToken DecryptToken(String token)
        {
            dtoUrlUserDateToken result = null;
            if (EncryptionInfo != null)
                result = CryptoUtils.Decrypt(token, EncryptionInfo, TokenFormat);
            return result;
        }

        public UrlAuthenticationProvider()
        {
           this.LogoutMode = Authentication.LogoutMode.externalPage;
           this.ProviderType =  AuthenticationProviderType.Url;
        }
         
    } 
}