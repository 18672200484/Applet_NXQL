﻿using System;
using System.Windows.Forms;
//
using DevComponents.DotNetBar;
using CMCS.Common.DAO;
using DevComponents.DotNetBar.Metro;
using CMCS.CarTransport.Out.Utilities;
using CMCS.CarTransport.Out.Core;
using CMCS.Common.Enums;
using CMCS.Common;

namespace CMCS.CarTransport.Out.Frms.Sys
{
	public partial class FrmMainFrame : MetroForm
	{
		CommonDAO commonDAO = CommonDAO.GetInstance();

		public static SuperTabControlManager superTabControlManager;

		public FrmMainFrame()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			lblVersion.Text = new AU.Updater().Version;

			this.superTabControl1.Tabs.Clear();
			FrmMainFrame.superTabControlManager = new SuperTabControlManager(this.superTabControl1);

			OpenWeight();
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			if (GlobalVars.LoginUser == null) GlobalVars.LoginUser = commonDAO.GetAdminUser_Cmcs();
			if (GlobalVars.LoginUser != null) lblLoginUserName.Text = GlobalVars.LoginUser.UserName;

			commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.系统.ToString(), "1");
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				if (MessageBoxEx.Show("确认退出系统？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.系统.ToString(), "0");

					Application.Exit();
				}
				else
				{
					e.Cancel = true;
				}
			}
		}

		/// <summary>
		/// 退出系统
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnApplicationExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void timer_CurrentTime_Tick(object sender, EventArgs e)
		{
			lblCurrentTime.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
		}

		#region 打开/切换可视主界面

		#region 弹出窗体

		/// <summary>
		/// 打开过衡界面
		/// </summary>
		public void OpenWeight()
		{
			string uniqueKey = FrmOuter.UniqueKey;

			if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
			{
				FrmOuter frm = new FrmOuter();
				FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, false);
			}
			else
				FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
		}

		/// <summary>
		/// 打开参数设置界面
		/// </summary>
		public void OpenSetting()
		{
			FrmSetting frm = new FrmSetting();
			frm.ShowDialog();
		}

		#endregion

		/// <summary>
		/// 打开参数设置界面
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOpenSetting_Click(object sender, EventArgs e)
		{
			OpenSetting();
		}

		/// <summary>
		/// 打开调试窗口
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDebugConsole_Click(object sender, EventArgs e)
		{
			foreach (Form frm in Application.OpenForms)
			{
				if (frm is FrmDebugConsole)
				{
					FrmDebugConsole.GetInstance().Activate();
					return;
				}
			}

			FrmDebugConsole.GetInstance().Show();
		}

		#endregion
	}
}
