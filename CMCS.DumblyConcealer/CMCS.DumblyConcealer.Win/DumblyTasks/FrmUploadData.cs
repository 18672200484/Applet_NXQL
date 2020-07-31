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
		/// ִ����������
		/// </summary>
		void ExecuteAllTask()
		{
			UploadXmlHelper xmlHelper = new UploadXmlHelper();
			List<TableOrView> list = xmlHelper.LoadConfig().Where(a => a.IsUse.ToLower() == "true").ToList();
			if (list.Count <= 0)
			{
				this.rTxtOutputer.Output("δ����������ã����ڡ�UploadData.AppConfig.xml���н������ú����´򿪳���", eOutputType.Error);
				return;
			}

			UploadDataDAO dao = UploadDataDAO.GetInstance();
			taskSimpleScheduler.StartNewTask("����ͬ��(����ֵ��<-->����)", () =>
			{
				if (isExeFinish)
				{
					isExeFinish = false;
					//ִ������
					dao.TransferData(list, rTxtOutputer.Output);
					isExeFinish = true;
				}
			}, 60 * 1000, OutputError);//һ����һ�Σ��ϱ�����Ҫ��ô��


			taskSimpleScheduler.StartNewTask("����ͬ����������Ϣ�����־��(����-->����ֵ��)", () =>
			{
				if (isBaseLogExeFinish)
				{
					isBaseLogExeFinish = false;
					//ִ������
					dao.TransferBaseOperLog(rTxtOutputer.Output);
					isBaseLogExeFinish = true;
				}
			}, 60 * 1000, BaseLogOutputError);

			taskSimpleScheduler.StartNewTask("����ͬ�����������ݡ�(����-->����ֵ��)", () =>
			{
				if (isAssayExeFinish)
				{
					isAssayExeFinish = false;
					//ִ������
					dao.TransferAssayQulity(rTxtOutputer.Output);
					isAssayExeFinish = true;
				}
			}, 60 * 1000, AssayOutputError);
		}


		/// <summary>
		/// ����쳣��Ϣ
		/// </summary>
		/// <param name="text"></param>
		/// <param name="ex"></param>
		void OutputError(string text, Exception ex)
		{
			this.isExeFinish = true;
			this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
		}

		/// <summary>
		/// ����쳣��Ϣ
		/// </summary>
		/// <param name="text"></param>
		/// <param name="ex"></param>
		void BaseLogOutputError(string text, Exception ex)
		{
			this.isBaseLogExeFinish = true;
			this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
		}

		/// <summary>
		/// ����쳣��Ϣ
		/// </summary>
		/// <param name="text"></param>
		/// <param name="ex"></param>
		void AssayOutputError(string text, Exception ex)
		{
			this.isAssayExeFinish = true;
			this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
		}

		/// <summary>
		/// ����رպ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmUploadData_FormClosed(object sender, FormClosedEventArgs e)
		{
			// ע�⣺����ȡ������
			this.taskSimpleScheduler.Cancal();
		}
	}
}
