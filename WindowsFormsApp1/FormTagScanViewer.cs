using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xx;

namespace WindowsFormsApp1
{
    public class FormTagScanViewer:Form
    {
        internal TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private RichTextBox txtLog;
        private TabPage tabPage3;
        private Label label2;
        private TextBox txtWaBlasToken;
        private Label label1;
        private TextBox txt_Api_endPoint;
        private Label label3;
        private TextBox txtHpAdmin;
        private Button btnSimpan;
        internal DataGridView dgv;

        private void InitializeComponent()
        {
            this.dgv = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txt_Api_endPoint = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWaBlasToken = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHpAdmin = new System.Windows.Forms.TextBox();
            this.btnSimpan = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(3, 3);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.Size = new System.Drawing.Size(399, 241);
            this.dgv.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(413, 273);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgv);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(405, 247);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Data Scan";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtLog);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(405, 247);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(399, 241);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnSimpan);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.txtHpAdmin);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.txtWaBlasToken);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.txt_Api_endPoint);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(405, 247);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txt_Api_endPoint
            // 
            this.txt_Api_endPoint.Location = new System.Drawing.Point(129, 17);
            this.txt_Api_endPoint.Name = "txt_Api_endPoint";
            this.txt_Api_endPoint.Size = new System.Drawing.Size(239, 20);
            this.txt_Api_endPoint.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Api Endpoint";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "WaBlast Token.Secret";
            // 
            // txtWaBlasToken
            // 
            this.txtWaBlasToken.Location = new System.Drawing.Point(129, 43);
            this.txtWaBlasToken.Name = "txtWaBlasToken";
            this.txtWaBlasToken.Size = new System.Drawing.Size(239, 20);
            this.txtWaBlasToken.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Hp Admin";
            // 
            // txtHpAdmin
            // 
            this.txtHpAdmin.Location = new System.Drawing.Point(129, 69);
            this.txtHpAdmin.Name = "txtHpAdmin";
            this.txtHpAdmin.Size = new System.Drawing.Size(239, 20);
            this.txtHpAdmin.TabIndex = 4;
            // 
            // btnSimpan
            // 
            this.btnSimpan.Location = new System.Drawing.Point(322, 216);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 6;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.UseVisualStyleBackColor = true;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // FormTagScanViewer
            // 
            this.ClientSize = new System.Drawing.Size(413, 273);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTagScanViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tag Scan";
            this.Load += new System.EventHandler(this.FormTagScanViewer_Load);
            this.Shown += new System.EventHandler(this.FormTagScanViewer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        public FormTagScanViewer()
        {
            InitializeComponent();
        }


        public Settings Settings { get; set; }



        public void SetLog(List<Tuple< string,DateTime>> logs)
        {
            foreach(var item in logs)
            {
                txtLog.AppendText(item.Item2.ToString() + " -- "+ item.Item1 + "\n");
            }
        }


        private void FormTagScanViewer_Shown(object sender, EventArgs e)
        {
            dgv.AutoResizeColumns(System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells);

        }

        private void FormTagScanViewer_Load(object sender, EventArgs e)
        {
            if (Settings!= null)
            {
                txtHpAdmin.Text = Settings.HpAdmin;
                txtWaBlasToken.Text = Settings.WaBlasToken;
                txt_Api_endPoint.Text = Settings.ApiEndPoint;

            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (Settings != null)
            {
                Settings.HpAdmin = txtHpAdmin.Text;
                Settings.WaBlasToken = txtWaBlasToken.Text;
                Settings.ApiEndPoint = txt_Api_endPoint.Text;

                Settings.Save();
                Close();
            }

        }
    }


}
