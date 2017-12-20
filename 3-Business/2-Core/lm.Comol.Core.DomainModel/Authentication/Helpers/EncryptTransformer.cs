using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace lm.Comol.Core.Authentication.Helpers
{
    internal class EncryptTransformer
    {
        private EncryptionAlgorithm algorithmID;
        //private byte[] initVec;
        private byte[] encKey;

        internal byte[] IV {get;set;}

        internal byte[] Key
        {
            get 
            { 
                return encKey; 
            }
        }

        internal EncryptTransformer(EncryptionAlgorithm algId)
        {
          //Save the algorithm being used.
          algorithmID = algId;
        }

        internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey, byte[] initVec)
        {
            // Pick the provider.
            switch (algorithmID)
            {
                case EncryptionAlgorithm.Des:
                {
                    DES des = new DESCryptoServiceProvider();
                    des.Mode = CipherMode.CBC;

                    // See if a key was provided
                    if (null == bytesKey)
                        encKey = des.Key;
                    else
                    {
                        des.Key = bytesKey;
                        encKey = des.Key;
                    }
                    // See if the client provided an initialization vector
                    if (null == initVec)
                    { // Have the algorithm create one
                        initVec = des.IV;
                    }
                    else
                    { //No, give it to the algorithm
                        des.IV = initVec;                        
                    }
                    return des.CreateEncryptor();
                }
                case EncryptionAlgorithm.TripleDes:
                {
                    TripleDES des3 = new TripleDESCryptoServiceProvider();
                    des3.Mode = CipherMode.CBC;
                    // See if a key was provided
                    if (null == bytesKey)
                        encKey = des3.Key;
                    else
                    {
                        des3.Key = bytesKey;
                        encKey = des3.Key;
                    }
                    // See if the client provided an IV
                    if (null == initVec)
                    { //Yes, have the alg create one
                        initVec = des3.IV;
                    }
                    else
                    { //No, give it to the alg.
                        des3.IV = initVec;
                    }
                    return des3.CreateEncryptor();
                }
                case EncryptionAlgorithm.Rc2:
                {
                    RC2 rc2 = new RC2CryptoServiceProvider();
                    rc2.Mode = CipherMode.CBC;
                    // Test to see if a key was provided
                    if (null == bytesKey)
                        encKey = rc2.Key;
                    else
                    {
                        rc2.Key = bytesKey;
                        encKey = rc2.Key;
                    }
                    // See if the client provided an IV
                    if (null == initVec)
                    { //Yes, have the alg create one
                        initVec = rc2.IV;
                    }
                    else
                    { //No, give it to the alg.
                        rc2.IV = initVec;
                    }
                    return rc2.CreateEncryptor();
                }
                case EncryptionAlgorithm.Rijndael:
                {
                    Rijndael rijndael = new RijndaelManaged();
                    rijndael.Mode = CipherMode.CBC;
                    // Test to see if a key was provided
                    if (null == bytesKey)
                        encKey = rijndael.Key;
                    else
                    {
                        rijndael.Key = bytesKey;
                        encKey = rijndael.Key;
                    }
                    // See if the client provided an IV
                    if (null == initVec)
                    { //Yes, have the alg create one
                        initVec = rijndael.IV;
                    }
                    else
                    { //No, give it to the alg.
                        rijndael.IV = initVec;
                    }
                    return rijndael.CreateEncryptor();
                }
               default:
                {
                    throw new CryptographicException("Algorithm ID '" + algorithmID +"' not supported.");
                }
            }
        }


    }
}