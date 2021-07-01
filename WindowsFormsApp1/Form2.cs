using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var l = new List<string>(new[] { "a", "b", "c", "D" });
            multiSelectionComboBox1.DataSource = l;
        }

        private void multiSelectionComboBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
