using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var l=GenerateDummyTags(1);

            _records=new BindingList<TagRecord>(l);
            
            _readCountMonitor = new ReadCountMonitor("http://localhost:5000", 1);
            _readCountMonitor.RequiredTimeDiff = TimeSpan.FromSeconds(1);


            dataGridView1.DataSource = _records;
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
            foreach(var item in _records)
            {
                _readCountMonitor.MonitorTag(item);
            }
            _readCountMonitor.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _readCountMonitor.Stop();

        }
    }
}
