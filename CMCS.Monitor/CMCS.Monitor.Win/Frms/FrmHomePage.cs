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
        /// ����Ψһ��ʶ��
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
        /// �����ʼ��
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
        /// ���� - ˢ��ҳ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cefWebBrowser.Browser.Reload();
        }

        /// <summary>
        /// ���� - ����ˢ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRequestData_Click(object sender, EventArgs e)
        {
            RequestData();
        }

        /// <summary>
        /// ��������
        /// </summary>
        void RequestData()
        {
            string value = string.Empty, machineCode = string.Empty;
            List<HtmlDataItem> datas = new List<HtmlDataItem>();

            datas.Clear();

            datas.Add(new HtmlDataItem("��_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "��_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("��_ú��", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "��_ú��"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("��_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "��_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("��;_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "��;_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("��;_�쳣����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "��;_�쳣����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_�������ʳ���", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_�������ʳ���"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_ë��", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_ë��"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_Ƥ��", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_Ƥ��"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("жú_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "жú_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("жú_ú��", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "жú_ú��"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("жú_�۶�", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "жú_�۶�"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("����_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "����_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("�Ŷ���_����", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "�Ŷ���_����"), eHtmlDataItemType.svg_text));
            datas.Add(new HtmlDataItem("������_������", commonDAO.GetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "������_������"), eHtmlDataItemType.svg_text));

            #region ����������

            machineCode = GlobalVars.MachineCode_QC_JxSampler_1;
            datas.Add(new HtmlDataItem("1�Ų�����_ϵͳ", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_1, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("1�Ų�����_����", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ1����.ToString(), "1�Ų�����_�ҵ�բ");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ2����.ToString(), "1�Ų�����_���բ");

            machineCode = GlobalVars.MachineCode_QC_JxSampler_2;
            datas.Add(new HtmlDataItem("2�Ų�����_ϵͳ", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_2, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("2�Ų�����_����", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ1����.ToString(), "2�Ų�����_�ҵ�բ");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ2����.ToString(), "2�Ų�����_���բ");

            machineCode = GlobalVars.MachineCode_QC_JxSampler_3;
            datas.Add(new HtmlDataItem("3�Ų�����_ϵͳ", monitorCommon.ConvertMachineStatusToColor(commonDAO.GetSignalDataValue(GlobalVars.MachineCode_QCJXCYJ_3, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("3�Ų�����_����", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ1����.ToString(), "3�Ų�����_�ҵ�բ");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ2����.ToString(), "3�Ų�����_���բ");

            #endregion

            #region ������

            machineCode = GlobalVars.MachineCode_QC_Weighter_1;
            datas.Add(new HtmlDataItem("1�ź�_ϵͳ", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("1�ź�_����", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ1����.ToString(), "1�ź�_�ҵ�բ");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ2����.ToString(), "1�ź�_���բ");

            machineCode = GlobalVars.MachineCode_QC_Weighter_2;
            datas.Add(new HtmlDataItem("2�ź�_ϵͳ", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("2�ź�_����", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ1����.ToString(), "2�ź�_�ҵ�բ");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ2����.ToString(), "2�ź�_���բ");

            machineCode = GlobalVars.MachineCode_QC_Weighter_3;
            datas.Add(new HtmlDataItem("3�ź�_ϵͳ", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("3�ź�_����", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ1����.ToString(), "3�ź�_�ҵ�բ");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ2����.ToString(), "3�ź�_���բ");

            machineCode = GlobalVars.MachineCode_QC_Weighter_4;
            datas.Add(new HtmlDataItem("4�ź�_ϵͳ", monitorCommon.ConvertBooleanToColor(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.ϵͳ.ToString())), eHtmlDataItemType.svg_color));
            datas.Add(new HtmlDataItem("4�ź�_����", monitorCommon.CarTitleText(commonDAO.GetSignalDataValue(machineCode, eSignalDataName.��ǰ����.ToString())), eHtmlDataItemType.svg_text));
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ1����.ToString(), "4�ź�_�ҵ�բ");
            monitorCommon.AddDataItemByGateUp(datas, machineCode, eSignalDataName.��բ2����.ToString(), "4�ź�_���բ");

            #endregion

            // ��Ӹ���...

            // ���͵�ҳ��
            cefWebBrowser.Browser.GetMainFrame().ExecuteJavaScript("requestData(" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + ");", "", 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // ���治�ɼ�ʱ��ֹͣ��������
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