namespace CMCS.DumblyConcealer.Win
{
    partial class MDIParent
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIParent));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsmiTasks = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmDataHandler = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmWeightBridger = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmBeltBalancer = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmTrainWeight = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmAssayDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmCarSampler = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmAutoMaker = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmCarDeviationRoute = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmCarSpeedRoute = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmCarStopTime = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFrmSyncNetData = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslblVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.车辆报修监测ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTasks,
            this.windowsMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.MdiWindowListItem = this.windowsMenu;
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(632, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // tsmiTasks
            // 
            this.tsmiTasks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpenFrmDataHandler,
            this.tsmiOpenFrmWeightBridger,
            this.tsmiOpenFrmBeltBalancer,
            this.tsmiOpenFrmTrainWeight,
            this.tsmiOpenFrmAssayDevice,
            this.tsmiOpenFrmCarSampler,
            this.tsmiOpenFrmAutoMaker,
            this.tsmiOpenFrmCarDeviationRoute,
            this.tsmiOpenFrmCarSpeedRoute,
            this.tsmiOpenFrmCarStopTime,
            this.tsmiOpenFrmSyncNetData,
            this.车辆报修监测ToolStripMenuItem});
            this.tsmiTasks.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.tsmiTasks.Name = "tsmiTasks";
            this.tsmiTasks.Size = new System.Drawing.Size(59, 20);
            this.tsmiTasks.Text = "任务(&T)";
            // 
            // tsmiOpenFrmDataHandler
            // 
            this.tsmiOpenFrmDataHandler.Name = "tsmiOpenFrmDataHandler";
            this.tsmiOpenFrmDataHandler.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmDataHandler.Text = "01.综合事件处理";
            this.tsmiOpenFrmDataHandler.Click += new System.EventHandler(this.tsmiOpenFrmDataHandler_Click);
            // 
            // tsmiOpenFrmWeightBridger
            // 
            this.tsmiOpenFrmWeightBridger.Name = "tsmiOpenFrmWeightBridger";
            this.tsmiOpenFrmWeightBridger.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmWeightBridger.Text = "02.同步轨道衡数据接口";
            this.tsmiOpenFrmWeightBridger.Click += new System.EventHandler(this.tsmiOpenFrmWeightBridger_Click);
            // 
            // tsmiOpenFrmBeltBalancer
            // 
            this.tsmiOpenFrmBeltBalancer.Name = "tsmiOpenFrmBeltBalancer";
            this.tsmiOpenFrmBeltBalancer.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmBeltBalancer.Text = "03.皮带秤称重数据同步";
            this.tsmiOpenFrmBeltBalancer.Click += new System.EventHandler(this.tsmiOpenFrmBeltBalancer_Click);
            // 
            // tsmiOpenFrmTrainWeight
            // 
            this.tsmiOpenFrmTrainWeight.Name = "tsmiOpenFrmTrainWeight";
            this.tsmiOpenFrmTrainWeight.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmTrainWeight.Text = "04.车号识别报文TCP/IP同步业务";
            this.tsmiOpenFrmTrainWeight.Click += new System.EventHandler(this.tsmiOpenFrmTrainWeight_Click);
            // 
            // tsmiOpenFrmAssayDevice
            // 
            this.tsmiOpenFrmAssayDevice.Name = "tsmiOpenFrmAssayDevice";
            this.tsmiOpenFrmAssayDevice.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmAssayDevice.Text = "05.化验设备数据同步业务";
            this.tsmiOpenFrmAssayDevice.Click += new System.EventHandler(this.tsmiOpenFrmAssayDevice_Click);
            // 
            // tsmiOpenFrmCarSampler
            // 
            this.tsmiOpenFrmCarSampler.Name = "tsmiOpenFrmCarSampler";
            this.tsmiOpenFrmCarSampler.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmCarSampler.Text = "06.汽车机械采样机接口";
            this.tsmiOpenFrmCarSampler.Click += new System.EventHandler(this.tsmiOpenFrmCarSampler_Click);
            // 
            // tsmiOpenFrmAutoMaker
            // 
            this.tsmiOpenFrmAutoMaker.Name = "tsmiOpenFrmAutoMaker";
            this.tsmiOpenFrmAutoMaker.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmAutoMaker.Text = "07.全自动制样机接口";
            this.tsmiOpenFrmAutoMaker.Click += new System.EventHandler(this.tsmiOpenFrmAutoMaker_Click);
            // 
            // tsmiOpenFrmCarDeviationRoute
            // 
            this.tsmiOpenFrmCarDeviationRoute.Name = "tsmiOpenFrmCarDeviationRoute";
            this.tsmiOpenFrmCarDeviationRoute.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmCarDeviationRoute.Text = "08.车辆路线偏移监测";
            this.tsmiOpenFrmCarDeviationRoute.Click += new System.EventHandler(this.tsmiOpenFrmCarDeviationRoute_Click);
            // 
            // tsmiOpenFrmCarSpeedRoute
            // 
            this.tsmiOpenFrmCarSpeedRoute.Name = "tsmiOpenFrmCarSpeedRoute";
            this.tsmiOpenFrmCarSpeedRoute.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmCarSpeedRoute.Text = "09.车速异常检测";
            this.tsmiOpenFrmCarSpeedRoute.Click += new System.EventHandler(this.tsmiOpenFrmCarSpeedRoute_Click);
            // 
            // tsmiOpenFrmCarStopTime
            // 
            this.tsmiOpenFrmCarStopTime.Name = "tsmiOpenFrmCarStopTime";
            this.tsmiOpenFrmCarStopTime.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmCarStopTime.Text = "10.车辆异常停留监测";
            this.tsmiOpenFrmCarStopTime.Click += new System.EventHandler(this.tsmiOpenFrmCarStopTime_Click);
            // 
            // tsmiOpenFrmSyncNetData
            // 
            this.tsmiOpenFrmSyncNetData.Name = "tsmiOpenFrmSyncNetData";
            this.tsmiOpenFrmSyncNetData.Size = new System.Drawing.Size(244, 22);
            this.tsmiOpenFrmSyncNetData.Text = "11.内外网数据同步业务";
            this.tsmiOpenFrmSyncNetData.Click += new System.EventHandler(this.tsmiOpenFrmSyncNetData_Click);
            // 
            // windowsMenu
            // 
            this.windowsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeToolStripMenuItem,
            this.tileVerticalToolStripMenuItem,
            this.tileHorizontalToolStripMenuItem});
            this.windowsMenu.Name = "windowsMenu";
            this.windowsMenu.Size = new System.Drawing.Size(59, 20);
            this.windowsMenu.Text = "窗口(&W)";
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.cascadeToolStripMenuItem.Text = "层叠(&C)";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.CascadeToolStripMenuItem_Click);
            // 
            // tileVerticalToolStripMenuItem
            // 
            this.tileVerticalToolStripMenuItem.Name = "tileVerticalToolStripMenuItem";
            this.tileVerticalToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.tileVerticalToolStripMenuItem.Text = "垂直平铺(&V)";
            this.tileVerticalToolStripMenuItem.Click += new System.EventHandler(this.TileVerticalToolStripMenuItem_Click);
            // 
            // tileHorizontalToolStripMenuItem
            // 
            this.tileHorizontalToolStripMenuItem.Name = "tileHorizontalToolStripMenuItem";
            this.tileHorizontalToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.tileHorizontalToolStripMenuItem.Text = "水平平铺(&H)";
            this.tileHorizontalToolStripMenuItem.Click += new System.EventHandler(this.TileHorizontalToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.tsslblVersion});
            this.statusStrip.Location = new System.Drawing.Point(0, 396);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(632, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(41, 17);
            this.toolStripStatusLabel.Text = "版本：";
            // 
            // tsslblVersion
            // 
            this.tsslblVersion.Name = "tsslblVersion";
            this.tsslblVersion.Size = new System.Drawing.Size(47, 17);
            this.tsslblVersion.Text = "1.0.0.0";
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // 车辆报修监测ToolStripMenuItem
            // 
            this.车辆报修监测ToolStripMenuItem.Name = "车辆报修监测ToolStripMenuItem";
            this.车辆报修监测ToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.车辆报修监测ToolStripMenuItem.Text = "12.车辆报修监测";
            this.车辆报修监测ToolStripMenuItem.Click += new System.EventHandler(this.tsmiOpenFrmCarRepair_Click);
            // 
            // MDIParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 418);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MDIParent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "燃料集中管控后台处理程序";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MDIParent1_FormClosing);
            this.Shown += new System.EventHandler(this.MDIParent1_Shown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem tileHorizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiTasks;
        private System.Windows.Forms.ToolStripMenuItem windowsMenu;
        private System.Windows.Forms.ToolStripMenuItem cascadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileVerticalToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripStatusLabel tsslblVersion;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmDataHandler;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmWeightBridger;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmBeltBalancer;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmTrainWeight;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmAssayDevice;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmCarSampler;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmAutoMaker;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmCarDeviationRoute;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmCarSpeedRoute;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmCarStopTime;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFrmSyncNetData;
        private System.Windows.Forms.ToolStripMenuItem 车辆报修监测ToolStripMenuItem;
    }
}



