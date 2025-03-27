using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class FormTagScanViewer:Form
    {
        internal DataGridView dgv;

        private void InitializeComponent()
        {
            this.dgv = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 0);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.Size = new System.Drawing.Size(413, 273);
            this.dgv.TabIndex = 0;
            // 
            // FormTagScanViewer
            // 
            this.ClientSize = new System.Drawing.Size(413, 273);
            this.Controls.Add(this.dgv);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTagScanViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tag Scan";
            this.Shown += new System.EventHandler(this.FormTagScanViewer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        public FormTagScanViewer()
        {
            InitializeComponent();
        }

        private void FormTagScanViewer_Shown(object sender, EventArgs e)
        {
            dgv.AutoResizeColumns(System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells);

        }
    }
}
