using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class liteGenericEncryption : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual MacType Type { get; set; }
        public virtual lm.Comol.Core.Authentication.Helpers.EncryptionInfo EncryptionInfo { get; set; }
        public virtual String ExternalIdentifier { get; set; }
        public virtual Boolean VerifyTimeout { get; set; }

        public virtual String Encrypt(params object [] items){
            String mac = String.Join("", items.Select(i => i.ToString()));
            return CryptoUtils.Crypt(mac, EncryptionInfo);
        }
        public virtual Boolean Validate(String mac, params object[] items)
        {
            try
            {
                String toValidate = String.Join("", items.Select(i => i.ToString()));
                return mac == CryptoUtils.Crypt(toValidate, EncryptionInfo);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}