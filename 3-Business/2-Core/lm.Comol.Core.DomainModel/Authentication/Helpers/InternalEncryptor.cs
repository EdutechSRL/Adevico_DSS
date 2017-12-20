using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace lm.Comol.Core.Authentication.Helpers
{
   
    public class InternalEncryptor
    {
        public enum HashMethod
        {
            MD5,
            SHA1,
            SHA384
        }

        //Variabili globali della classe
        //NB: le chiavi per la crittazione delle Pwd rimangono sempre uguali, 
        private byte[] TdesKey = {98,189,249,149,226,66,187,161,19,	177,198,77,	6,20,78,8,178,78,94,93,191,120,34,74};
        private byte[] TdesIV = {67,232,150,149,100,216,252,228};
        //Costruttore della classe
        public InternalEncryptor()
            : base()
        {
        }

        //function che mi restituisce la stringa in ingresso criptata
        //Secondo le mie chiavi 3des 
        //NB: le chiavi di crittazione rimangono fisse per tutte le crittaz.
        public string Encrypt(string InputString)
        {
            MemoryStream MyMemoryStream = new MemoryStream();
            MyMemoryStream.SetLength(0);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = TdesKey;
            tdes.IV = TdesIV;
            CryptoStream encStream = new CryptoStream(MyMemoryStream, tdes.CreateEncryptor(TdesKey, TdesIV), CryptoStreamMode.Write);
            encStream.Write(PlainStringToByteArray(InputString), 0, InputString.Length);
            encStream.Close();
            return EncryptedByteArrayToString(MyMemoryStream.ToArray());
        }

        //function che mi restituisce la stringa in ingresso decriptata
        //Secondo le mie chiavi 3des 
        public string Decrypt(string InputString)
        {

            MemoryStream MyMemoryStream = new MemoryStream();
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = TdesKey;
            tdes.IV = TdesIV;
            CryptoStream encStream = new CryptoStream(MyMemoryStream, tdes.CreateDecryptor(TdesKey, TdesIV), CryptoStreamMode.Write);
            byte[] EncArray = null;
            //(EncryptedStringToByteArray(StringIn).Length) as Byte
            EncArray = EncryptedStringToByteArray(InputString);
            int i = EncArray.Length;
            Array.Resize(ref EncArray, i - 1);
            encStream.Write(EncArray, 0, EncArray.Length);
            encStream.Close();
            return PlainByteArrayToString(MyMemoryStream.ToArray());
        }


        public bool Compare(string PlainString, string EncryptedString)
        {
            //function che mi verificadue stringhe, una in chiaro, fornita
            //dall'utente, e una crittata, le confronta e restituisce TRUE se sono
            //identiche, altrimenti FALSE. NB: il confronto viene effettuato sulla
            //stringa crittata 
            if (Encrypt(PlainString) == EncryptedString)
                return true;
            else
                return false;
        }

        private byte[] PlainStringToByteArray(string StringIn)
        {
            int i = 0;
            char[] caratteri = new char[StringIn.Length + 1];
            byte[] ByteArrayOut = new byte[StringIn.Length + 1];
            caratteri = StringIn.ToCharArray();
            for (i = 0; i <= (caratteri.Length - 1); i++)
            {
                ByteArrayOut[i] = Convert.ToByte(Convert.ToInt32(caratteri[i]));
            }
            return ByteArrayOut;
        }

        private string EncryptedByteArrayToString(byte[] ArrayIn)
        {
            int Caratt = 0;
            string StringOut = "";
            int i = 0;
            int NElements = ArrayIn.Length - 1;
            if (ArrayIn.Length != 0)
            {
                for (i = 0; i <= NElements; i++)
                {
                    Caratt = Convert.ToInt32(ArrayIn[i]);
                    StringOut += Caratt.ToString();
                    if (i != NElements)
                        StringOut += "-";
                }
            }
            else
                StringOut = "";
            return StringOut;
        }

        private byte[] EncryptedStringToByteArray(string StringIn)
        {
            string[] StringaArray = null;
            int i = 0;
            int NElementi = 0;
            StringaArray = StringIn.Split('-');
            NElementi = StringaArray.Length;
            byte[] MyByteArray = new byte[NElementi + 1];
            for (i = 0; i <= (NElementi - 1); i++)
            {
                MyByteArray[i] = Convert.ToByte(StringaArray[i]);
            }
            return MyByteArray;
        }

        private string PlainByteArrayToString(byte[] ArrayIn)
        {
            string StringOut = "";
            int i = 0;
            int NElements = ArrayIn.Length - 1;
            if (ArrayIn.Length != 0)
            {
                for (i = 0; i <= NElements; i++)
                {
                    StringOut += Convert.ToChar(ArrayIn[i]).ToString();
                }
            }
            else
                StringOut = "";
            return StringOut;
        }
        
        //public static string GeneraNumeriCasuali(int MaxChar)
        //{
        //    string cifra = "";
        //    try
        //    {
        //        int i = 0;
        //        int codice = 0;
        //        for (i = 1; i <= MaxChar; i++)
        //        {
        //            VBMath.Randomize();
        //            codice = Convert.ToInt32(Conversion.Int((9 * VBMath.Rnd())));
        //            cifra = cifra + codice;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cifra = "0";
        //    }

        //    return cifra;
        //}

        public static string GenerateHashDigest(string source, HashMethod algorithm)
        {
            HashAlgorithm hashAlgorithm = null;
            switch (algorithm)
            {
                case HashMethod.MD5:
                    hashAlgorithm = new MD5CryptoServiceProvider();
                    break;
                case HashMethod.SHA1:
                    hashAlgorithm = new SHA1CryptoServiceProvider();
                    break;
                case HashMethod.SHA384:
                    hashAlgorithm = new SHA384Managed();
                    break;
                default:
                    break;
            }

            byte[] byteValue = Encoding.UTF8.GetBytes(source);
            byte[] hashValue = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(hashValue);
        }

    }

}
