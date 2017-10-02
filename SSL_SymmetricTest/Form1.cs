using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace SSL_SymmetricTest
{
    public partial class Form1 : Form
    {
        DESCryptoServiceProvider alg;
        byte[] key;
        byte[] IV;
        byte[] salt;

        public Form1()
        {
            InitializeComponent();
            alg = new DESCryptoServiceProvider();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGenerateKeys_Click(object sender, EventArgs e)
        {
            string passPhrase = tbPassPhrase.Text;
            if (!String.IsNullOrWhiteSpace(passPhrase))
            {
                GenerateKeys(passPhrase);

                tbKey.Text = key.ToHexString();
                tbIV.Text = IV.ToHexString();
                tbSalt.Text = salt.ToHexString();
            }
        }

        private void GenerateKeys(string passPhrase)
        {
            alg.GenerateIV();
            this.IV = alg.IV;

            salt = new byte[8];
            RNGCryptoServiceProvider saltGen = new RNGCryptoServiceProvider();
            saltGen.GetBytes(salt);

            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(passPhrase, salt);
            key = rfc2898.GetBytes(8);
        }

        private static EncryptionResult EncryptDES(byte[] data, byte[] key)
        {
            EncryptionResult res = new EncryptionResult();

            using (var des = DES.Create())
            {
                des.GenerateIV(); res.IV = des.IV;
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.Zeros;

                //Generate random salt
                byte[] saltBuffer = new byte[16];
                RNGCryptoServiceProvider saltGen = new RNGCryptoServiceProvider();
                saltGen.GetBytes(saltBuffer);
                res.salt = saltBuffer;

                //Generate key based on password and salt
                Rfc2898DeriveBytes PBDKF2 = new Rfc2898DeriveBytes(key, saltBuffer, 1000);
                key = PBDKF2.GetBytes(8);
                res.key = key;

                var cipher = des.CreateEncryptor(key, des.IV);
                using (var ms = new MemoryStream())
                {
                    ms.Write(des.IV, 0, des.IV.Length);
                    ms.Write(saltBuffer, 0, saltBuffer.Length);
                    using (CryptoStream cs = new CryptoStream(ms, cipher, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(Encoding.UTF8.GetString(data).ToCharArray());
                        }
                    }
                    //return ms.ToArray();
                    var r = ms.ToArray();
                    res.encryptedData = Convert.ToBase64String(r);
                    //res.encryptedData = Convert.tob
                }

            }

            return res;
        }

        private string EncryptDES2(byte[] data, string password)
        {
            string result = String.Empty;

            if (this.IV == null || this.salt == null|| this.key==null)
            {
                GenerateKeys(password);
            }

            return result;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string passKey = tbPassPhrase.Text;
            string input = tbInput.Text;
            if (String.IsNullOrWhiteSpace(passKey) || String.IsNullOrWhiteSpace(input)) return;

            byte[] data = Encoding.UTF8.GetBytes(input);
            byte[] key = Encoding.UTF8.GetBytes(passKey);
            var res = EncryptDES(data, key);            
            String encrypted = res.encryptedData;
            tbOutput.Clear();
            tbOutput.Text = encrypted;
            tbIV.Clear();
            tbIV.Text = Encoding.UTF8.GetChars(res.IV).ToString();

        }
    }

    public struct EncryptionResult
    {
        public byte[] key;
        public byte[] IV;
        public byte[] salt;
        public string encryptedData;
    }

    static class Extensions
    {
        public static string ToHexString(this byte[] data)
        {
            return (BitConverter.ToString(data)).Replace("-", "");
        }

        public static byte[] ToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();
        }
    }
}
