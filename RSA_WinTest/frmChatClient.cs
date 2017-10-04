using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSA_WinTest
{
    //public delegate void LogMethod(string message);
    public partial class frmChatClient : Form
    {


        BindingList<String> logList = new BindingList<string>();
        BindingList<string> chatList = new BindingList<string>();
        LogMethod Log;
        SocketClient client;
        BackgroundWorker bgWorker;
        Timer checkTimer;

        string incomeBuffer = String.Empty;
        public frmChatClient()
        {
            InitializeComponent();
            lbLog.DataSource = logList;
            lbChat.DataSource = chatList;
            //lbLog.
            Log = logList.Add;
            client = new SocketClient();
            client.RegisterLogger(Log);
  
            //Init timer
            checkTimer = new Timer();
            checkTimer.Interval = 1000;
            checkTimer.Tick += CheckTimer_Tick;

            //Init BackgroundWorker
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Log($"BW_ERROR: {e.Error.Message}");
                return;
            }
            else
            {
                WriteChat(incomeBuffer, "server");
            }


        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
            incomeBuffer+= client.ProcessMessages();
        }

        private void CheckTimer_Tick(object sender, EventArgs e)
        {
            incomeBuffer = String.Empty;
            if(!bgWorker.IsBusy)
            bgWorker.RunWorkerAsync();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string message = tbMessage.Text;
            client.SendMessage(message);
            WriteChat(message, "Me");
            tbMessage.Clear();
            tbMessage.Focus();
            checkTimer.Start();

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }

        private void WriteChat(string message, string author)
        {
            string msg = $"<{author}>: {message}";
            chatList.Add(msg);
        }

        private void tbMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
                btnSend_Click(sender, e);
        }

        private void btnConnect_Click_1(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbUserName.Text)) return;
            string username = tbUserName.Text;
            ChatClient client = new ChatClient(
                "192.168.88.250",
                9999,
                username
                );
            client.Connect();
        }
    }
}
