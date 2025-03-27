using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xx;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {

        private BindingList<TagRecord> _records;
        private ReadCountMonitor _readCountMonitor;
        private string fs = Application.StartupPath + "/opt";

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            if (!File.Exists(fs))
            {
                ReadCountMonitor.ShowSettingsForm(fs);
            }

            _readCountMonitor = new ReadCountMonitor(fs);


            _records =new BindingList<TagRecord>();

            _records.ListChanged += (ss, ee) =>
            {
                if (ee.ListChangedType == ListChangedType.ItemAdded)
                {
                    var obj = _records[ee.NewIndex];
                    if (string.IsNullOrEmpty(obj.EPC))
                        return;

                    _readCountMonitor.MonitorTag(_records[ee.NewIndex]);

                }
            };


            //_readCountMonitor = new ReadCountMonitor("http://localhost:5000",
            //    "H79DarFc0iLXamKRYOWdFR9JTP6iInyBTNuwBi7uKijFIynC84JAP65.wKrBjk97");





            _readCountMonitor.RequiredTimeDiff = TimeSpan.FromSeconds(10);
            _readCountMonitor.NomorTelpAdmin = "6283124204712";


            dataGridView1.DataSource = _records;


            foreach (var item in _records)
            {
                _readCountMonitor.MonitorTag(item);
            }

            _readCountMonitor.NamaPerangkat = "Perangkat test";
            _readCountMonitor.Start();
        }

        private void multiSelectionComboBox1_Click(object sender, EventArgs e)
        {

        }

        static List<TagRecord> GenerateDummyTags(int count)
        {
            var random = new Random();
            var dummyTags = new List<TagRecord>();

            for (int i = 0; i < count; i++)
            {
                dummyTags.Add(new TagRecord
                {
                    //EPC = $"EPC-{random.Next(100000, 999999)}",
                    EPC= "EPC12345",
                    NamaPerangkat = $"Device-{random.Next(1, 10)}",
                    ReadCount = random.Next(1, 50)
                });
            }

            return dummyTags;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _records[(int)numericUpDown1.Value].ReadCount += 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var l = GenerateDummyTags(1);
            _records.Add(l.First());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _readCountMonitor.Stop();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //_readCountMonitor.ShowScanTagHistory("EPC12345");
            _readCountMonitor.ShowLog();
            
            //ReadCountMonitor.ShowSettingsForm(fs);
            //_readCountMonitor.Settings.Reload();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            string token = "H79DarFc0iLXamKRYOWdFR9JTP6iInyBTNuwBi7uKijFIynC84JAP65.wKrBjk97"; // Ganti dengan API Key dari Wablas
            string phoneNumber = "6283124204712"; // Nomor tujuan
            string message = "Halo, ini pesan dari C#!";

            bool success = await SendWaMessage(token, phoneNumber, message);
            Console.WriteLine(success ? "Pesan berhasil dikirim" : "Gagal mengirim pesan");
        }

        static async Task<bool> SendWaMessage(string token, string phone, string message)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", token);
                var content = new StringContent($"{{\"phone\":\"{phone}\",\"message\":\"{message}\"}}", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://solo.wablas.com/api/send-message", content);
                return response.IsSuccessStatusCode;
            }
        }
    }
}
