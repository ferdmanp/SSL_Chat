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
using RSA_CryptLib;

namespace RSA_WinClient
{
    public partial class frmMain : Form
    {

        public  string UserName {
            get;
            set;
        }
        //private string userName;
        ChatSvcProxy svc;
        RSA_ChatService.ChatClient currentClient;
        BindingList<RSA_ChatService.ChatClient> clients = new BindingList<RSA_ChatService.ChatClient>();
        BindingList<RSA_ChatService.ChatMessage> messages = new BindingList<RSA_ChatService.ChatMessage>();
        BindingList<string> log = new BindingList<string>();
        Timer refreshTimer = new Timer();
        BackgroundWorker bgThread = new BackgroundWorker();

        

        RSAWrapper myRsa;
        RSAParameters publicKey;
        RSAParameters privateKey;

        Encoding ByteConverter = new UTF8Encoding();
        public frmMain()
        {
            InitializeComponent();
            svc = new ChatSvcProxy();
            lbOnlineClients.DataSource = clients;
            lbMessages.DataSource = messages;
            lbLog.DataSource = log;
            refreshTimer.Interval = 200;//ms
            refreshTimer.Tick += RefreshTimer_Tick;
            bgThread.WorkerSupportsCancellation = true;
            bgThread.DoWork += BgThread_DoWork;
            bgThread.RunWorkerCompleted += BgThread_RunWorkerCompleted;

            InitRSA();

        }

        private void InitRSA()
        {
            myRsa = new RSAWrapper();
            myRsa.Log = log.Add;
        }

        private void WriteLog(string message)
        {
            throw new NotImplementedException();
        }

        private void BgThread_RunWorkerCompleted
            (object sender, RunWorkerCompletedEventArgs e)
        {
            var selection=(RSA_ChatService.ChatClient)e.Result;
            if (clients.Contains(selection))
            {
                lbOnlineClients.SelectedItem = selection;
            }
            lbOnlineClients.EndUpdate();
            lbMessages.EndUpdate();
        }

        private void BgThread_DoWork(object sender, DoWorkEventArgs e)
        {
            RecieveMessages();
            UpdateActiveConnectionsList();
            e.Result = e.Argument;
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (!bgThread.IsBusy)
            {
                var selectedClient = lbOnlineClients.SelectedItem;
                lbMessages.BeginUpdate();
                lbOnlineClients.BeginUpdate();
                bgThread.RunWorkerAsync(selectedClient);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            var frmLogin = new frmLogin();
            frmLogin.ShowDialog();
            this.UserName = frmLogin.UserName;             
            UpdateHeader();
            Connect();
            currentClient = svc.Register(this.UserName);
            if (this.myRsa != null)
            {
                svc.SetSecurityParams(currentClient, myRsa.PublicKey);
            }
            
        }

        private void Connect()
        {
            try
            {
                svc.Open();                
                refreshTimer.Enabled = true;
                refreshTimer.Start();
                
            }
            catch (Exception exc)
            {
                ProcessError(exc);
                //throw;
            }
            
        }

        private void UpdateActiveConnectionsList()
        {
            
            try
            {
               var _clients = svc.GetConnectionsList();
                clients.Clear();
                foreach (var _client in _clients)
                {
                    if(_client.Id!=currentClient.Id)
                        clients.Add(_client);
                }
            }
            catch (Exception exc)
            {
                ProcessError(exc);
            }
        }

        private void ProcessError(Exception exc)
        {
            MessageBox.Show(exc.Message, "Error occured!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void UpdateHeader()
        {
            //throw new NotImplementedException();
            this.Text = $"Secure chat [{this.UserName}]";
        }

        private void RecieveMessages()
        {
            var messages = svc.RecieveMessagesById(currentClient.Id);
            foreach (var item in messages)
            {
                //myRsa.Decrypt()
                log.Add(item.MessageText.Length.ToString());
                byte[] bytesCrypted = Convert.FromBase64String(item.MessageText);
                    //ByteConverter.GetBytes(item.MessageText);
                log.Add(bytesCrypted.Length.ToString());
                //System.Threading.Thread.CurrentThread.Suspend();
                byte[] bytesDecrypted = myRsa.Decrypt(bytesCrypted, myRsa.PrivateKey);
                item.MessageText = //Convert.ToBase64String(bytesDecrypted);
                    ByteConverter.GetString(bytesDecrypted);
                this.messages.Add(item);
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateActiveConnectionsList();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void Disconnect()
        {
            try
            {
                if (currentClient != null)
                {
                    refreshTimer.Stop();
                    bgThread.CancelAsync();                    
                    svc.Unregister(this.currentClient);
                }

                svc.Close();
            }
            catch (Exception exc)
            {
                ProcessError(exc);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbMessage.Text))
            {
                string msgSrc = tbMessage.Text;
                //string message
                RSA_ChatService.ChatClient recipient = (RSA_ChatService.ChatClient)lbOnlineClients.SelectedItem;
                byte[] cryptedBytes = myRsa.Encrypt(ByteConverter.GetBytes(msgSrc), recipient.PublicKey);
                            //myRsa.Encrypt(Convert.FromBase64String(message), recipient.PublicKey);
                log.Add(cryptedBytes.Length.ToString());
                string cryptedMessage = Convert.ToBase64String(cryptedBytes);
                            //ByteConverter.GetString(cryptedBytes);
                            //Convert.ToBase64String(cryptedBytes);
                log.Add(cryptedMessage.Length.ToString());

                var chatMessage = svc.SendMessage(cryptedMessage, this.currentClient, recipient);
                    //svc.SendMessage(Convert.ToBase64String(cryptedMessage), this.currentClient, recipient);
                chatMessage.MessageText = msgSrc;
                messages.Add(chatMessage);
                tbMessage.Clear();
                tbMessage.Focus();                
            }
        }

        private void tbMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Control)
                btnSend_Click(sender, e);
        }

        private void tbMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbMessages_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbUserName.Text))
            {
                this.UserName = tbUserName.Text;
                UpdateHeader();
                Connect();
                currentClient = svc.Register(this.UserName);
                if (this.myRsa != null)
                {
                    svc.SetSecurityParams(currentClient, myRsa.PublicKey);
                }
            }

            tbUserName.Enabled = false;
            btnConnect.Text = "Disconnect";
            btnConnect.Click -= btnConnect_Click;
            btnConnect.Click += btnDisconnectClick;
        }

        private void btnDisconnectClick(object sender, EventArgs e)
        {
            Disconnect();
            btnConnect.Text = "Connect";
            btnConnect.Click += btnConnect_Click;
            btnConnect.Click -= btnDisconnectClick;
        }
    }
}
