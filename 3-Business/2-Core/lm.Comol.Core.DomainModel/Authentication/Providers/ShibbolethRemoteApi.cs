using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;
using System.Net;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class ShibbolethRemoteApi
    {
        public virtual Boolean IsActive { get; set; }
        public virtual Boolean AllowEditMail { get; set; }
        public virtual String RemoteEditMailUrl { get; set; }
        public virtual String RemoteEditMailUrlHelp { get; set; }
        public virtual String RemoteApiUrl { get; set; }
        public virtual TimestampFormat Format { get; set; }
        public virtual String SecretKey { get; set; }
        public virtual IList<JsonAttribute> Attributes { get; set; }
        public ShibbolethRemoteApi()
        {
            Format = TimestampFormat.aaaammgghhmmss; 
        }


        public virtual Dictionary<ProfileAttributeType,String> GetAttributes(String identifyer)
        {
            Dictionary<ProfileAttributeType, String> attributes = new Dictionary<ProfileAttributeType, String>();
            System.Security.Cryptography.SHA256 sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            Byte[] inputBytes;
            Byte[] outputBytes;
            string outputString;
            string data =""; 
            switch(Format){
                case TimestampFormat.aaaammgghhmmss:
                    data = DateTime.Now.ToString("yyyyMMddHHmm");
                    break;
            }
                
            // Calcolo della chiave di hash
         
            inputBytes = ASCIIEncoding.Default.GetBytes(identifyer + data + SecretKey);
            outputBytes = sha256.ComputeHash(inputBytes);
            outputString = Convert(outputBytes);
            String url = RemoteApiUrl;
            if (RemoteApiUrl.Contains("{0}") && RemoteApiUrl.Contains("{1}"))
            {
                try
                {
                    WebClient webClient = new WebClient();
                    string json = webClient.DownloadString(String.Format(RemoteApiUrl, identifyer, outputString));
                    if (!String.IsNullOrEmpty(json) && json !="{\"result\":\"\"}")
                    {
                        var deserializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var result = (Dictionary<String, object>)deserializer.DeserializeObject(json);
                        if (result != null && Attributes != null && Attributes.Where(a => a.Deleted == BaseStatusDeleted.None).Any())
                        {
                            foreach (Dictionary<String, object> jStruct in result.Values.Where(v=> v !=null))
                            {
                                foreach (String key in jStruct.Keys)
                                {
                                    JsonAttribute attribute = Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Name == key.ToLower()).FirstOrDefault();
                                    if (attribute != null)
                                        attributes.Add(attribute.Type, jStruct[key].ToString());
                                }
                            }
                        }
                    }
                }
                catch (ArgumentException e)
                {
                }            
            }
            return attributes;
        }

        static string Convert(Byte[] outputBytes)
        {
            StringBuilder sb = new StringBuilder(256);
            sb.Clear();
            foreach (var b in outputBytes)
            {
                sb.Append(String.Format("{0:x2}", b));
            }
            return sb.ToString();
        }
    } 
}