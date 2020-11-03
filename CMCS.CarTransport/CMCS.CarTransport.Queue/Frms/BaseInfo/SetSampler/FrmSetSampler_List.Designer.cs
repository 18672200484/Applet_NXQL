namespace CMCS.CarTransport.Queue.Frms.BaseInfo.SetSampler
{
	partial class FrmSetSampler_List
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
			DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn1 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
			DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn2 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
			DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn3 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
			DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn4 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
			DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn5 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
			DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn6 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
			DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn7 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
			DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn8 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
			DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn9 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
			this.superGridControl1 = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
			this.btnPrevious = new DevComponents.DotNetBar.ButtonX();
			this.btnFirst = new DevComponents.DotNetBar.ButtonX();
			this.btnLast = new DevComponents.DotNetBar.ButtonX();
			this.btnNext = new DevComponents.DotNetBar.ButtonX();
			this.btnSearch = new DevComponents.DotNetBar.ButtonX();
			this.panel2 = new System.Windows.Forms.Panel();
			this.lblPagerInfo = new DevComponents.DotNetBar.LabelX();
			this.superGridControl2 = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.labelX1 = new DevComponents.DotNetBar.LabelX();
			this.btnAdd = new DevComponents.DotNetBar.ButtonX();
			this.dtpEndTime = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
			this.dtpStartTime = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
			this.txtContent = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dtpEndTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtpStartTime)).BeginInit();
			this.SuspendLayout();
			// 
			// superGridControl1
			// 
			this.superGridControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			this.superGridControl1.DefaultVisualStyles.CellStyles.Default.Alignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
			this.superGridControl1.DefaultVisualStyles.CellStyles.Default.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.superGridControl1.DefaultVisualStyles.ColumnHeaderStyles.Default.Font = new System.Drawing.Font("Segoe UI", 14.25F);
			this.superGridControl1.DefaultVisualStyles.HeaderStyles.Default.Font = new System.Drawing.Font("宋体", 11.25F);
			this.superGridControl1.DefaultVisualStyles.RowStyles.Default.RowHeaderStyle.Font = new System.Drawing.Font("宋体", 11.25F);
			this.superGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.superGridControl1.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
			this.superGridControl1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.superGridControl1.ForeColor = System.Drawing.Color.White;
			this.superGridControl1.Location = new System.Drawing.Point(0, 0);
			this.superGridControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.superGridControl1.Name = "superGridControl1";
			this.superGridControl1.PrimaryGrid.AutoGenerateColumns = false;
			this.superGridControl1.PrimaryGrid.Caption.Text = "";
			gridColumn1.CellStyles.Default.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Underline);
			gridColumn1.DefaultNewRowCellValue = "查看";
			gridColumn1.HeaderText = "";
			gridColumn1.InfoImageAlignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
			gridColumn1.Name = "clmShow";
			gridColumn1.NullString = "查看";
			gridColumn1.Width = 50;
			gridColumn2.CellStyles.Default.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Underline);
			gridColumn2.DataPropertyName = "修改";
			gridColumn2.HeaderText = "";
			gridColumn2.Name = "clmEdit";
			gridColumn2.NullString = "修改";
			gridColumn2.Width = 50;
			gridColumn3.CellStyles.Default.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Underline);
			gridColumn3.DataPropertyName = "删除";
			gridColumn3.HeaderText = "";
			gridColumn3.Name = "clmDelete";
			gridColumn3.NullString = "删除";
			gridColumn3.Width = 50;
			gridColumn4.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.AllCells;
			gridColumn4.DataPropertyName = "MineName";
			gridColumn4.HeaderText = "矿点";
			gridColumn4.Name = "";
			gridColumn4.Width = 200;
			gridColumn5.CellStyles.Default.Alignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
			gridColumn5.DataPropertyName = "StartTime";
			gridColumn5.HeaderText = "开始时间";
			gridColumn5.InfoImageAlignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
			gridColumn5.MinimumWidth = 150;
			gridColumn5.Name = "";
			gridColumn5.Width = 200;
			gridColumn6.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.AllCells;
			gridColumn6.CellStyles.Default.Alignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
			gridColumn6.DataPropertyName = "EndTime";
			gridColumn6.FilterPopupMaxItems = 200;
			gridColumn6.HeaderText = "结束时间";
			gridColumn6.InfoImageAlignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
			gridColumn6.Name = "";
			gridColumn6.Width = 200;
			gridColumn7.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.AllCells;
			gridColumn7.DataPropertyName = "Sampler";
			gridColumn7.HeaderText = "采样机";
			gridColumn7.Name = "";
			gridColumn7.Width = 150;
			gridColumn8.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.AllCells;
			gridColumn8.DataPropertyName = "CreationTime";
			gridColumn8.HeaderText = "创建时间";
			gridColumn8.Name = "";
			gridColumn9.DataPropertyName = "Id";
			gridColumn9.Name = "clmId";
			gridColumn9.Visible = false;
			this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn1);
			this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn2);
			this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn3);
			this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn4);
			this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn5);
			this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn6);
			this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn7);
			this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn8);
			this.superGridControl1.PrimaryGrid.Columns.Add(gridColumn9);
			this.superGridControl1.PrimaryGrid.DefaultRowHeight = 0;
			this.superGridControl1.PrimaryGrid.InitialSelection = DevComponents.DotNetBar.SuperGrid.RelativeSelection.Row;
			this.superGridControl1.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
			this.superGridControl1.Size = new System.Drawing.Size(1369, 611);
			this.superGridControl1.TabIndex = 4;
			this.superGridControl1.Text = "superGridControl1";
			this.superGridControl1.CellMouseDown += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs>(this.superGridControl1_CellMouseDown);
			this.superGridControl1.DataBindingComplete += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs>(this.superGridControl1_DataBindingComplete);
			this.superGridControl1.BeginEdit += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridEditEventArgs>(this.superGridControl1_BeginEdit);
			this.superGridControl1.GetRowHeaderText += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs>(this.superGridControl_GetRowHeaderText);
			// 
			// btnPrevious
			// 
			this.btnPrevious.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrevious.CommandParameter = "Previous";
			this.btnPrevious.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnPrevious.Location = new System.Drawing.Point(1154, 9);
			this.btnPrevious.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(64, 23);
			this.btnPrevious.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnPrevious.TabIndex = 104;
			this.btnPrevious.Text = "<";
			this.btnPrevious.Click += new System.EventHandler(this.btnPagerCommand_Click);
			// 
			// btnFirst
			// 
			this.btnFirst.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFirst.CommandParameter = "First";
			this.btnFirst.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnFirst.Location = new System.Drawing.Point(1084, 9);
			this.btnFirst.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.Size = new System.Drawing.Size(64, 23);
			this.btnFirst.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnFirst.TabIndex = 103;
			this.btnFirst.Text = "|<";
			this.btnFirst.Click += new System.EventHandler(this.btnPagerCommand_Click);
			// 
			// btnLast
			// 
			this.btnLast.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLast.CommandParameter = "Last";
			this.btnLast.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnLast.Location = new System.Drawing.Point(1293, 9);
			this.btnLast.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnLast.Name = "btnLast";
			this.btnLast.Size = new System.Drawing.Size(64, 23);
			this.btnLast.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnLast.TabIndex = 101;
			this.btnLast.Text = ">|";
			this.btnLast.Click += new System.EventHandler(this.btnPagerCommand_Click);
			// 
			// btnNext
			// 
			this.btnNext.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNext.CommandParameter = "Next";
			this.btnNext.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.btnNext.Location = new System.Drawing.Point(1223, 9);
			this.btnNext.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(64, 23);
			this.btnNext.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnNext.TabIndex = 100;
			this.btnNext.Text = ">";
			this.btnNext.Click += new System.EventHandler(this.btnPagerCommand_Click);
			// 
			// btnSearch
			// 
			this.btnSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSearch.Location = new System.Drawing.Point(1293, 9);
			this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(64, 23);
			this.btnSearch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnSearch.TabIndex = 13;
			this.btnSearch.Text = "搜 索";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
			this.panel2.Controls.Add(this.btnPrevious);
			this.panel2.Controls.Add(this.btnFirst);
			this.panel2.Controls.Add(this.btnLast);
			this.panel2.Controls.Add(this.btnNext);
			this.panel2.Controls.Add(this.lblPagerInfo);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.ForeColor = System.Drawing.Color.White;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Margin = new System.Windows.Forms.Padding(2);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1369, 40);
			this.panel2.TabIndex = 1;
			// 
			// lblPagerInfo
			// 
			this.lblPagerInfo.AutoSize = true;
			this.lblPagerInfo.BackColor = System.Drawing.Color.Transparent;
			// 
			// 
			// 
			this.lblPagerInfo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.lblPagerInfo.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.lblPagerInfo.ForeColor = System.Drawing.Color.White;
			this.lblPagerInfo.Location = new System.Drawing.Point(7, 8);
			this.lblPagerInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.lblPagerInfo.Name = "lblPagerInfo";
			this.lblPagerInfo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblPagerInfo.Size = new System.Drawing.Size(338, 24);
			this.lblPagerInfo.TabIndex = 99;
			this.lblPagerInfo.Text = "共 0 条记录，每页20 条，共 0 页，当前第 0 页";
			// 
			// superGridControl2
			// 
			this.superGridControl2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			this.superGridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.superGridControl2.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
			this.superGridControl2.ForeColor = System.Drawing.Color.White;
			this.superGridControl2.Location = new System.Drawing.Point(0, 0);
			this.superGridControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.superGridControl2.Name = "superGridControl2";
			this.superGridControl2.PrimaryGrid.Caption.Text = "";
			this.superGridControl2.Size = new System.Drawing.Size(1369, 611);
			this.superGridControl2.TabIndex = 3;
			this.superGridControl2.Text = "superGridControl1";
			// 
			// splitContainer2
			// 
			this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer2.ForeColor = System.Drawing.Color.White;
			this.splitContainer2.IsSplitterFixed = true;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.BackColor = System.Drawing.Color.Transparent;
			this.splitContainer2.Panel1.Controls.Add(this.superGridControl1);
			this.splitContainer2.Panel1.Controls.Add(this.superGridControl2);
			this.splitContainer2.Panel1.ForeColor = System.Drawing.Color.White;
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.BackColor = System.Drawing.Color.Transparent;
			this.splitContainer2.Panel2.Controls.Add(this.panel2);
			this.splitContainer2.Panel2.ForeColor = System.Drawing.Color.White;
			this.splitContainer2.Panel2MinSize = 40;
			this.splitContainer2.Size = new System.Drawing.Size(1369, 652);
			this.splitContainer2.SplitterDistance = 611;
			this.splitContainer2.SplitterWidth = 1;
			this.splitContainer2.TabIndex = 0;
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.ForeColor = System.Drawing.Color.White;
			this.splitContainer1.Location = new System.Drawing.Point(0, 1);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
			this.splitContainer1.Panel1.Controls.Add(this.panel1);
			this.splitContainer1.Panel1.ForeColor = System.Drawing.Color.White;
			this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Panel2.ForeColor = System.Drawing.Color.White;
			this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainer1.Size = new System.Drawing.Size(1369, 693);
			this.splitContainer1.SplitterDistance = 40;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 147;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
			this.panel1.Controls.Add(this.labelX1);
			this.panel1.Controls.Add(this.btnAdd);
			this.panel1.Controls.Add(this.dtpEndTime);
			this.panel1.Controls.Add(this.dtpStartTime);
			this.panel1.Controls.Add(this.txtContent);
			this.panel1.Controls.Add(this.btnSearch);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.ForeColor = System.Drawing.Color.White;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1369, 40);
			this.panel1.TabIndex = 12;
			// 
			// labelX1
			// 
			this.labelX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelX1.BackColor = System.Drawing.Color.Transparent;
			// 
			// 
			// 
			this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX1.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.labelX1.ForeColor = System.Drawing.Color.White;
			this.labelX1.Location = new System.Drawing.Point(676, 9);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(75, 23);
			this.labelX1.TabIndex = 275;
			this.labelX1.Text = "创建时间";
			// 
			// btnAdd
			// 
			this.btnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAdd.Location = new System.Drawing.Point(7, 9);
			this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(64, 23);
			this.btnAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnAdd.TabIndex = 274;
			this.btnAdd.Text = "新 增";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// dtpEndTime
			// 
			this.dtpEndTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpEndTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.dtpEndTime.BackgroundStyle.Class = "DateTimeInputBackground";
			this.dtpEndTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dtpEndTime.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
			this.dtpEndTime.ButtonDropDown.Visible = true;
			this.dtpEndTime.CustomFormat = "yyyy-MM-dd";
			this.dtpEndTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.dtpEndTime.ForeColor = System.Drawing.Color.White;
			this.dtpEndTime.Format = DevComponents.Editors.eDateTimePickerFormat.Custom;
			this.dtpEndTime.IsPopupCalendarOpen = false;
			this.dtpEndTime.Location = new System.Drawing.Point(946, 8);
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
			this.dtpEndTime.Size = new System.Drawing.Size(185, 26);
			this.dtpEndTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.dtpEndTime.TabIndex = 272;
			this.dtpEndTime.WatermarkText = "结束时间";
			// 
			// dtpStartTime
			// 
			this.dtpStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpStartTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.dtpStartTime.BackgroundStyle.Class = "DateTimeInputBackground";
			this.dtpStartTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.dtpStartTime.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
			this.dtpStartTime.ButtonDropDown.Visible = true;
			this.dtpStartTime.CustomFormat = "yyyy-MM-dd";
			this.dtpStartTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.dtpStartTime.ForeColor = System.Drawing.Color.White;
			this.dtpStartTime.Format = DevComponents.Editors.eDateTimePickerFormat.Custom;
			this.dtpStartTime.IsPopupCalendarOpen = false;
			this.dtpStartTime.Location = new System.Drawing.Point(757, 8);
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
			this.dtpStartTime.Size = new System.Drawing.Size(183, 26);
			this.dtpStartTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.dtpStartTime.TabIndex = 273;
			this.dtpStartTime.WatermarkText = "开始时间";
			// 
			// txtContent
			// 
			this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
			// 
			// 
			// 
			this.txtContent.Border.Class = "TextBoxBorder";
			this.txtContent.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txtContent.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtContent.ForeColor = System.Drawing.Color.White;
			this.txtContent.Location = new System.Drawing.Point(1137, 7);
			this.txtContent.Name = "txtContent";
			this.txtContent.Size = new System.Drawing.Size(150, 27);
			this.txtContent.TabIndex = 14;
			this.txtContent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtContent.WatermarkImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtContent.WatermarkText = "请输入矿点...";
			// 
			// FrmSetSampler_List
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1370, 695);
			this.Controls.Add(this.splitContainer1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "FrmSetSampler_List";
			this.Text = "采样设置";
			this.Load += new System.EventHandler(this.FrmAppletLog_List_Load);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dtpEndTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtpStartTime)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevComponents.DotNetBar.SuperGrid.SuperGridControl superGridControl1;
		private DevComponents.DotNetBar.ButtonX btnPrevious;
		private DevComponents.DotNetBar.ButtonX btnFirst;
		private DevComponents.DotNetBar.ButtonX btnLast;
		private DevComponents.DotNetBar.ButtonX btnNext;
		private DevComponents.DotNetBar.ButtonX btnSearch;
		private System.Windows.Forms.Panel panel2;
		private DevComponents.DotNetBar.LabelX lblPagerInfo;
		private DevComponents.DotNetBar.SuperGrid.SuperGridControl superGridControl2;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Panel panel1;
		private DevComponents.DotNetBar.Controls.TextBoxX txtContent;
		private DevComponents.Editors.DateTimeAdv.DateTimeInput dtpEndTime;
		private DevComponents.Editors.DateTimeAdv.DateTimeInput dtpStartTime;
		private DevComponents.DotNetBar.ButtonX btnAdd;
		private DevComponents.DotNetBar.LabelX labelX1;
	}
}