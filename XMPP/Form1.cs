using Artalk.Xmpp.Client;
using Artalk.Xmpp.Im;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace XMPP
{
    public partial class Form1 : Form
    {

        ArtalkXmppClient client;
        SynchronizationContext _sync;
        public Form1()
        {
            InitializeComponent();
            _sync = SynchronizationContext.Current;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new ArtalkXmppClient(txtServer.Text, txtUsername.Text, txtPassword.Text);
            client.Message += OnNewMessage;
        }

        private void OnNewMessage(object sender, MessageEventArgs e)
        {
            //MessageBox.Show("Message from <" + e.Jid + ">: " + e.Message.Body);

            _sync.Send(x => {
                rtf.AppendText("Message from <" + e.Jid + ">: " + e.Message.Body + "\n");
            }, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Connect();

            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
        }

        private void btnDiconnect_Click(object sender, EventArgs e)
        {
            client.Dispose();
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            client.SendMessage(
                new Artalk.Xmpp.Jid("xmpp.jp", "syafrie007"), txtMessage.Text,type: MessageType.Normal);

        }
    }





}
