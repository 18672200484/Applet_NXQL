using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.Queue.Enums;

namespace CMCS.CarTransport.Queue.Frms.Sys
{
    /// <summary>
    /// 调试输出控制台
    /// </summary>
    public partial class FrmDebugConsole : DevComponents.DotNetBar.Metro.MetroForm
    {
        private static FrmDebugConsole instance;

        public static FrmDebugConsole GetInstance()
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new FrmDebugConsole();
            }

            return instance;
        }

        private FrmDebugConsole()
        {
            InitializeComponent();
        }

        private void FrmDebugConsole_Load(object sender, EventArgs e)
        {
            cmbPassWay.Items.Add(new DataItem("道路一", "道路一", ePassWay.Way1));
            cmbPassWay.Items.Add(new DataItem("道路二", "道路二", ePassWay.Way2));
            cmbPassWay.SelectedIndex = 0;
        }

        public void Output(string message)
        {
            try
            {
                rtxtOutput.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message + Environment.NewLine);
                rtxtOutput.ScrollToCaret();
            }
            catch { }
        }

        /// <summary>
        /// 模拟刷卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVoucher.Text.Trim()))
            {
                MessageBoxEx.Show("请输入车牌号\\标签号！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FrmQueuer.passCarQueuer.Enqueue((ePassWay)(cmbPassWay.SelectedItem as DataItem).Data, txtVoucher.Text.Trim(), true);

            Output("模拟来车：" + txtVoucher.Text.Trim() + "  " + cmbPassWay.Text);
        }
    }
}