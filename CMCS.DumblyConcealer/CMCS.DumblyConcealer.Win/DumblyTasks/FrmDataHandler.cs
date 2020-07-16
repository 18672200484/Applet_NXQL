using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Utilities;
using CMCS.DapperDber.Dbs.AccessDb;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.DataHandler;
using CMCS.DumblyConcealer.Win.Core;

namespace CMCS.DumblyConcealer.Win.DumblyTasks
{
    public partial class FrmDataHandler : TaskForm
    {
        RTxtOutputer rTxtOutputer;
        RTxtOutputer rTxtOutResultputer;
        TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();
        CommonDAO commonDAO = CommonDAO.GetInstance();

        public FrmDataHandler()
        {
            InitializeComponent();
        }

        private void FrmCarSynchronous_Load(object sender, EventArgs e)
        {
            this.Text = "综合事件处理";

            this.rTxtOutputer = new RTxtOutputer(rtxtOutput);

            ExecuteAllTask();

        }

        /// <summary>
        /// 执行所有任务
        /// </summary>
        void ExecuteAllTask()
        {
            DataHandlerDAO dataHandlerDAO = DataHandlerDAO.GetInstance();
            string outNetWebApi = commonDAO.GetAppletConfigString(CommonAppConfig.GetInstance().AppIdentifier, "外网Api请求地址");

            taskSimpleScheduler.StartNewTask("同步外网矿发运输记录信息", () =>
            {
                dataHandlerDAO.SyncOutNetTransport(this.rTxtOutputer.Output, outNetWebApi);

            }, 10000, OutputError);

            taskSimpleScheduler.StartNewTask("同步更新外网矿发运输记录节点状态", () =>
            {
                dataHandlerDAO.SyncUpdateTransportStepName(this.rTxtOutputer.Output, outNetWebApi);

            }, 2000, OutputError);
        }

        /// <summary>
        /// 输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void OutputError(string text, Exception ex)
        {
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);

            Log4Neter.Error(text, ex);
        }

        /// <summary>
        /// 输出异常信息（结果）
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void OutputResultError(string text, Exception ex)
        {
            this.rTxtOutResultputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);

            Log4Neter.Error(text, ex);
        }

        /// <summary>
        /// 窗体关闭后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCarSynchronous_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 注意：必须取消任务
            this.taskSimpleScheduler.Cancal();
        }
    }
}
