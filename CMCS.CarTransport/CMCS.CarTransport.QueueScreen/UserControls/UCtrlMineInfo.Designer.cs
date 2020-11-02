namespace CMCS.CarTransport.QueueScreen.UserControls
{
    partial class UCtrlMineInfo
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCarNumbers = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Black;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 14.5F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(3, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(339, 20);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "中煤乌审旗洗精煤：精煤/18车/800吨";
            // 
            // lblCarNumbers
            // 
            this.lblCarNumbers.AutoSize = true;
            this.lblCarNumbers.Font = new System.Drawing.Font("宋体", 14.5F);
            this.lblCarNumbers.ForeColor = System.Drawing.Color.White;
            this.lblCarNumbers.Location = new System.Drawing.Point(11, 21);
            this.lblCarNumbers.Name = "lblCarNumbers";
            this.lblCarNumbers.Size = new System.Drawing.Size(699, 60);
            this.lblCarNumbers.TabIndex = 16;
            this.lblCarNumbers.Text = "宁AD8888  宁AD8888  宁AD8888  宁AD8888  宁AD8888   宁AD8888  宁AD8888\r\n宁AD8888  宁AD8888 " +
    " 宁AD8888  宁AD8888  宁AD8888   宁AD8888  宁AD8888\r\n宁AD8888  宁AD8888  宁AD8888  宁AD888" +
    "8  宁AD8888   宁AD8888  宁AD8888\r\n";
            // 
            // UCtrlMineInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.lblCarNumbers);
            this.Controls.Add(this.lblTitle);
            this.Name = "UCtrlMineInfo";
            this.Size = new System.Drawing.Size(720, 109);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCarNumbers;
    }
}
