namespace CMCS.CarTransport.Queue.Frms.BaseInfo.SetSampler
{
    partial class FrmSetSampler_Oper
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
			this.labelX1 = new DevComponents.DotNetBar.LabelX();
			this.dtpStartTime = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
			this.dtpEndTime = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
			this.BtnMine = new DevComponents.DotNetBar.ButtonX();
			this.txt_MineName = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.labelX4 = new DevComponents.DotNetBar.LabelX();
			this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
			this.btnSubmit = new DevComponents.DotNetBar.ButtonX();
			this.btnCancel = new DevComponents.DotNetBar.ButtonX();
			this.labelX2 = new DevComponents.DotNetBar.LabelX();
			this.cmbSampler = new DevComponents.DotNetBar.Controls.ComboBoxEx();
			this.tableLayoutPanel1.SuspendLayout();
			this.panelEx2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dtpStartTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtpEndTime)).BeginInit();
			this.panelEx1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panelEx2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panelEx1, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.ForeColor = System.Drawing.Color.White;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(472, 244);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panelEx2
			// 
			this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
			this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.panelEx2.Controls.Add(this.cmbSampler);
			this.panelEx2.Controls.Add(this.labelX2);
			this.panelEx2.Controls.Add(this.labelX1);
			this.panelEx2.Controls.Add(this.dtpStartTime);
			this.panelEx2.Controls.Add(this.dtpEndTime);
			this.panelEx2.Controls.Add(this.BtnMine);
			this.panelEx2.Controls.Add(this.txt_MineName);
			this.panelEx2.Controls.Add(this.labelX4);
			this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelEx2.Location = new System.Drawing.Point(3, 3);
			this.panelEx2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.panelEx2.Name = "panelEx2";
			this.panelEx2.Size = new System.Drawing.Size(466, 201);
			this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.panelEx2.Style.GradientAngle = 90;
			this.panelEx2.TabIndex = 1;
			// 
			// labelX1
			// 
			this.labelX1.AutoSize = true;
			// 
			// 
			// 
			this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX1.ForeColor = System.Drawing.Color.White;
			this.labelX1.Location = new System.Drawing.Point(67, 57);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(56, 24);
			this.labelX1.TabIndex = 291;
			this.labelX1.Text = "时间段";
			// 
			// dtpStartTime
			// 
			this.dtpStartTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.dtpStartTime.BackgroundStyle.Class = "DateTimeInputBackground";
			this.dtpStartTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dtpStartTime.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
			this.dtpStartTime.ButtonDropDown.Visible = true;
			this.dtpStartTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dtpStartTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.dtpStartTime.ForeColor = System.Drawing.Color.White;
			this.dtpStartTime.Format = DevComponents.Editors.eDateTimePickerFormat.Custom;
			this.dtpStartTime.IsPopupCalendarOpen = false;
			this.dtpStartTime.Location = new System.Drawing.Point(127, 57);
			this.dtpStartTime.LockUpdateChecked = false;
			// 
			// 
			// 
			this.dtpStartTime.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
			// 
			// 
			// 
			this.dtpStartTime.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dtpStartTime.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
			this.dtpStartTime.MonthCalendar.ClearButtonVisible = true;
			// 
			// 
			// 
			this.dtpStartTime.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
			this.dtpStartTime.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
			this.dtpStartTime.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
			this.dtpStartTime.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.dtpStartTime.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
			this.dtpStartTime.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
			this.dtpStartTime.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dtpStartTime.MonthCalendar.DisplayMonth = new System.DateTime(2016, 7, 1, 0, 0, 0, 0);
			this.dtpStartTime.MonthCalendar.MarkedDates = new System.DateTime[0];
			this.dtpStartTime.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
			// 
			// 
			// 
			this.dtpStartTime.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.dtpStartTime.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
			this.dtpStartTime.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.dtpStartTime.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dtpStartTime.MonthCalendar.TodayButtonVisible = true;
			this.dtpStartTime.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
			this.dtpStartTime.Name = "dtpStartTime";
			this.dtpStartTime.Size = new System.Drawing.Size(271, 26);
			this.dtpStartTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.dtpStartTime.TabIndex = 290;
			this.dtpStartTime.WatermarkText = "开始时间";
			// 
			// dtpEndTime
			// 
			this.dtpEndTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.dtpEndTime.BackgroundStyle.Class = "DateTimeInputBackground";
			this.dtpEndTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dtpEndTime.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
			this.dtpEndTime.ButtonDropDown.Visible = true;
			this.dtpEndTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dtpEndTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.dtpEndTime.ForeColor = System.Drawing.Color.White;
			this.dtpEndTime.Format = DevComponents.Editors.eDateTimePickerFormat.Custom;
			this.dtpEndTime.IsPopupCalendarOpen = false;
			this.dtpEndTime.Location = new System.Drawing.Point(127, 102);
			this.dtpEndTime.LockUpdateChecked = false;
			// 
			// 
			// 
			this.dtpEndTime.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
			// 
			// 
			// 
			this.dtpEndTime.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dtpEndTime.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
			this.dtpEndTime.MonthCalendar.ClearButtonVisible = true;
			// 
			// 
			// 
			this.dtpEndTime.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
			this.dtpEndTime.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
			this.dtpEndTime.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
			this.dtpEndTime.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.dtpEndTime.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
			this.dtpEndTime.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
			this.dtpEndTime.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dtpEndTime.MonthCalendar.DisplayMonth = new System.DateTime(2016, 7, 1, 0, 0, 0, 0);
			this.dtpEndTime.MonthCalendar.MarkedDates = new System.DateTime[0];
			this.dtpEndTime.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
			// 
			// 
			// 
			this.dtpEndTime.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.dtpEndTime.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
			this.dtpEndTime.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.dtpEndTime.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dtpEndTime.MonthCalendar.TodayButtonVisible = true;
			this.dtpEndTime.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
			this.dtpEndTime.Name = "dtpEndTime";
			this.dtpEndTime.Size = new System.Drawing.Size(271, 26);
			this.dtpEndTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.dtpEndTime.TabIndex = 289;
			this.dtpEndTime.WatermarkText = "结束时间";
			// 
			// BtnMine
			// 
			this.BtnMine.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.BtnMine.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.BtnMine.Location = new System.Drawing.Point(374, 10);
			this.BtnMine.Name = "BtnMine";
			this.BtnMine.Size = new System.Drawing.Size(25, 25);
			this.BtnMine.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.BtnMine.TabIndex = 235;
			this.BtnMine.Text = "选";
			this.BtnMine.Click += new System.EventHandler(this.BtnMine_Click);
			// 
			// txt_MineName
			// 
			this.txt_MineName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.txt_MineName.Border.Class = "TextBoxBorder";
			this.txt_MineName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txt_MineName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txt_MineName.ForeColor = System.Drawing.Color.White;
			this.txt_MineName.Location = new System.Drawing.Point(128, 9);
			this.txt_MineName.Name = "txt_MineName";
			this.txt_MineName.ReadOnly = true;
			this.txt_MineName.Size = new System.Drawing.Size(271, 27);
			this.txt_MineName.TabIndex = 234;
			// 
			// labelX4
			// 
			this.labelX4.AutoSize = true;
			// 
			// 
			// 
			this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX4.ForeColor = System.Drawing.Color.White;
			this.labelX4.Location = new System.Drawing.Point(83, 10);
			this.labelX4.Name = "labelX4";
			this.labelX4.Size = new System.Drawing.Size(40, 24);
			this.labelX4.TabIndex = 233;
			this.labelX4.Text = "矿点";
			// 
			// panelEx1
			// 
			this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
			this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.panelEx1.Controls.Add(this.btnSubmit);
			this.panelEx1.Controls.Add(this.btnCancel);
			this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelEx1.Location = new System.Drawing.Point(3, 207);
			this.panelEx1.Name = "panelEx1";
			this.panelEx1.Size = new System.Drawing.Size(466, 34);
			this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.panelEx1.Style.GradientAngle = 90;
			this.panelEx1.TabIndex = 2;
			// 
			// btnSubmit
			// 
			this.btnSubmit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnSubmit.Location = new System.Drawing.Point(305, 6);
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.Size = new System.Drawing.Size(75, 23);
			this.btnSubmit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnSubmit.TabIndex = 5;
			this.btnSubmit.Text = "保  存";
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnCancel.Location = new System.Drawing.Point(386, 6);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "取  消";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// labelX2
			// 
			this.labelX2.AutoSize = true;
			// 
			// 
			// 
			this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX2.ForeColor = System.Drawing.Color.White;
			this.labelX2.Location = new System.Drawing.Point(67, 145);
			this.labelX2.Name = "labelX2";
			this.labelX2.Size = new System.Drawing.Size(56, 24);
			this.labelX2.TabIndex = 292;
			this.labelX2.Text = "采样机";
			// 
			// cmbSampler
			// 
			this.cmbSampler.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.cmbSampler.DisplayMember = "Text";
			this.cmbSampler.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cmbSampler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSampler.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.cmbSampler.ForeColor = System.Drawing.Color.White;
			this.cmbSampler.FormattingEnabled = true;
			this.cmbSampler.ItemHeight = 23;
			this.cmbSampler.Location = new System.Drawing.Point(127, 140);
			this.cmbSampler.Name = "cmbSampler";
			this.cmbSampler.Size = new System.Drawing.Size(271, 29);
			this.cmbSampler.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.cmbSampler.TabIndex = 293;
			// 
			// FrmSetSampler_Oper
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(472, 244);
			this.Controls.Add(this.tableLayoutPanel1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.Name = "FrmSetSampler_Oper";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "采样设置详情";
			this.Load += new System.EventHandler(this.FrmAppletLog_Oper_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panelEx2.ResumeLayout(false);
			this.panelEx2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dtpStartTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtpEndTime)).EndInit();
			this.panelEx1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.PanelEx panelEx2;
		private DevComponents.DotNetBar.PanelEx panelEx1;
		private DevComponents.DotNetBar.ButtonX btnSubmit;
		private DevComponents.DotNetBar.ButtonX btnCancel;
		private DevComponents.DotNetBar.ButtonX BtnMine;
		private DevComponents.DotNetBar.Controls.TextBoxX txt_MineName;
		private DevComponents.DotNetBar.LabelX labelX4;
		private DevComponents.Editors.DateTimeAdv.DateTimeInput dtpStartTime;
		private DevComponents.Editors.DateTimeAdv.DateTimeInput dtpEndTime;
		private DevComponents.DotNetBar.LabelX labelX1;
		private DevComponents.DotNetBar.LabelX labelX2;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSampler;
	}
}