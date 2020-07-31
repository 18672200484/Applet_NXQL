using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.DumblyConcealer.Win.Core;
using CMCS.Common.Utilities;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.Intelogistics;

namespace CMCS.DumblyConcealer.Win.DumblyTasks
{
    public partial class FrmIntelogistics : TaskForm
    {
        RTxtOutputer rTxtOutputer;
        TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();
        Boolean isExeFinish = true;
        Boolean isTransExeFinish = true;
        Boolean isAssayExeFinish = true;

        public FrmIntelogistics()
        {
            InitializeComponent();
        }

        private void FrmIntelogistics_Load(object sender, EventArgs e)
        {
            this.rTxtOutputer = new RTxtOutputer(rtxtOutput);
            ExecuteAllTask();
        }

        /// <summary>
        /// 执行所有任务
        /// </summary>
        void ExecuteAllTask()
        {
            IntelogisticsDAO dao = IntelogisticsDAO.GetInstance();
            taskSimpleScheduler.StartNewTask("获取智能物流始发表数据", () =>
            {
                if (isExeFinish)
                {
                    isExeFinish = false;
                    //执行任务
                    dao.GetData(this.rTxtOutputer.Output);
                    isExeFinish = true;
                }
            }, 30 * 1000, OutputError);


            taskSimpleScheduler.StartNewTask("推送运输记录至智能物流", () =>
            {
                if (isTransExeFinish)
                {
                    isTransExeFinish = false;
                    //执行任务
                    dao.SendTransData(this.rTxtOutputer.Output);
                    isTransExeFinish = true;
                }
            }, 60 * 1000, BaseLogOutputError);

            taskSimpleScheduler.StartNewTask("推送化验信息至智能物流", () =>
            {
                if (isAssayExeFinish)
                {
                    isAssayExeFinish = false;
                    //执行任务
                    dao.SendAssayData(this.rTxtOutputer.Output);
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
            this.isTransExeFinish = true;
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
        private void FrmIntelogistics_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 注意：必须取消任务
            this.taskSimpleScheduler.Cancal();
        }
    }
}