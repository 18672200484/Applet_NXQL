namespace CMCS.DataTester.Frms
{
    partial class FrmCamera
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
			this.btnStart = new System.Windows.Forms.Button();
			this.panVideo1 = new System.Windows.Forms.Panel();
			this.errorMsg = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(586, 12);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(106, 23);
			this.btnStart.TabIndex = 1;
			this.btnStart.Text = "初始化设备";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// panVideo1
			// 
			this.panVideo1.Location = new System.Drawing.Point(12, 12);
			this.panVideo1.Name = "panVideo1";
			this.panVideo1.Size = new System.Drawing.Size(542, 455);
			this.panVideo1.TabIndex = 2;
			// 
			// errorMsg
			// 
			this.errorMsg.Location = new System.Drawing.Point(586, 41);
			this.errorMsg.Name = "errorMsg";
			this.errorMsg.Size = new System.Drawing.Size(303, 243);
			this.errorMsg.TabIndex = 3;
			this.errorMsg.Text = "";
			// 
			// FrmCamera
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(916, 478);
			this.Controls.Add(this.errorMsg);
			this.Controls.Add(this.panVideo1);
			this.Controls.Add(this.btnStart);
			this.Name = "FrmCamera";
			this.Text = "车号识别预览";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCamera_FormClosing);
			this.Load += new System.EventHandler(this.FrmCarLocation_Load);
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Panel panVideo1;
		private System.Windows.Forms.RichTextBox errorMsg;
	}
}