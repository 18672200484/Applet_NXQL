using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.Common;
using CMCS.Common.Utilities;
using CMCS.DataTester.Core;
using CMCS.DumblyConcealer.Enums;
using HikVisionSDK.Core;

namespace CMCS.DataTester.Frms
{
    public partial class FrmCamera : Form
    {
        public FrmCamera()
        {
            InitializeComponent();
        }

        Boolean isInsertData = false;
        RTxtOutputer rTxtOutputer;
        TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();
        IPCer iPCer_Identify1 = new IPCer();

        /// <summary>
        /// 窗体加载的时候获取所有状态为在途的车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCarLocation_Load(object sender, EventArgs e)
        {
            IPCer.InitSDK();

        }

        void ReceiveData1(string number)
        {
            PrintError(number);
        }

        /// <summary>
        /// 开始模拟数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            iPCer_Identify1.Login("192.168.1.50", 80, "admin", "admin123");
            uint ss = IPCer.GetLastErrorCode();
            iPCer_Identify1.StartPreview(panVideo1.Handle, 1);
            //iPCer_Identify1.OnReceived = ReceiveData1;
            iPCer_Identify1.SetDVRCallBack();
            iPCer_Identify1.SetupAlarm();
        }

        private void PrintError(String error)
        {
            if (this.errorMsg.TextLength > 100000)
                this.errorMsg.Text = error + "\n";
            else
                this.errorMsg.Text += error + "\n";
        }

        /// <summary>
        /// 输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void OutputError(string text, Exception ex)
        {
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.errorMsg.Text = "";
        }

        private void FrmCamera_FormClosing(object sender, FormClosingEventArgs e)
        {
            iPCer_Identify1.CloseAlarm();
            iPCer_Identify1.LoginOut();
            IPCer.CleanupSDK();
        }
    }

}
