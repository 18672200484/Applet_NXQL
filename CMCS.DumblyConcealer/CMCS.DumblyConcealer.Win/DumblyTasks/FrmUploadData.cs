using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using CMCS.DumblyConcealer.Win.Core;
using CMCS.Common.Utilities;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.UploadData;
using CMCS.DumblyConcealer.Utilities;
using CMCS.DumblyConcealer.Tasks.UploadData.Entities;

namespace CMCS.DumblyConcealer.Win.DumblyTasks
{
	public partial class FrmUploadData : TaskForm
	{
		RTxtOutputer rTxtOutputer;
		TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();
		Boolean isExeFinish = true;
		Boolean isBaseLogExeFinish = true;
		Boolean isAssayExeFinish = true;

		public FrmUploadData()
		{
			InitializeComponent();
		}

		private void FrmUploadData_Load(object sender, EventArgs e)
		{
			this.rTxtOutputer = new RTxtOutputer(rtxtOutput);
			ExecuteAllTask();
		}

		/// <summary>
		/// 执行所有任务
		/// </summary>
		void ExecuteAllTask()
		{
			UploadXmlHelper xmlHelper = new UploadXmlHelper();
			List<TableOrView> list = xmlHelper.LoadConfig().Where(a => a.IsUse.ToLower() == "true").ToList();
			if (list.Count <= 0)
			{
				this.rTxtOutputer.Output("未进行相关配置，请在【UploadData.AppConfig.xml】中进行配置后重新打开程序！", eOutputType.Error);
				return;
			}

			UploadDataDAO dao = UploadDataDAO.GetInstance();
			taskSimpleScheduler.StartNewTask("数据同步(无人值守<-->智仁)", () =>
			{
				if (isExeFinish)
				{
					isExeFinish = false;
					//执行任务
					dao.TransferData(list, rTxtOutputer.Output);
					isExeFinish = true;
				}
			}, 60 * 1000, OutputError);//一分钟一次，上报不需要那么快


			taskSimpleScheduler.StartNewTask("数据同步【基础信息变更日志】(智仁-->无人值守)", () =>
			{
				if (isBaseLogExeFinish)
				{
					isBaseLogExeFinish = false;
					//执行任务
					dao.TransferBaseOperLog(rTxtOutputer.Output);
					isBaseLogExeFinish = true;
				}
			}, 60 * 1000, BaseLogOutputError);

			taskSimpleScheduler.StartNewTask("数据同步【化验数据】(智仁-->无人值守)", () =>
			{
				if (isAssayExeFinish)
				{
					isAssayExeFinish = false;
					//执行任务
					dao.TransferAssayQulity(rTxtOutputer.Output);
					isAssayExeFinish = true;
				}
			}, 60 * 1000, AssayOutputError);
		}


		/// <summary>
		/// 输出异常信息
		/// </summary>
		/// <param name="text"></param>
		/// <param name="ex"></param>
		void OutputError(string text, Exception ex)
		{
			this.isExeFinish = true;
			this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
		}

		/// <summary>
		/// 输出异常信息
		/// </summary>
		/// <param name="text"></param>
		/// <param name="ex"></param>
		void BaseLogOutputError(string text, Exception ex)
		{
			this.isBaseLogExeFinish = true;
			this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
		}

		/// <summary>
		/// 输出异常信息
		/// </summary>
		/// <param name="text"></param>
		/// <param name="ex"></param>
		void AssayOutputError(string text, Exception ex)
		{
			this.isAssayExeFinish = true;
			this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
		}

		/// <summary>
		/// 窗体关闭后
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmUploadData_FormClosed(object sender, FormClosedEventArgs e)
		{
			// 注意：必须取消任务
			this.taskSimpleScheduler.Cancal();
		}
	}
}
