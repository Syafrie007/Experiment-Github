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
using S22.Xmpp.Im;
using jabber.client;
using System.Diagnostics;

namespace XMPP
{
    public partial class Form1 : Form
    {
        private JabberClient client;
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
            client = new jabber.client.JabberClient();
            client.Password = txtPassword.Text.Trim();
            client.User = txtUsername.Text.Trim();
            client.Server = txtServer.Text.Trim();
            client.OnReadText += (ss, ee)=>{
                var ll = "";
            };
            client.OnMessage += (ss, ee) =>
             {
                 _sync.Send(x => {
                     var txt = $"Pesan dari <{ee.ID}> {ee.Body}\n";
                     rtf.AppendText(txt);
                 }, null);

             };


            client.Connect();
            client.Login();
            //client.SetStatus(new Status(Availability.Online, "kiosgamer",priority: 127));
            var txt2 = $"Terkoneksi ke <{client.JID}>\n";
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

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (client == null)
                return;

            for(var i = numericUpDown1.Value; i <= 1000000; i++)
            {
                try
                {
                    client.Message("x220992@xmpp.jp", $"#test{i}.5.1460139372");
                    rtf.AppendText($"Mengirim   : #test{i}.5.1460139372\n");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                await Task.Delay(20000);
            }

        }
    }





}
