

namespace WindowsFormsApp1
{
    partial class Form_DownLoad
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
      this.button_download = new Sunny.UI.UISymbolButton();
      this.textBox_tail = new Sunny.UI.UIRichTextBox();
      this.button_OenBin = new Sunny.UI.UISymbolButton();
      this.textBox_OpenFile = new Sunny.UI.UITextBox();
      this.label1 = new Sunny.UI.UIMarkLabel();
      this.comboBox_Moudlenumber = new Sunny.UI.UIComboBox();
      this.comboBox1 = new Sunny.UI.UIComboBox();
      this.progressBar1 = new Sunny.UI.UIProcessBar();
      this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
      this.uiComboBox1_Chip = new Sunny.UI.UIComboBox();
      this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
      this.SuspendLayout();
      // 
      // button_download
      // 
      this.button_download.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.button_download.Cursor = System.Windows.Forms.Cursors.Hand;
      this.button_download.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.button_download.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.button_download.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.button_download.Location = new System.Drawing.Point(465, 280);
      this.button_download.MinimumSize = new System.Drawing.Size(1, 1);
      this.button_download.Name = "button_download";
      this.button_download.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.button_download.Size = new System.Drawing.Size(110, 39);
      this.button_download.Style = Sunny.UI.UIStyle.Custom;
      this.button_download.TabIndex = 19;
      this.button_download.Text = "DownLoad";
      this.button_download.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.button_download.Click += new System.EventHandler(this.button_download_Click_1);
      // 
      // textBox_tail
      // 
      this.textBox_tail.AcceptsTab = true;
      this.textBox_tail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox_tail.FillColor = System.Drawing.Color.White;
      this.textBox_tail.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.textBox_tail.ImeMode = System.Windows.Forms.ImeMode.Close;
      this.textBox_tail.Location = new System.Drawing.Point(16, 100);
      this.textBox_tail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.textBox_tail.MinimumSize = new System.Drawing.Size(1, 1);
      this.textBox_tail.Name = "textBox_tail";
      this.textBox_tail.Padding = new System.Windows.Forms.Padding(2);
      this.textBox_tail.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.textBox_tail.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.textBox_tail.Size = new System.Drawing.Size(574, 177);
      this.textBox_tail.Style = Sunny.UI.UIStyle.Custom;
      this.textBox_tail.TabIndex = 10;
      this.textBox_tail.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // button_OenBin
      // 
      this.button_OenBin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.button_OenBin.Cursor = System.Windows.Forms.Cursors.Hand;
      this.button_OenBin.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.button_OenBin.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.button_OenBin.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.button_OenBin.Location = new System.Drawing.Point(16, 53);
      this.button_OenBin.MinimumSize = new System.Drawing.Size(1, 1);
      this.button_OenBin.Name = "button_OenBin";
      this.button_OenBin.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.button_OenBin.Size = new System.Drawing.Size(126, 40);
      this.button_OenBin.Style = Sunny.UI.UIStyle.Custom;
      this.button_OenBin.StyleCustomMode = true;
      this.button_OenBin.TabIndex = 9;
      this.button_OenBin.Text = "Open file";
      this.button_OenBin.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.button_OenBin.Click += new System.EventHandler(this.button_OenBin_Click);
      // 
      // textBox_OpenFile
      // 
      this.textBox_OpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox_OpenFile.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.textBox_OpenFile.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.textBox_OpenFile.Enabled = false;
      this.textBox_OpenFile.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.textBox_OpenFile.Location = new System.Drawing.Point(162, 55);
      this.textBox_OpenFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.textBox_OpenFile.MinimumSize = new System.Drawing.Size(1, 16);
      this.textBox_OpenFile.Name = "textBox_OpenFile";
      this.textBox_OpenFile.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.textBox_OpenFile.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.textBox_OpenFile.Size = new System.Drawing.Size(428, 41);
      this.textBox_OpenFile.Style = Sunny.UI.UIStyle.Custom;
      this.textBox_OpenFile.TabIndex = 8;
      this.textBox_OpenFile.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.label1.Location = new System.Drawing.Point(205, 21);
      this.label1.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.label1.Name = "label1";
      this.label1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.label1.Size = new System.Drawing.Size(103, 21);
      this.label1.TabIndex = 14;
      this.label1.Text = "Select Shelf";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.label1.Click += new System.EventHandler(this.label1_Click);
      // 
      // comboBox_Moudlenumber
      // 
      this.comboBox_Moudlenumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBox_Moudlenumber.DataSource = null;
      this.comboBox_Moudlenumber.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
      this.comboBox_Moudlenumber.FillColor = System.Drawing.Color.White;
      this.comboBox_Moudlenumber.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.comboBox_Moudlenumber.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.comboBox_Moudlenumber.Location = new System.Drawing.Point(304, 16);
      this.comboBox_Moudlenumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.comboBox_Moudlenumber.MinimumSize = new System.Drawing.Size(63, 0);
      this.comboBox_Moudlenumber.Name = "comboBox_Moudlenumber";
      this.comboBox_Moudlenumber.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
      this.comboBox_Moudlenumber.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.comboBox_Moudlenumber.Size = new System.Drawing.Size(86, 34);
      this.comboBox_Moudlenumber.Style = Sunny.UI.UIStyle.Custom;
      this.comboBox_Moudlenumber.TabIndex = 15;
      this.comboBox_Moudlenumber.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBox1
      // 
      this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBox1.DataSource = null;
      this.comboBox1.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
      this.comboBox1.FillColor = System.Drawing.Color.White;
      this.comboBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.comboBox1.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.comboBox1.Location = new System.Drawing.Point(111, 16);
      this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.comboBox1.MinimumSize = new System.Drawing.Size(63, 0);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
      this.comboBox1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.comboBox1.Size = new System.Drawing.Size(84, 34);
      this.comboBox1.Style = Sunny.UI.UIStyle.Custom;
      this.comboBox1.TabIndex = 17;
      this.comboBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // progressBar1
      // 
      this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.progressBar1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
      this.progressBar1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.progressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.progressBar1.Location = new System.Drawing.Point(16, 280);
      this.progressBar1.MinimumSize = new System.Drawing.Size(70, 3);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.progressBar1.Size = new System.Drawing.Size(430, 39);
      this.progressBar1.Style = Sunny.UI.UIStyle.Custom;
      this.progressBar1.TabIndex = 18;
      // 
      // uiMarkLabel1
      // 
      this.uiMarkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.uiMarkLabel1.AutoSize = true;
      this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.uiMarkLabel1.Location = new System.Drawing.Point(12, 21);
      this.uiMarkLabel1.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.uiMarkLabel1.Name = "uiMarkLabel1";
      this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.uiMarkLabel1.Size = new System.Drawing.Size(101, 21);
      this.uiMarkLabel1.TabIndex = 20;
      this.uiMarkLabel1.Text = "Select Rack";
      this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // uiComboBox1_Chip
      // 
      this.uiComboBox1_Chip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.uiComboBox1_Chip.DataSource = null;
      this.uiComboBox1_Chip.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
      this.uiComboBox1_Chip.FillColor = System.Drawing.Color.White;
      this.uiComboBox1_Chip.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.uiComboBox1_Chip.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.uiComboBox1_Chip.Location = new System.Drawing.Point(497, 16);
      this.uiComboBox1_Chip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.uiComboBox1_Chip.MinimumSize = new System.Drawing.Size(63, 0);
      this.uiComboBox1_Chip.Name = "uiComboBox1_Chip";
      this.uiComboBox1_Chip.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
      this.uiComboBox1_Chip.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.uiComboBox1_Chip.Size = new System.Drawing.Size(90, 34);
      this.uiComboBox1_Chip.Style = Sunny.UI.UIStyle.Custom;
      this.uiComboBox1_Chip.TabIndex = 22;
      this.uiComboBox1_Chip.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // uiMarkLabel2
      // 
      this.uiMarkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.uiMarkLabel2.AutoSize = true;
      this.uiMarkLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.uiMarkLabel2.Location = new System.Drawing.Point(400, 21);
      this.uiMarkLabel2.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.uiMarkLabel2.Name = "uiMarkLabel2";
      this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.uiMarkLabel2.Size = new System.Drawing.Size(105, 21);
      this.uiMarkLabel2.TabIndex = 21;
      this.uiMarkLabel2.Text = "Select Chip ";
      this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // Form_DownLoad
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.ClientSize = new System.Drawing.Size(594, 327);
      this.Controls.Add(this.uiComboBox1_Chip);
      this.Controls.Add(this.uiMarkLabel2);
      this.Controls.Add(this.uiMarkLabel1);
      this.Controls.Add(this.progressBar1);
      this.Controls.Add(this.comboBox1);
      this.Controls.Add(this.comboBox_Moudlenumber);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.button_download);
      this.Controls.Add(this.textBox_tail);
      this.Controls.Add(this.button_OenBin);
      this.Controls.Add(this.textBox_OpenFile);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "Form_DownLoad";
      this.Text = "Form2";
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion
        private Sunny.UI.UISymbolButton button_download;
        private Sunny.UI.UITextBox textBox_OpenFile;
        private Sunny.UI.UIComboBox comboBox_Moudlenumber;
        private Sunny.UI.UIComboBox comboBox1;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        public Sunny.UI.UIRichTextBox textBox_tail;
        private Sunny.UI.UIMarkLabel label1;
        public Sunny.UI.UISymbolButton button_OenBin;
        public Sunny.UI.UIProcessBar progressBar1;
    private Sunny.UI.UIComboBox uiComboBox1_Chip;
    private Sunny.UI.UIMarkLabel uiMarkLabel2;
  }
}