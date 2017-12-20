using System;
using System.Collections.Generic;
using System.Text;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication.Helpers
{
    /// <summary>
    /// Classe che presenta tutti metodi Share/Static per la cifratura e decifratura.
    /// </summary>
    public class CryptoUtils
    {
        /// <summary>
        /// Separa i dati della stringa Decifrata, restituendo un Udt con i valori corretti.
        /// </summary>
        /// <param name="value">La stringa DECIFRATA.</param>
        /// <param name="Udt">La classe con Utente/Data</param>
        static void GetDateUser(string value, ref dtoUrlUserDateToken Udt, UrlUserTokenFormat format)
        {
            switch (format) {
                case UrlUserTokenFormat.PrefixDateTimeLogin:
                    GetDateUserPrefixDateTimeLogin(value, ref Udt);
                    break;
                case UrlUserTokenFormat.DateTimeLogin:
                    GetDateUserDateTimeLogin(value, ref Udt);
                    break;
                case UrlUserTokenFormat.LoginDateTime:
                    GetDateUserLoginDateTime(value, ref Udt);
                    break;
                default:
                    GetDateUserDateTimeLogin(value, ref Udt);
                    break;
            }     
        }

        static void GetDateUserPrefixDateTimeLogin(string value, ref dtoUrlUserDateToken Udt)
        {
            int firstSepIndex;
            firstSepIndex = value.IndexOf(" ");
            value = value.Remove(0, firstSepIndex + 1);
            int SeprationIndex = value.IndexOf(" ");
            int lenght = value.Length;
            int UserLenght;
            int DateLenght;
            DateLenght = SeprationIndex;
            UserLenght = lenght - DateLenght - 1;
            Udt.Login = value.Remove(0, DateLenght + 1);
            string date = value.Remove(SeprationIndex, UserLenght + 1);
            try
            {
                Udt.Data = DateTime.Parse(date);
            }
            catch (FormatException ex)
            {
                if (date.Contains("."))
                    Udt.Data = DateTime.Parse(date.Replace(".", ":"));
                else if (date.Contains(":"))
                    Udt.Data = DateTime.Parse(date.Replace(":", "."));
            }

        }

        static void GetDateUserDateTimeLogin(string value, ref dtoUrlUserDateToken Udt)
        {
            int firstSepIndex;
            if (value.StartsWith(" "))
             {
            firstSepIndex = value.IndexOf(" ");
            value = value.Remove(0, firstSepIndex + 1);
            }
            int SeprationIndex = value.IndexOf(" ");
            int lenght = value.Length;
            int UserLenght;
            int DateLenght;
            DateLenght = SeprationIndex;
            UserLenght = lenght - DateLenght - 1;
            Udt.Login = value.Remove(0, DateLenght + 1);
            string date = value.Remove(SeprationIndex, UserLenght + 1);
            try
            {
                Udt.Data = DateTime.Parse(date);
            }
            catch (FormatException ex)
            {
                if (date.Contains("."))
                    Udt.Data = DateTime.Parse(date.Replace(".", ":"));
                else if (date.Contains(":"))
                    Udt.Data = DateTime.Parse(date.Replace(":", "."));
            }

        }

        static void GetDateUserLoginDateTime(string value, ref dtoUrlUserDateToken Udt)
        {
            int SeprationIndex = value.IndexOf(" ");
            int lenght = value.Length;
            int UserLenght;
            int DateLenght;
            UserLenght  = SeprationIndex;
            DateLenght = lenght - UserLenght - 1;
            Udt.Login = value.Remove(0, UserLenght + 1);
            string date = value.Remove(SeprationIndex, DateLenght + 1);
            try
            {
                Udt.Data = DateTime.Parse(date);
            }
            catch (FormatException ex)
            {
                if (date.Contains("."))
                    Udt.Data = DateTime.Parse(date.Replace(".", ":"));
                else if (date.Contains(":"))
                    Udt.Data = DateTime.Parse(date.Replace(":", "."));
            }

        }


        /// <summary>
        /// Converte una stringa con data in formato ISO 8601 in datatime.
        /// </summary>
        /// <param name="ISOString">Stringa con data in formato ISO 8601.</param>
        /// <returns>Il datetime corrispondente</returns>
        public static DateTime FromISO8601ToDateTime(string ISOString)
        {
            return DateTime.Parse(ISOString);
        }

        /// <summary>
        /// Converte un DateTime in stringa formato ISO 8601
        /// </summary>
        /// <param name="Dt">DateTime da formattare</param>
        /// <returns>Stringa corretta</returns>
        public static string FromDataTimeToISO8610(DateTime Dt)
        {
            return Dt.ToString("s");
        }

        /// <summary>
        /// Effettua la decifratura della stringa.
        /// </summary>
        /// <param name="value">La stringa a base 64 da decifrare. VEDI REMARKS</param>
        /// <param name="alg">L'algoritmo utilizzato (Testato solo il Rijndael)</param>
        /// <param name="key">La chiave di cifratura, come stringa a base 64.</param>
        /// <param name="iv">Il vettore di cifratura, come stringa a base 64.</param>
        /// <returns>Una classe con UserName e Datetime corrispondente.</returns>
        /// <remarks>
        /// 1. value: gli spazi vengono convertiti in '+', a causa del passaggio via querystring che fa la conversione inversa. Html non fa una mazza.
        /// 2. numero iniziale: eventualmente il numero iniziale può essere utilizzato come ID dell'accesso, per non essere poi più utilizzato, purchè univoco. Al momento è inutilizzato.</remarks>
        /// 

        public static dtoUrlUserDateToken Decrypt(string token, EncryptionInfo encryptionInfo,UrlUserTokenFormat tokenFormat)
        {
            dtoUrlUserDateToken UdT = new dtoUrlUserDateToken();
            byte[] DecipherText;
            byte[] cypher;
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            string DecriptString = "";
            cypher = Convert.FromBase64String(token.Replace(" ", "+"));
            try
            {
                Decryptor dec = new Decryptor(encryptionInfo.EncryptionAlgorithm);
                DecipherText = dec.Decrypt(cypher, Convert.FromBase64String(encryptionInfo.Key), Convert.FromBase64String(encryptionInfo.InitializationVector));
                DecriptString = enc.GetString(DecipherText);
            }
            catch (Exception ex)
            {
                UdT.ExceptionString = "Message=" + ex.Message + "\n\r";
                if (ex.InnerException != null)
                    UdT.ExceptionString += "InnerException=" + ex.InnerException.ToString();
            }
            if (DecriptString != "")
            {
                GetDateUser(DecriptString, ref UdT, tokenFormat);
            }
            return UdT;
        }

        //public static dtoUrlUserDateToken Decrypt(string value, EncryptionAlgorithm alg, string key, string iv, UrlUserTokenFormat format)
        //{
        //    dtoUrlUserDateToken UdT = new dtoUrlUserDateToken();
        //    byte[] DecipherText;
        //    byte[] cypher;
        //    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        //    string DecriptString = "";
        //    cypher = Convert.FromBase64String(value.Replace(" ", "+"));
        //    try
        //    {
        //        Decryptor dec = new Decryptor(alg);
        //        DecipherText = dec.Decrypt(cypher, Convert.FromBase64String(key), Convert.FromBase64String(iv));
        //        DecriptString = enc.GetString(DecipherText);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    if (DecriptString != "")
        //    {
        //        GetDateUser(DecriptString, ref UdT,format);
        //    }
        //    return UdT;
        //}

        /// <summary>
        /// Cifra i dati necessari secondo gli accordi con IT/IOP
        /// </summary>
        /// <param name="UdT">UserName e TimeStamp</param>
        /// <param name="Num">Numero (in stringa) anteposto alla stringa da criptare. Gli spazi sono convertiti in 0.</param>
        /// <param name="alg">L'algoritmo utilizzato. (Testato solo il Rijndael)</param>
        /// <param name="key">Chiave di cifratura, come stringa in base 64.</param>
        /// <param name="iv">Vettore di cifratura, come stringa in base 64.</param>
        /// <returns>Stringa cifrata in base 64.</returns>
        public static string Crypt(dtoUrlUserDateToken UdT, string Num, EncryptionAlgorithm alg, string key, string iv)
        { 
            String OutStr = "";
            String UserStr = "";
            UserStr = Num.Replace(" ", "0");
            UserStr += " ";
            UserStr += FromDataTimeToISO8610(UdT.Data);
            UserStr += " ";
            UserStr += UdT.Login;

            System.Text.ASCIIEncoding Encoder = new System.Text.ASCIIEncoding();
            byte[] _Key;
            byte[] _Iv;

            _Key = Convert.FromBase64String(key);
            _Iv = Convert.FromBase64String(iv);


            byte[] cipherText;
                        
            try
            {
                Encryptor enc = new Encryptor(alg);
                byte[] plainText = Encoding.ASCII.GetBytes(UserStr);
                cipherText = enc.Encrypt(plainText, _Key, _Iv);
                OutStr = Convert.ToBase64String(cipherText);
            }
            catch (Exception ex)
            {
                OutStr = "";
            }
            
            return OutStr;
        }


        public static string Crypt(String value, EncryptionInfo encryptionInfo)
        {
            String OutStr = "";
       
            System.Text.ASCIIEncoding Encoder = new System.Text.ASCIIEncoding();
            switch (encryptionInfo.EncryptionAlgorithm)
            {
                case EncryptionAlgorithm.None:
                    return value;
                case EncryptionAlgorithm.Md5:
                    System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value + (String.IsNullOrEmpty(encryptionInfo.Key) ? "1q2w3e4r5t6y7u8i9o0p'èì+" : encryptionInfo.Key)));
                    //byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(value+ encryptionInfo.Key);
                    //byte[] hash = md5.ComputeHash(inputBytes);
 
                    // step 2, convert byte array to hex string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sb.Append(data[i].ToString("x2"));
                    }
                    OutStr = sb.ToString();
                    break;
                default:
                    byte[] _Key;
                    byte[] _Iv;

                    _Key = Convert.FromBase64String(encryptionInfo.Key);
                    _Iv = Convert.FromBase64String(encryptionInfo.InitializationVector);


                    byte[] cipherText;

                    try
                    {
                        Encryptor enc = new Encryptor(encryptionInfo.EncryptionAlgorithm);
                        byte[] plainText = Encoding.ASCII.GetBytes(value);
                        cipherText = enc.Encrypt(plainText, _Key, _Iv);
                        OutStr = Convert.ToBase64String(cipherText);
                    }
                    catch (Exception ex)
                    {
                        OutStr = "";
                    }
                    break;
            }
            return OutStr;
        }
        public static string DecryptValue(string value, EncryptionInfo encryptionInfo)
        {
            byte[] DecipherText;
            byte[] cypher;
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            string DecriptString = "";
            cypher = Convert.FromBase64String(value.Replace(" ", "+"));
            try
            {
                Decryptor dec = new Decryptor(encryptionInfo.EncryptionAlgorithm);
                DecipherText = dec.Decrypt(cypher, Convert.FromBase64String(encryptionInfo.Key), Convert.FromBase64String(encryptionInfo.InitializationVector));
                DecriptString = enc.GetString(DecipherText);
            }
            catch (Exception ex)
            {
            }
            return DecriptString;
        }
    }

    /// <summary>
    /// La classe usata per contenere Login utente e TimeStamp.
    /// </summary>
   
}
