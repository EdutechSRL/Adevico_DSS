using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class InternalAuthenticationProvider : AuthenticationProvider
    {
        public virtual Int32 ChangePasswordAfterDays { get; set; }

        public InternalAuthenticationProvider()
        {
            this.LogoutMode = Authentication.LogoutMode.internalLogonPage;
            this.ProviderType = AuthenticationProviderType.Internal;
        }

        public virtual String GetNewPassword(){
            return GetNewPassword(8, 10);
        }
        public virtual String GetNewPassword(int minLength, int maxLength)
        {
            return lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(minLength, maxLength, true, true, false);
        }

        public virtual String GetEncryptedPassword()
        {
            return GetEncryptedPassword(8, 10);
        }
        public virtual String GetEncryptedPassword(int minLength, int maxLength)
        {
            String password = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(8, 10, true, true, false);
            return EncryptPassword(password);
        }
        public virtual String EncryptPassword(string password)
        {
            InternalEncryptor encryptor = new InternalEncryptor();
            return encryptor.Encrypt(password);
        }
        public virtual Boolean VerifyPassword(string blank, string encrypted)
        {
            InternalEncryptor encryptor = new InternalEncryptor();
            return (encryptor.Encrypt(blank) == encrypted );
        }
    } 
}