using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace RSA_CryptLib
{
    public delegate void LogMethod(string message);

    public class RSAWrapper
    {


        #region --VARS--
        RSACryptoServiceProvider rsa;
        public LogMethod Log;
        RSAParameters privateKey;
        RSAParameters publicKey;
        #endregion

        #region --PROPS--

        

        public RSAParameters PrivateKey
        {
            get { return privateKey; }
            set { privateKey = value; }
        }


        public RSAParameters PublicKey
        {
            get { return publicKey; }
            set { publicKey = value; }
        }


        public string PublicKeyString
        {
            get
            {
                using (var sw = new StringWriter())
                {
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    xs.Serialize(sw, this.publicKey);
                    return sw.ToString();
                }
            }
            set
            {
                using (var sr = new StringReader(value))
                {
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    this.publicKey = (RSAParameters)xs.Deserialize(sr);
                }
            }
        }
        public string PrivateKeyString
        {
            get
            {
                using (var sw = new StringWriter())
                {
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    xs.Serialize(sw, this.privateKey);
                    return sw.ToString();
                }
            }
            set
            {
                using (var sr = new StringReader(value))
                {
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    this.privateKey = (RSAParameters)xs.Deserialize(sr);
                }
            }
        }
        #endregion

        #region --CONST--
        const int DEFAULT_KEY_PAIR_LENGTH = 2048;
        #endregion

        #region --CTOR--

        public RSAWrapper(int keyPairLength)
        {
            rsa = new RSACryptoServiceProvider(keyPairLength);
            this.privateKey = rsa.ExportParameters(true);
            this.publicKey = rsa.ExportParameters(false);
        }

        public RSAWrapper() : this(DEFAULT_KEY_PAIR_LENGTH)
        {

        }

        #endregion

        #region --METHODS--
        public byte[] Encrypt(byte[] data)
        {
            return this.Encrypt(data, this.PublicKey);
        }

        public byte[] Encrypt(byte[] data, RSAParameters publicKey)
        {
            try
            {
                byte[] bytesCipherText;
                using (var locRsa = new RSACryptoServiceProvider())
                {
                    locRsa.ImportParameters(publicKey);
                    bytesCipherText = locRsa.Encrypt(data, false);
                }

                return bytesCipherText;
            }
            catch (CryptographicException exc)
            {

                Log(exc.Message);
                return null;
            }
        }



        public byte[] Decrypt(byte[] data, RSAParameters privateKey)
        {
            try
            {
                byte[] bytesPlainText;
                using (var locRsa = new RSACryptoServiceProvider())
                {
                    locRsa.ImportParameters(privateKey);
                    bytesPlainText = locRsa.Decrypt(data, false);
                }

                return bytesPlainText;
            }
            catch (CryptographicException exc)
            {

                Log(exc.Message);
                return null;
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            return Decrypt(data, this.privateKey);
        }

        #endregion
    }
}

