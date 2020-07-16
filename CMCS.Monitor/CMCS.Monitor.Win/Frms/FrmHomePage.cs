using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Enums;
using CMCS.Monitor.Win.CefGlue;
using CMCS.Monitor.Win.Core;
using CMCS.Monitor.Win.Html;
using CMCS.Monitor.Win.UserControls;
using CMCS.Monitor.Win.Utilities;
using DevComponents.DotNetBar;
using Xilium.CefGlue;
using Xilium.CefGlue.WindowsForms;

namespace CMCS.Monitor.Win.Frms
{
    public partial class FrmHomePage : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmHomePage";

        CommonDAO commonDAO = CommonDAO.GetInstance();
        MonitorCommon monitorCommon = MonitorCommon.GetInstance();

        CefWebBrowserEx cefWebBrowser = new CefWebBrowserEx();

        public FrmHomePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void FormInit()
        {
#if DEBUG
            gboxTest.Visible = true;
#else
            gboxTest.Visible = false;
#endif
            cefWebBrowser.StartUrl = SelfVars.Url_HomePage;
            cefWebBrowser.Dock = DockStyle.Fill;
            cefWebBrowser.WebClient = new HomePageCefWebClient(cefWebBrowser);
            cefWebBrowser.LoadEnd += new EventHandler<LoadEndEventArgs>(cefWebBrowser_LoadEnd);
            panWebBrower.Controls.Add(cefWebBrowser);
        }

        void cefWebBrowser_LoadEnd(object sender, LoadEndEventArgs e)
        {
            timer1.Enabled = true;

            RequestData();
        }

        private void FrmHomePage_Load(object sender, EventArgs e)
        {
            monitorCommon.SetFromSize(this);

            FormInit();
        }

        /// <summary>
        /// 测试 - 刷新页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cefWebBrowser.Browser.Reload();
        }

        /// <summary>
        /// 测试 - 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRequestData_Click(object sender, EventArgs e)
        {
            RequestData();
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        void RequestData()
        {
            string value = string.Empty, machineCode = string.Empty;
            List<HtmlDataItem> datas = new List<HtmlDataItem>();

            datas.Clear();

            datas.Add(new HtmlDataItem("矿发_车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "矿发_车数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("矿发_煤量", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "矿发_煤量"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("矿发_矿数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "矿发_矿数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("在途_车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "在途_车数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("在途_异常车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "在途_异常车数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("进厂_车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "进厂_车数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("进厂_矿数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "进厂_矿数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("进厂_其他物资车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "进厂_其他物资车数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("采样_车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "采样_车数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("采样_样重", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "采样_样重"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("采样_批数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "采样_批数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("称重_车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "称重_车数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("称重_毛重", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "称重_毛重"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("称重_皮重", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "称重_皮重"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("卸煤_车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "卸煤_车数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("卸煤_煤量", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "卸煤_煤量"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("卸煤_扣吨", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "卸煤_扣吨"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("出厂_车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "出厂_车数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("出厂_验收", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "出厂_验收"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("排队区_车数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "排队区_车数"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("制样室_制样数", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "制样室_制样数"), eHtmlDataItemType.svg_text));

            #region 汽车采样机

            machineCode = GlobalVars.MachineCode_QC_JxSampler_1;
            datas.Add(new HtmlDataItem("1号采样机_系统", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_1, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("1号采样机_车号", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸1升杆.ToString(), "1号采样机_右道闸");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸2升杆.ToString(), "1号采样机_左道闸");

            machineCode = GlobalVars.MachineCode_QC_JxSampler_2;
            datas.Add(new HtmlDataItem("2号采样机_系统", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_2, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("2号采样机_车号", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸1升杆.ToString(), "2号采样机_右道闸");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸2升杆.ToString(), "2号采样机_左道闸");

            machineCode = GlobalVars.MachineCode_QC_JxSampler_3;
            datas.Add(new HtmlDataItem("3号采样机_系统", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_3, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("3号采样机_车号", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸1升杆.ToString(), "3号采样机_右道闸");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸2升杆.ToString(), "3号采样机_左道闸");

            #endregion

            #region 汽车衡

            machineCode = GlobalVars.MachineCode_QC_Weighter_1;
            datas.Add(new HtmlDataItem("1号衡_系统", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("1号衡_车号", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸1升杆.ToString(), "1号衡_右道闸");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸2升杆.ToString(), "1号衡_左道闸");

            machineCode = GlobalVars.MachineCode_QC_Weighter_2;
            datas.Add(new HtmlDataItem("2号衡_系统", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("2号衡_车号", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸1升杆.ToString(), "2号衡_右道闸");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸2升杆.ToString(), "2号衡_左道闸");

            machineCode = GlobalVars.MachineCode_QC_Weighter_3;
            datas.Add(new HtmlDataItem("3号衡_系统", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("3号衡_车号", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸1升杆.ToString(), "3号衡_右道闸");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸2升杆.ToString(), "3号衡_左道闸");

            machineCode = GlobalVars.MachineCode_QC_Weighter_4;
            datas.Add(new HtmlDataItem("4号衡_系统", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.系统.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("4号衡_车号", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.当前车号.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸1升杆.ToString(), "4号衡_右道闸");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.道闸2升杆.ToString(), "4号衡_左道闸");

            #endregion

            // 添加更多...

            // 发送到页面
            cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 界面不可见时，停止发送数据
            if (!this.Visible) return;

            RequestData();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("test1();", "", 0);
        }

    }

    public class HomePageCefWebClient : CefWebClient
    {
        CefWebBrowser cefWebBrowser;

        public HomePageCefWebClient(CefWebBrowser cefWebBrowser)
            : base(cefWebBrowser)
        {
            this.cefWebBrowser = cefWebBrowser;
        }

        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            if (message.Name == "OpenTruckWeighter")
                SelfVars.MainFrameForm.OpenTruckWeighter();
            else if (message.Name == "TruckWeighterChangeSelected")
                SelfVars.TruckWeighterForm.CurrentMachineCode = MonitorCommon.GetInstance().GetTruckWeighterMachineCodeBySelected(message.Arguments.GetString(0));

            return true;
        }

        protected override CefContextMenuHandler GetContextMenuHandler()
        {
            return new CefMenuHandler();
        }
    }
}