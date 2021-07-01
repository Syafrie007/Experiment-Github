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
                     var txt = $"Pesan dari <{ee.From}> {ee.Body}\n";
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

        private Random _rnd=new Random();
        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (client == null)
                return;

            //var s = new[] { "5", "530", "510", "100", "35", "25", "15", "25", "40", "5320",
            //"320","22","110","155","60","mingguan","bulanan","mingguan","bulanan"};


            var s=new[] {"10" };

            for(var i = numericUpDown1.Value; i <= 1000000; i++)
            {
                try
                {
                    client.Message("x220992@xmpp.jp", $"#test{i}.{s[_rnd.Next(0, s.Length - 1)]}.785612127");
                    rtf.AppendText($"Mengirim   : #test{i}.{s[_rnd.Next(0,s.Length-1)]}.785612127\n");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                await Task.Delay(2000);
            }

        }


        public static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(long unixTime)
        {
            var rval = epoch.AddSeconds(unixTime);
            return rval;
        }

        public static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        public static long ToUnixTime( DateTime dateTime)
        {
            var timeSpan = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            var t = new OtpNet.Hotp(OtpNet.Base32Encoding.ToBytes ("LS7XPKD55HWIDXLR"));
            var i = UnixTimeNow() / 180;
            var ok =180- UnixTimeNow() % 180;
            var o=t.ComputeHOTP(i);

            rtf.AppendText(o + $"    {ok}" + "\n");
        }
    }





}
