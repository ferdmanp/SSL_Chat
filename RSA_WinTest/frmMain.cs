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

namespace RSA_WinTest
{
    public partial class frmMain : Form
    {
        List<String> Log;
        UnicodeEncoding ByteConverter = new UnicodeEncoding();

        RSAWrapper rsa;       

        public frmMain()
        {
            InitializeComponent();
            Log = new List<string>();
            InitRSA();
        }

        private void InitRSA()
        {          

            rsa = new RSAWrapper();
            rsa.Log = Log.Add;            

            tbPublicKey.Text = rsa.PublicKey;
            tbPrivateKey.Text = rsa.PrivateKey;

        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string input = tbPlaintText.Text;

            if (!String.IsNullOrWhiteSpace(input))
            {
                byte[] bytesInput = ByteConverter.GetBytes(input);                
                var encoded = rsa.Encrypt(bytesInput);
                tbEncrypted.Text = Convert.ToBase64String(encoded);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            string input = tbEncrypted.Text;

            if (!String.IsNullOrWhiteSpace(input))
            {
                byte[] bytesInput = Convert.FromBase64String(input);                
                var decoded = rsa.Decrypt(bytesInput);
                tbDecrypted.Text = ByteConverter.GetString(decoded);
            }
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            InitRSA();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChatClient frm = new frmChatClient();
            frm.Show();
        }
    }
}

