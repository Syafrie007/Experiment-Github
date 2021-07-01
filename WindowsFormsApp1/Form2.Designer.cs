
namespace WindowsFormsApp1
{
    partial class Form2
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
            Syncfusion.Windows.Forms.Tools.MetroSplitButtonRenderer metroSplitButtonRenderer2 = new Syncfusion.Windows.Forms.Tools.MetroSplitButtonRenderer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            Syncfusion.Windows.Forms.Tools.TreeNavigator.HeaderCollection headerCollection2 = new Syncfusion.Windows.Forms.Tools.TreeNavigator.HeaderCollection();
            Syncfusion.Windows.Forms.Tools.CustomImageCollection customImageCollection2 = new Syncfusion.Windows.Forms.Tools.CustomImageCollection();
            Syncfusion.Windows.Forms.Tools.ResetButton resetButton2 = new Syncfusion.Windows.Forms.Tools.ResetButton();
            this.splitButton1 = new Syncfusion.Windows.Forms.Tools.SplitButton();
            this.multiSelectionComboBox1 = new Syncfusion.Windows.Forms.Tools.MultiSelectionComboBox();
            this.treeNavigator1 = new Syncfusion.Windows.Forms.Tools.TreeNavigator();
            this.treeMenuItem1 = new Syncfusion.Windows.Forms.Tools.TreeMenuItem();
            this.treeMenuItem5 = new Syncfusion.Windows.Forms.Tools.TreeMenuItem();
            this.treeMenuItem6 = new Syncfusion.Windows.Forms.Tools.TreeMenuItem();
            this.treeMenuItem7 = new Syncfusion.Windows.Forms.Tools.TreeMenuItem();
            this.treeMenuItem2 = new Syncfusion.Windows.Forms.Tools.TreeMenuItem();
            this.treeMenuItem3 = new Syncfusion.Windows.Forms.Tools.TreeMenuItem();
            this.treeMenuItem4 = new Syncfusion.Windows.Forms.Tools.TreeMenuItem();
            this.barItem1 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItem2 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItem3 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItem4 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.ratingControl1 = new Syncfusion.Windows.Forms.Tools.RatingControl();
            ((System.ComponentModel.ISupportInitialize)(this.multiSelectionComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitButton1
            // 
            this.splitButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(220)))));
            this.splitButton1.BeforeTouchSize = new System.Drawing.Size(133, 30);
            this.splitButton1.DropDownPosition = Syncfusion.Windows.Forms.Tools.Position.Bottom;
            this.splitButton1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitButton1.ForeColor = System.Drawing.Color.Black;
            this.splitButton1.Location = new System.Drawing.Point(300, 8);
            this.splitButton1.MinimumSize = new System.Drawing.Size(75, 23);
            this.splitButton1.Name = "splitButton1";
            metroSplitButtonRenderer2.SplitButton = this.splitButton1;
            this.splitButton1.Renderer = metroSplitButtonRenderer2;
            this.splitButton1.ShowDropDownOnButtonClick = false;
            this.splitButton1.Size = new System.Drawing.Size(133, 30);
            this.splitButton1.TabIndex = 4;
            this.splitButton1.Text = "asdasd";
            this.splitButton1.ThemeName = "Metro";
            // 
            // multiSelectionComboBox1
            // 
            this.multiSelectionComboBox1.BeforeTouchSize = new System.Drawing.Size(281, 30);
            this.multiSelectionComboBox1.ButtonStyle = Syncfusion.Windows.Forms.ButtonAppearance.Metro;
            this.multiSelectionComboBox1.DataSource = ((object)(resources.GetObject("multiSelectionComboBox1.DataSource")));
            this.multiSelectionComboBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multiSelectionComboBox1.Location = new System.Drawing.Point(13, 8);
            this.multiSelectionComboBox1.Name = "multiSelectionComboBox1";
            this.multiSelectionComboBox1.Size = new System.Drawing.Size(281, 30);
            this.multiSelectionComboBox1.TabIndex = 0;
            this.multiSelectionComboBox1.ThemeName = "Metro";
            this.multiSelectionComboBox1.UseVisualStyle = true;
            this.multiSelectionComboBox1.Click += new System.EventHandler(this.multiSelectionComboBox1_Click);
            // 
            // treeNavigator1
            // 
            headerCollection2.Font = new System.Drawing.Font("Arial", 8F);
            this.treeNavigator1.Header = headerCollection2;
            this.treeNavigator1.ItemBackColor = System.Drawing.SystemColors.Control;
            this.treeNavigator1.Items.Add(this.treeMenuItem1);
            this.treeNavigator1.Items.Add(this.treeMenuItem2);
            this.treeNavigator1.Items.Add(this.treeMenuItem3);
            this.treeNavigator1.Items.Add(this.treeMenuItem4);
            this.treeNavigator1.Location = new System.Drawing.Point(12, 43);
            this.treeNavigator1.MinimumSize = new System.Drawing.Size(150, 150);
            this.treeNavigator1.Name = "treeNavigator1";
            this.treeNavigator1.NavigationMode = Syncfusion.Windows.Forms.Tools.NavigationMode.Default;
            this.treeNavigator1.ShowHeader = false;
            this.treeNavigator1.Size = new System.Drawing.Size(200, 325);
            this.treeNavigator1.TabIndex = 2;
            this.treeNavigator1.Text = "treeNavigator1";
            this.treeNavigator1.ThemeName = "Metro";
            this.treeNavigator1.UseTouchScrollBehavior = true;
            // 
            // treeMenuItem1
            // 
            this.treeMenuItem1.BackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem1.ItemBackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem1.Items.Add(this.treeMenuItem5);
            this.treeMenuItem1.Items.Add(this.treeMenuItem6);
            this.treeMenuItem1.Items.Add(this.treeMenuItem7);
            this.treeMenuItem1.Location = new System.Drawing.Point(0, 0);
            this.treeMenuItem1.Name = "treeMenuItem1";
            this.treeMenuItem1.Size = new System.Drawing.Size(198, 50);
            this.treeMenuItem1.TabIndex = 1;
            this.treeMenuItem1.Text = "treeMenuItem1";
            // 
            // treeMenuItem5
            // 
            this.treeMenuItem5.ItemBackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem5.Location = new System.Drawing.Point(0, 0);
            this.treeMenuItem5.Name = "treeMenuItem5";
            this.treeMenuItem5.Size = new System.Drawing.Size(0, 0);
            this.treeMenuItem5.TabIndex = 0;
            this.treeMenuItem5.Text = "treeMenuItem5";
            // 
            // treeMenuItem6
            // 
            this.treeMenuItem6.ItemBackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem6.Location = new System.Drawing.Point(0, 0);
            this.treeMenuItem6.Name = "treeMenuItem6";
            this.treeMenuItem6.Size = new System.Drawing.Size(0, 0);
            this.treeMenuItem6.TabIndex = 0;
            this.treeMenuItem6.Text = "treeMenuItem6";
            // 
            // treeMenuItem7
            // 
            this.treeMenuItem7.ItemBackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem7.Location = new System.Drawing.Point(0, 0);
            this.treeMenuItem7.Name = "treeMenuItem7";
            this.treeMenuItem7.Size = new System.Drawing.Size(0, 0);
            this.treeMenuItem7.TabIndex = 0;
            this.treeMenuItem7.Text = "treeMenuItem7";
            // 
            // treeMenuItem2
            // 
            this.treeMenuItem2.BackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem2.ItemBackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem2.Location = new System.Drawing.Point(0, 52);
            this.treeMenuItem2.Name = "treeMenuItem2";
            this.treeMenuItem2.Size = new System.Drawing.Size(198, 50);
            this.treeMenuItem2.TabIndex = 1;
            this.treeMenuItem2.Text = "treeMenuItem2";
            // 
            // treeMenuItem3
            // 
            this.treeMenuItem3.BackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem3.ItemBackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem3.Location = new System.Drawing.Point(0, 104);
            this.treeMenuItem3.Name = "treeMenuItem3";
            this.treeMenuItem3.Size = new System.Drawing.Size(198, 50);
            this.treeMenuItem3.TabIndex = 1;
            this.treeMenuItem3.Text = "treeMenuItem3";
            // 
            // treeMenuItem4
            // 
            this.treeMenuItem4.BackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem4.ItemBackColor = System.Drawing.SystemColors.Control;
            this.treeMenuItem4.Location = new System.Drawing.Point(0, 156);
            this.treeMenuItem4.Name = "treeMenuItem4";
            this.treeMenuItem4.Size = new System.Drawing.Size(198, 50);
            this.treeMenuItem4.TabIndex = 1;
            this.treeMenuItem4.Text = "treeMenuItem4";
            // 
            // barItem1
            // 
            this.barItem1.BarName = "barItem1";
            this.barItem1.ShowToolTipInPopUp = false;
            this.barItem1.SizeToFit = true;
            // 
            // barItem2
            // 
            this.barItem2.BarName = "barItem2";
            this.barItem2.ShowToolTipInPopUp = false;
            this.barItem2.SizeToFit = true;
            // 
            // barItem3
            // 
            this.barItem3.BarName = "barItem3";
            this.barItem3.ShowToolTipInPopUp = false;
            this.barItem3.SizeToFit = true;
            // 
            // barItem4
            // 
            this.barItem4.BarName = "barItem4";
            this.barItem4.ShowToolTipInPopUp = false;
            this.barItem4.SizeToFit = true;
            // 
            // ratingControl1
            // 
            this.ratingControl1.Images = customImageCollection2;
            this.ratingControl1.Location = new System.Drawing.Point(439, 10);
            this.ratingControl1.Name = "ratingControl1";
            this.ratingControl1.ResetButton = resetButton2;
            this.ratingControl1.Shape = Syncfusion.Windows.Forms.Tools.Shapes.Heart;
            this.ratingControl1.Size = new System.Drawing.Size(150, 28);
            this.ratingControl1.TabIndex = 5;
            this.ratingControl1.Text = "ratingControl1";
            // 
            // Form2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 522);
            this.Controls.Add(this.ratingControl1);
            this.Controls.Add(this.splitButton1);
            this.Controls.Add(this.multiSelectionComboBox1);
            this.Controls.Add(this.treeNavigator1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.multiSelectionComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.MultiSelectionComboBox multiSelectionComboBox1;
        private Syncfusion.Windows.Forms.Tools.TreeNavigator treeNavigator1;
        private Syncfusion.Windows.Forms.Tools.TreeMenuItem treeMenuItem1;
        private Syncfusion.Windows.Forms.Tools.TreeMenuItem treeMenuItem2;
        private Syncfusion.Windows.Forms.Tools.TreeMenuItem treeMenuItem3;
        private Syncfusion.Windows.Forms.Tools.TreeMenuItem treeMenuItem4;
        private Syncfusion.Windows.Forms.Tools.TreeMenuItem treeMenuItem5;
        private Syncfusion.Windows.Forms.Tools.TreeMenuItem treeMenuItem6;
        private Syncfusion.Windows.Forms.Tools.TreeMenuItem treeMenuItem7;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem1;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem2;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem3;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem4;
        private Syncfusion.Windows.Forms.Tools.SplitButton splitButton1;
        private Syncfusion.Windows.Forms.Tools.RatingControl ratingControl1;
    }
}