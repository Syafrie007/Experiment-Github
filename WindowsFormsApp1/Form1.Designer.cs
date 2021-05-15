
namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.iTalk_Button_21 = new iTalk.iTalk_Button_2();
            this.iTalk_Button_11 = new iTalk.iTalk_Button_1();
            this.iTalk_TextBox_Big1 = new iTalk.iTalk_TextBox_Big();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel5 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(1143, 604);
            this.splitContainer1.SplitterDistance = 296;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(296, 604);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 60);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(296, 544);
            this.panel4.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(296, 60);
            this.panel3.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.iTalk_Button_21);
            this.panel2.Controls.Add(this.iTalk_Button_11);
            this.panel2.Controls.Add(this.iTalk_TextBox_Big1);
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(843, 604);
            this.panel2.TabIndex = 0;
            // 
            // iTalk_Button_21
            // 
            this.iTalk_Button_21.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Button_21.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.iTalk_Button_21.ForeColor = System.Drawing.Color.White;
            this.iTalk_Button_21.Image = null;
            this.iTalk_Button_21.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iTalk_Button_21.Location = new System.Drawing.Point(116, 91);
            this.iTalk_Button_21.Name = "iTalk_Button_21";
            this.iTalk_Button_21.Size = new System.Drawing.Size(166, 40);
            this.iTalk_Button_21.TabIndex = 4;
            this.iTalk_Button_21.Text = "iTalk_Button_21";
            this.iTalk_Button_21.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // iTalk_Button_11
            // 
            this.iTalk_Button_11.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Button_11.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.iTalk_Button_11.Image = null;
            this.iTalk_Button_11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iTalk_Button_11.Location = new System.Drawing.Point(417, 104);
            this.iTalk_Button_11.Name = "iTalk_Button_11";
            this.iTalk_Button_11.Size = new System.Drawing.Size(166, 62);
            this.iTalk_Button_11.TabIndex = 3;
            this.iTalk_Button_11.Text = "iTalk_Button_11";
            this.iTalk_Button_11.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // iTalk_TextBox_Big1
            // 
            this.iTalk_TextBox_Big1.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_TextBox_Big1.Font = new System.Drawing.Font("Tahoma", 11F);
            this.iTalk_TextBox_Big1.ForeColor = System.Drawing.Color.DimGray;
            this.iTalk_TextBox_Big1.Image = global::WindowsFormsApp1.Properties.Resources.binoculars_24;
            this.iTalk_TextBox_Big1.Location = new System.Drawing.Point(75, 137);
            this.iTalk_TextBox_Big1.MaxLength = 32767;
            this.iTalk_TextBox_Big1.Multiline = false;
            this.iTalk_TextBox_Big1.Name = "iTalk_TextBox_Big1";
            this.iTalk_TextBox_Big1.ReadOnly = false;
            this.iTalk_TextBox_Big1.Size = new System.Drawing.Size(279, 41);
            this.iTalk_TextBox_Big1.TabIndex = 2;
            this.iTalk_TextBox_Big1.Text = "iTalk_TextBox_Big1";
            this.iTalk_TextBox_Big1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.iTalk_TextBox_Big1.UseSystemPasswordChar = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 201);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(593, 391);
            this.dataGridView1.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(843, 60);
            this.panel5.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 604);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView dataGridView1;
        private iTalk.iTalk_TextBox_Big iTalk_TextBox_Big1;
        private iTalk.iTalk_Button_2 iTalk_Button_21;
        private iTalk.iTalk_Button_1 iTalk_Button_11;
    }
}

