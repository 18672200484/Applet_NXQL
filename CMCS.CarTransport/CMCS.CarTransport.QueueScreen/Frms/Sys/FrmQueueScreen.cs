using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.CarTransport.QueueScreen.UserControls;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.DAO;
using CMCS.Common;
using CMCS.Common.Utilities;
using CMCS.Common.Enums;

namespace CMCS.CarTransport.QueueScreen.Frms.Sys
{
    public partial class FrmQueueScreen : Form
    {
        /// <summary>
        /// 自动滚动偏移量
        /// </summary>
        int Offset = 0;

        CommonDAO commonDAO = CommonDAO.GetInstance();

        public FrmQueueScreen()
        {
            InitializeComponent();
        }

        private void FrmQueueScreen_Load(object sender, EventArgs e)
        {
            LoadTotalInfo();

            LoadContent();
        }

        private void FrmQueueScreen_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (MessageBox.Show("确定退出系统!", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                    Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScrollContent();
        }

        /// <summary>
        /// 加载汇总信息
        /// </summary>
        private void LoadTotalInfo()
        {
            //今日有效运输记录
            DateTime dtStart = DateTime.Now.Date;
            DateTime dtEnd = dtStart.AddDays(1);
            List<CmcsBuyFuelTransport> todayList = commonDAO.SelfDber.Entities<CmcsBuyFuelTransport>("where IsUse=1 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd", new { dtStart = dtStart, dtEnd = dtEnd });

            //今日入厂总车数
            label2.Text = todayList.Count() + "车";

            //今日入厂总矿点数
            label7.Text = todayList.GroupBy(a => a.MineName).Count() + "个";

            //今日采样总车数
            label9.Text = todayList.Where(a => !string.IsNullOrWhiteSpace(a.SamplePlace)).Count() + "车";

            //今日出厂总车数
            label13.Text = todayList.Where(a => a.IsFinish == 1).Count() + "车";

            //今日来煤总量
            label11.Text = todayList.Sum(a => a.SuttleWeight) + "吨";

            string samplerMachineCode = "#1机械采样机";
            string systemStatus = commonDAO.GetSignalDataValue(samplerMachineCode, eSignalDataName.设备状态.ToString());
            eEquInfSamplerSystemStatus result;
            if (Enum.TryParse(systemStatus, out result))
                label20.Text = GetSamplerSystemStatus(result);

            samplerMachineCode = "#2机械采样机";
            systemStatus = commonDAO.GetSignalDataValue(samplerMachineCode, eSignalDataName.设备状态.ToString());
            if (Enum.TryParse(systemStatus, out result))
                label18.Text = GetSamplerSystemStatus(result);

            samplerMachineCode = "#3机械采样机";
            systemStatus = commonDAO.GetSignalDataValue(samplerMachineCode, eSignalDataName.设备状态.ToString());
            if (Enum.TryParse(systemStatus, out result))
                label6.Text = GetSamplerSystemStatus(result);
        }

        private string GetSamplerSystemStatus(eEquInfSamplerSystemStatus result)
        {
            if (result == eEquInfSamplerSystemStatus.就绪待机)
                return "空闲";
            else if (result == eEquInfSamplerSystemStatus.发生故障)
                return "故障";
            else if (result == eEquInfSamplerSystemStatus.系统停止)
                return "停止";
            else if (result == eEquInfSamplerSystemStatus.正在运行)
                return "运行";

            return "未知";
        }

        /// <summary>
        /// 加载滚动内容
        /// </summary>
        private void LoadContent()
        {
            //每次加载清除内容
            panelIn.Controls.Clear();

            int i = 0;
            int TotalHeight = 0;

			//List<CmcsInNetTransport> list = commonDAO.SelfDber.Entities<CmcsInNetTransport>("where (StepName='矿发' or StepName='在途') and IDCard is not null");
			List<CmcsBuyFuelTransport> list = commonDAO.SelfDber.Entities<CmcsBuyFuelTransport>("where IsFinish=0 order by CreationTime ");
			foreach (var item in list.GroupBy(a => new Tuple<string, string>(a.MineName, a.FuelKindName)).OrderBy(a => a.Key))
            {
                UCtrlMineInfo mineInfo = new UCtrlMineInfo();
                mineInfo.Title = item.Key.Item1 + "：" + item.Key.Item2 + "/" + item.Count() + "车/" + item.Sum(a => a.TicketWeight) + "吨";
                mineInfo.Content = item.Select(a => a.CarNumber).ToList();
                mineInfo.Location = new Point(1, 1 + TotalHeight);
                if ((i % 2) == 0)
                    mineInfo.CtlFontColor = Color.Orange;
                else
                    mineInfo.CtlFontColor = Color.White;

                TotalHeight += mineInfo.Height;

                panelIn.Controls.Add(mineInfo);

                i++;
            }
            panelIn.Height = TotalHeight;

            //动态设置每次滚动偏移量（一次滚动一页）
            //vSBarMain.Maximum = panelIn.Height - panelOut.Height;
            //if (vSBarMain.Maximum > panelOut.Height)
            //    Offset = panelOut.Height;
            //else
            //    Offset = vSBarMain.Maximum;

            //固定滚动偏移量
            Offset = 20;

            BindFromMoveEvent(this);
        }

        /// <summary>
        /// 滚动内容
        /// </summary>
        /// <returns>完成一次滚动</returns>
        private bool ScrollContent()
        {
            if (vSBarMain.Value >= vSBarMain.Maximum)
            {
                panelIn.Top = -1;
                vSBarMain.Value = 0;
                return true;
            }
            else
            {
                if (vSBarMain.Maximum - vSBarMain.Value > Offset)
                    vSBarMain.Value += Offset;
                else
                    vSBarMain.Value += vSBarMain.Maximum - vSBarMain.Value;

                return false;
            }
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vSBarMain_ValueChanged(object sender, EventArgs e)
        {
            panelIn.Top = -vSBarMain.Value - 1;
        }

        /// <summary>
        /// 定时请求数据及页面刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            try
            {
                //10秒滚动一页，完成一次后刷新数据
                if (DateTime.Now.Second % 3 == 0)
                {
                    if (ScrollContent())
                    {
                        LoadTotalInfo();
                        LoadContent();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("timer1_Tick", ex);
            }
            finally
            {
                timer1.Start();
            }
        }

        /// <summary>
        /// 显示当前时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_CurrentTime_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
        }

        #region 移动窗体 移动窗口

        /// <summary>
        /// 绑定窗体移动事件
        /// </summary>
        private void BindFromMoveEvent(Control ctl)
        {
            foreach (Control item in ctl.Controls)
            {
                if (item.Controls.Count > 0) BindFromMoveEvent(item);

                item.MouseDown += new MouseEventHandler(ctl_MouseDown);
                item.MouseMove += new MouseEventHandler(ctl_MouseMove);
            }
        }

        private Point _mousePoint;
        private int topA(Control cc)
        {
            if (cc == null || cc == this) return 0;
            if (cc.Parent == null || cc.Parent == this)
                return cc.Top;
            else
                return topA(cc.Parent) + cc.Top;
        }

        private int leftA(Control cc)
        {
            if (cc == null || cc == this) return 0;
            if (cc.Parent == null || cc.Parent == this)
                return cc.Left;
            else
                return leftA(cc.Parent) + cc.Left;
        }

        private void ctl_MouseDown(object sender, MouseEventArgs e)
        {
            Control cc = (Control)sender;
            if (e.Button == MouseButtons.Left)
            {
                _mousePoint.X = e.X + leftA(cc);
                _mousePoint.Y = e.Y + topA(cc);
            }
        }

        private void ctl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Top = MousePosition.Y - _mousePoint.Y;
                Left = MousePosition.X - _mousePoint.X;
            }
        }
        #endregion

    }
}
