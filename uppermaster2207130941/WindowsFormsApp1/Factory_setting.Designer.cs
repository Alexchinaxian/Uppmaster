namespace WindowsFormsApp1
{
    partial class Factory_setting
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
      this.panel_Factory = new Sunny.UI.UITitlePanel();
      this.SuspendLayout();
      // 
      // panel_Factory
      // 
      this.panel_Factory.AutoSize = true;
      this.panel_Factory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panel_Factory.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
      this.panel_Factory.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel_Factory.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.panel_Factory.Location = new System.Drawing.Point(0, 0);
      this.panel_Factory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.panel_Factory.MinimumSize = new System.Drawing.Size(1, 1);
      this.panel_Factory.Name = "panel_Factory";
      this.panel_Factory.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
      this.panel_Factory.Radius = 1;
      this.panel_Factory.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
      this.panel_Factory.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      this.panel_Factory.Size = new System.Drawing.Size(377, 351);
      this.panel_Factory.Style = Sunny.UI.UIStyle.Custom;
      this.panel_Factory.TabIndex = 10;
      this.panel_Factory.Text = "Factory_setting";
      this.panel_Factory.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
      this.panel_Factory.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      // 
      // Factory_setting
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(377, 351);
      this.Controls.Add(this.panel_Factory);
      this.MaximumSize = new System.Drawing.Size(393, 390);
      this.MinimumSize = new System.Drawing.Size(393, 390);
      this.Name = "Factory_setting";
      this.Text = "Factory_setting";
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private Sunny.UI.UITitlePanel panel_Factory;
    }
}