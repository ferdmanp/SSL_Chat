using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSL_Chat_Client
{
    public partial class frmMainWin : Form
    {
        public frmMainWin()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            List<string> messages = new List<string>();
            messages.AddRange(tbMessage.Lines);
            messages.AddRange(tbChatMessages.Lines);
            tbChatMessages.Lines = messages.ToArray();
            tbMessage.Clear();

        }
    }
}

