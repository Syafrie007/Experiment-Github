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
using S22.Xmpp.Client;

namespace XMPP
{
    public partial class Form1 : Form
    {
        private XmppClient client;
        SynchronizationContext _sync;
        public Form1()
        {
            InitializeComponent();
            _sync = SynchronizationContext.Current;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            client = new XmppClient(txtServer.Text.Trim(), 
                txtUsername.Text.Trim(),
                txtPassword.Text.Trim());

            client.Message += (ss, ee) =>
             {
                 _sync.Send(x => {
                     var txt = $"Pesan dari <{ee.Jid.Node}@{ee.Jid.Domain}> {ee.Message.Body}\n";
                     rtf.AppendText(txt);
                 }, null);

             };

            client.Connect();
            var txt2 = $"Terkoneksi ke <{client.Jid.Node}@{client.Jid.Domain}>\n";
            rtf.AppendText(txt2);

            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
        }

        private void btnDiconnect_Click(object sender, EventArgs e)
        {

            client?.Close();

            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (client == null)
                return;

            client.SendMessage(txtTo.Text.Trim(), txtMessage.Text.Trim());

        }
    }





}
