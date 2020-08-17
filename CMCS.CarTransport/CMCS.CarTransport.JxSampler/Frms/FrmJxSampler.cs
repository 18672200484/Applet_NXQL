using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
//
using CMCS.CarTransport.DAO;
using CMCS.CarTransport.JxSampler.Core;
using CMCS.CarTransport.JxSampler.Enums;
using CMCS.CarTransport.JxSampler.Frms.Sys;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Entities.Inf;
using CMCS.Common.Entities.Sys;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using DevComponents.DotNetBar;
using HikVisionSDK.Core;
using LED.YB_Bx5K1;

namespace CMCS.CarTransport.JxSampler.Frms
{
	public partial class FrmJxSampler : DevComponents.DotNetBar.Metro.MetroForm
	{
		/// <summary>
		/// ����Ψһ��ʶ��
		/// </summary>
		public static string UniqueKey = "FrmCarSampler";

		public FrmJxSampler()
		{
			InitializeComponent();
		}

		#region Vars

		CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();
		JxSamplerDAO jxSamplerDAO = JxSamplerDAO.GetInstance();
		CommonDAO commonDAO = CommonDAO.GetInstance();

		IocControler iocControler;
		/// <summary>
		/// ��������
		/// </summary>
		VoiceSpeaker voiceSpeaker = new VoiceSpeaker();

		bool autoHandMode = true;
		/// <summary>
		/// �ֶ�ģʽ=true  �ֶ�ģʽ=false
		/// </summary>
		public bool AutoHandMode
		{
			get { return autoHandMode; }
			set
			{
				autoHandMode = value;

				btnSendSamplingPlan.Visible = !value;
				btnSelectAutotruck.Visible = !value;
				btnReset.Visible = !value;
			}
		}

		bool inductorCoil1 = false;
		/// <summary>
		/// �ظ�1״̬ true=���ź�  false=���ź�
		/// </summary>
		public bool InductorCoil1
		{
			get
			{
				return inductorCoil1;
			}
			set
			{
				inductorCoil1 = value;

				panCurrentCarNumber.Refresh();

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.�ظ�1�ź�.ToString(), value ? "1" : "0");
			}
		}

		int inductorCoil1Port;
		/// <summary>
		/// �ظ�1�˿�
		/// </summary>
		public int InductorCoil1Port
		{
			get { return inductorCoil1Port; }
			set { inductorCoil1Port = value; }
		}

		bool inductorCoil2 = false;
		/// <summary>
		/// �ظ�2״̬ true=���ź�  false=���ź�
		/// </summary>
		public bool InductorCoil2
		{
			get
			{
				return inductorCoil2;
			}
			set
			{
				inductorCoil2 = value;

				panCurrentCarNumber.Refresh();

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.�ظ�2�ź�.ToString(), value ? "1" : "0");
			}
		}

		int inductorCoil2Port;
		/// <summary>
		/// �ظ�2�˿�
		/// </summary>
		public int InductorCoil2Port
		{
			get { return inductorCoil2Port; }
			set { inductorCoil2Port = value; }
		}

		bool infraredSensor1 = false;
		/// <summary>
		/// ����1״̬ true=�ڵ�  false=��ͨ
		/// </summary>
		public bool InfraredSensor1
		{
			get
			{
				return infraredSensor1;
			}
			set
			{
				infraredSensor1 = value;

				panCurrentCarNumber.Refresh();

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.����1�ź�.ToString(), value ? "1" : "0");
			}
		}

		int infraredSensor1Port;
		/// <summary>
		/// ����1�˿�
		/// </summary>
		public int InfraredSensor1Port
		{
			get { return infraredSensor1Port; }
			set { infraredSensor1Port = value; }
		}

		bool infraredSensor2 = false;
		/// <summary>
		/// ����2״̬ true=�ڵ�  false=��ͨ
		/// </summary>
		public bool InfraredSensor2
		{
			get
			{
				return infraredSensor2;
			}
			set
			{
				infraredSensor2 = value;
				panCurrentCarNumber.Refresh();

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.����2�ź�.ToString(), value ? "1" : "0");
			}
		}

		int infraredSensor2Port;
		/// <summary>
		/// ����2�˿�
		/// </summary>
		public int InfraredSensor2Port
		{
			get { return infraredSensor2Port; }
			set { infraredSensor2Port = value; }
		}

		/// <summary>
		/// �ظ�1����բ�ظУ�״̬
		/// </summary>
		public bool InductorCoil1State = false;

		/// <summary>
		/// �ظ�2����բ�ظУ�״̬
		/// </summary>
		public bool InductorCoil2State = false;

		public static PassCarQueuer passCarQueuer = new PassCarQueuer();

		ImperfectCar currentImperfectCar;
		/// <summary>
		/// ʶ���ѡ��ĳ���ƾ֤
		/// </summary>
		public ImperfectCar CurrentImperfectCar
		{
			get { return currentImperfectCar; }
			set
			{
				currentImperfectCar = value;

				if (value != null)
					panCurrentCarNumber.Text = value.Voucher;
				else
					panCurrentCarNumber.Text = "�ȴ�����";
			}
		}

		eFlowFlag currentFlowFlag = eFlowFlag.�ȴ�����;
		/// <summary>
		/// ��ǰҵ�����̱�ʶ
		/// </summary>
		public eFlowFlag CurrentFlowFlag
		{
			get { return currentFlowFlag; }
			set
			{
				currentFlowFlag = value;

				lblFlowFlag.Text = value.ToString();
			}
		}

		private CmcsBuyFuelTransport currentBuyFuelTransport;
		/// <summary>
		/// ��ǰ�����¼
		/// </summary>
		public CmcsBuyFuelTransport CurrentBuyFuelTransport
		{
			get { return currentBuyFuelTransport; }
			set
			{
				currentBuyFuelTransport = value;

				if (value != null)
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ�����¼Id.ToString(), value.Id);

					txtSupplierName.Text = value.SupplierName;
					txtMineName.Text = value.MineName;
					txtTicketWeight.Text = value.TicketWeight.ToString();
					txtTransportCompanyName.Text = value.TransportCompanyName;
					txtFuelKindName.Text = value.FuelKindName;
				}
				else
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ�����¼Id.ToString(), string.Empty);

					txtSupplierName.ResetText();
					txtMineName.ResetText();
					txtTransportCompanyName.ResetText();
					txtFuelKindName.ResetText();
					txtTicketWeight.ResetText();
				}
			}
		}

		CmcsAutotruck currentAutotruck;
		/// <summary>
		/// ��ǰ��
		/// </summary>
		public CmcsAutotruck CurrentAutotruck
		{
			get { return currentAutotruck; }
			set
			{
				currentAutotruck = value;

				if (value != null)
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ��Id.ToString(), value.Id);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ����.ToString(), value.CarNumber);

					CmcsEPCCard ePCCard = Dbers.GetInstance().SelfDber.Get<CmcsEPCCard>(value.EPCCardId);
					if (ePCCard != null) txtTagId.Text = ePCCard.TagId;

					txtCarNumber.Text = value.CarNumber;
					panCurrentCarNumber.Text = value.CarNumber;

					dbi_CarriageLength.Value = value.CarriageLength;
					dbi_CarriageWidth.Value = value.CarriageWidth;
					dbi_CarriageBottomToFloor.Value = value.CarriageBottomToFloor;
					dbi_LeftObstacle1.Value = value.LeftObstacle1;
					dbi_LeftObstacle2.Value = value.LeftObstacle2;
					dbi_LeftObstacle3.Value = value.LeftObstacle3;
					dbi_LeftObstacle4.Value = value.LeftObstacle4;
					dbi_LeftObstacle5.Value = value.LeftObstacle5;
					dbi_LeftObstacle6.Value = value.LeftObstacle6;
				}
				else
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ��Id.ToString(), string.Empty);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ����.ToString(), string.Empty);

					dbi_CarriageLength.Value = 0;
					dbi_CarriageWidth.Value = 0;
					dbi_CarriageBottomToFloor.Value = 0;
					dbi_LeftObstacle1.Value = 0;
					dbi_LeftObstacle2.Value = 0;
					dbi_LeftObstacle3.Value = 0;
					dbi_LeftObstacle4.Value = 0;
					dbi_LeftObstacle5.Value = 0;
					dbi_LeftObstacle6.Value = 0;

					txtTagId.ResetText();
					txtCarNumber.ResetText();
					panCurrentCarNumber.ResetText();
				}
			}
		}

		private InfQCJXCYSampleCMD currentSampleCMD;
		/// <summary>
		/// ��ǰ��������
		/// </summary>
		public InfQCJXCYSampleCMD CurrentSampleCMD
		{
			get { return currentSampleCMD; }
			set { currentSampleCMD = value; }
		}

		private eEquInfSamplerSystemStatus samplerSystemStatus;
		/// <summary>
		/// ������ϵͳ״̬
		/// </summary>
		public eEquInfSamplerSystemStatus SamplerSystemStatus
		{
			get { return samplerSystemStatus; }
			set
			{
				samplerSystemStatus = value;

				if (value == eEquInfSamplerSystemStatus.��������)
					slightSamplerStatus.LightColor = EquipmentStatusColors.BeReady;
				else if (value == eEquInfSamplerSystemStatus.��������)
					slightSamplerStatus.LightColor = EquipmentStatusColors.Working;
				else if (value == eEquInfSamplerSystemStatus.��������)
					slightSamplerStatus.LightColor = EquipmentStatusColors.Breakdown;
				else if (value == eEquInfSamplerSystemStatus.ϵͳֹͣ)
					slightSamplerStatus.LightColor = EquipmentStatusColors.Forbidden;
			}
		}

		/// <summary>
		/// �������豸����
		/// </summary>
		public string SamplerMachineCode;
		/// <summary>
		/// �������豸����
		/// </summary>
		public string SamplerMachineName;

		/// <summary>
		/// ���ö�����
		/// </summary>
		public bool UseRwer = true;

		/// <summary>
		/// ����ʶ�����
		/// </summary>
		public bool UseCamera = true;
		#endregion

		/// <summary>
		/// �����ʼ��
		/// </summary>
		private void InitForm()
		{
			FrmDebugConsole.GetInstance();

			// �������豸����
			this.SamplerMachineCode = commonDAO.GetAppletConfigString("�������豸����");
			this.SamplerMachineName = commonDAO.GetMachineNameByCode(this.SamplerMachineCode);
			this.UseRwer = commonDAO.GetAppletConfigInt32("���ö�����") == 1;
			this.UseCamera = commonDAO.GetAppletConfigInt32("����ʶ�����") == 1;
			// Ĭ���Զ�
			sbtnChangeAutoHandMode.Value = true;

			// ���ó���Զ�̿�������
			commonDAO.ResetAppRemoteControlCmd(CommonAppConfig.GetInstance().AppIdentifier);

			btnRefresh_Click(null, null);
		}

		private void FrmCarSampler_Load(object sender, EventArgs e)
		{

		}

		private void FrmCarSampler_Shown(object sender, EventArgs e)
		{
			InitHardware();

			InitForm();
		}

		private void FrmCarSampler_FormClosing(object sender, FormClosingEventArgs e)
		{
			// ж���豸
			UnloadHardware();
		}

		#region �豸���

		#region IO������

		void Iocer_StatusChange(bool status)
		{
			// ����IO������״̬ 
			InvokeEx(() =>
			{
				slightIOC.LightColor = (status ? Color.Green : Color.Red);

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.IO������_����״̬.ToString(), status ? "1" : "0");
			});
		}

		/// <summary>
		/// IO��������������ʱ����
		/// </summary>
		/// <param name="receiveValue"></param>
		void Iocer_Received(int[] receiveValue)
		{
			// ���յظ�״̬  
			InvokeEx(() =>
			  {
				  this.InductorCoil1 = (receiveValue[this.InductorCoil1Port - 1] == 1);
				  this.InductorCoil2 = (receiveValue[this.InductorCoil2Port - 1] == 1);
				  this.InfraredSensor1 = (receiveValue[this.InfraredSensor1Port - 1] == 1);
				  this.InfraredSensor2 = (receiveValue[this.InfraredSensor2Port - 1] == 1);
			  });
		}

		/// <summary>
		/// ǰ������
		/// </summary>
		void FrontGateUp()
		{
			this.iocControler.Gate2Up();
			this.iocControler.GreenLight1();
		}

		/// <summary>
		/// ǰ������
		/// </summary>
		void FrontGateDown()
		{
			if (!this.InductorCoil2)
				this.iocControler.Gate2Down();
			this.iocControler.RedLight1();
		}

		/// <summary>
		/// ������
		/// </summary>
		void BackGateUp()
		{
			this.iocControler.Gate1Up();
			this.iocControler.GreenLight1();
		}

		/// <summary>
		/// �󷽽���
		/// </summary>
		void BackGateDown()
		{
			if (!this.InductorCoil1)
				this.iocControler.Gate1Down();
			this.iocControler.RedLight1();
		}

		/// <summary>
		/// �г�������ڵ�·��
		/// </summary>
		/// <returns></returns>
		bool HasCarOnCurrentInWay()
		{
			if (this.CurrentImperfectCar == null) return false;
			return this.InductorCoil1;
		}

		/// <summary>
		/// �г����ڳ��ڵ�·��
		/// </summary>
		/// <returns></returns>
		bool HasCarOnCurrentOutWay()
		{
			if (this.CurrentImperfectCar == null) return false;
			return this.InductorCoil2;
		}

		#endregion

		#region ������

		void Rwer1_OnScanError(Exception ex)
		{
			Log4Neter.Error("������1", ex);
		}

		void Rwer1_OnStatusChange(bool status)
		{
			// ���ն�����1״̬ 
			InvokeEx(() =>
			 {
				 slightRwer1.LightColor = (status ? Color.Green : Color.Red);

				 commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.������1_����״̬.ToString(), status ? "1" : "0");
			 });
		}

		#endregion

		#region LED���ƿ�
		YB_Bx5K1 LED1 = new YB_Bx5K1();
		/// <summary>
		/// LED1���±�ʶ
		/// </summary>
		bool LED1m_bSendBusy = false;

		private bool _LED1ConnectStatus;
		/// <summary>
		/// LED1����״̬
		/// </summary>
		public bool LED1ConnectStatus
		{
			get
			{
				return _LED1ConnectStatus;
			}

			set
			{
				_LED1ConnectStatus = value;

				slightLED1.LightColor = (value ? Color.Green : Color.Red);

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.LED��1_����״̬.ToString(), value ? "1" : "0");
			}
		}

		/// <summary>
		/// LED1��һ����ʾ����
		/// </summary>
		string LED1PrevLedFileContent = string.Empty;

		/// <summary>
		/// ����LED1��̬����
		/// </summary>
		/// <param name="value1">��һ������</param>
		/// <param name="value2">�ڶ�������</param>
		private void UpdateLedShow(string value1 = "", string value2 = "")
		{
			if (this.LED1PrevLedFileContent == value1 + value2) return;
			FrmDebugConsole.GetInstance().Output("����LED1:|" + value1 + "|" + value2 + "|");
			if (!this.LED1ConnectStatus) return;

			if (LED1.UpdateArea(value1, value2))
			{
				LED1m_bSendBusy = true;
			}
			else
				LED1m_bSendBusy = false;

			this.LED1PrevLedFileContent = value1 + value2;
		}

		#endregion

		#region ��������ץ�����

		/// <summary>
		/// �������������
		/// </summary>
		IPCer iPCer_Identify1 = new IPCer();

		void ReceiveData1(string carNumber, string IP)
		{
			InvokeEx(() =>
			  {
				  this.UpdateLedShow("ʶ�𵽳���:" + carNumber);
				  if (this.UseCamera && this.CurrentFlowFlag == eFlowFlag.�ȴ�����)
				  {
					  passCarQueuer.Enqueue(carNumber);
					  this.CurrentFlowFlag = eFlowFlag.��֤����;
					  timer1_Tick(null, null);
				  }
			  });
		}

		#endregion
		#endregion

		#region �豸��ʼ����ж��

		/// <summary>
		/// ��ʼ������豸
		/// </summary>
		private void InitHardware()
		{
			try
			{
				bool success = false;

				this.InductorCoil1Port = commonDAO.GetAppletConfigInt32("IO������_�ظ�1�˿�");
				this.InductorCoil2Port = commonDAO.GetAppletConfigInt32("IO������_�ظ�2�˿�");
				this.InfraredSensor1Port = commonDAO.GetAppletConfigInt32("IO������_����1�˿�");
				this.InfraredSensor2Port = commonDAO.GetAppletConfigInt32("IO������_����2�˿�");

				// IO������
				Hardwarer.Iocer.OnReceived += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.ReceivedEventHandler(Iocer_Received);
				Hardwarer.Iocer.OnStatusChange += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.StatusChangeHandler(Iocer_StatusChange);
				success = Hardwarer.Iocer.OpenCom(commonDAO.GetAppletConfigInt32("IO������_����"), commonDAO.GetAppletConfigInt32("IO������_������"), commonDAO.GetAppletConfigInt32("IO������_����λ"), (StopBits)commonDAO.GetAppletConfigInt32("IO������_ֹͣλ"), (Parity)commonDAO.GetAppletConfigInt32("IO������_У��λ"));
				if (!success) MessageBoxEx.Show("IO����������ʧ�ܣ�", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				this.iocControler = new IocControler(Hardwarer.Iocer);

				// ������1
				Hardwarer.Rwer1.StartWith = commonDAO.GetAppletConfigString("������_��ǩ����");
				Hardwarer.Rwer1.OnStatusChange += new RW.LZR12.Net.Lzr12Rwer.StatusChangeHandler(Rwer1_OnStatusChange);
				Hardwarer.Rwer1.OnScanError += new RW.LZR12.Net.Lzr12Rwer.ScanErrorEventHandler(Rwer1_OnScanError);
				success = CommonUtil.PingReplyTest(commonDAO.GetAppletConfigString("������1_IP��ַ")) && Hardwarer.Rwer1.OpenCom(commonDAO.GetAppletConfigString("������1_IP��ַ"), 500, Convert.ToByte(commonDAO.GetAppletConfigInt32("������1_����")));
				if (!success) MessageBoxEx.Show("������1����ʧ�ܣ�", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				#region LED���ƿ�1

				string led1SocketIP = commonDAO.GetAppletConfigString("LED��ʾ��1_IP��ַ");
				if (!string.IsNullOrEmpty(led1SocketIP))
				{
					if (CommonUtil.PingReplyTest(led1SocketIP))
					{
						if (LED1.CreateListent(led1SocketIP))
						{
							// ��ʼ���ɹ�
							this.LED1ConnectStatus = true;
							UpdateLedShow("  �ȴ�����");
						}
						else
						{
							this.LED1ConnectStatus = false;
							Log4Neter.Error("LED1���ƿ�����ʧ��", new Exception("����ʧ��"));
							MessageBoxEx.Show("LED1���ƿ�����ʧ��", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					}
					else
					{
						this.LED1ConnectStatus = false;
						Log4Neter.Error("��ʼ��LED1���ƿ�����������ʧ��", new Exception("�����쳣"));
						MessageBoxEx.Show("LED1���ƿ���������ʧ�ܣ�", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}

				#endregion

				IPCer.InitSDK();
				//ʶ�������
				CmcsCamare video_Identify1 = commonDAO.SelfDber.Entity<CmcsCamare>("where Name=:Name", new { Name = CommonAppConfig.GetInstance().AppIdentifier + "����ʶ��1" });
				if (video_Identify1 != null)
				{
					if (CommonUtil.PingReplyTest(video_Identify1.Ip))
					{
						if (iPCer_Identify1.Login(video_Identify1.Ip, video_Identify1.Port, video_Identify1.UserName, video_Identify1.Password))
						{
							iPCer_Identify1.StartPreview(panVideo1.Handle, video_Identify1.Channel);
							iPCer_Identify1.OnReceived += new IPCer.ReceivedEventHandler(ReceiveData1);
							iPCer_Identify1.SetDVRCallBack();
							iPCer_Identify1.SetupAlarm();
						}
					}
				}

				//��������
				voiceSpeaker.SetVoice(commonDAO.GetAppletConfigInt32("����"), commonDAO.GetAppletConfigInt32("����"), commonDAO.GetAppletConfigString("������"));

				timer1.Enabled = true;
			}
			catch (Exception ex)
			{
				Log4Neter.Error("�豸��ʼ��", ex);
			}
		}

		/// <summary>
		/// ж���豸
		/// </summary>
		private void UnloadHardware()
		{
			// ע��˶δ���
			Application.DoEvents();

			try
			{
				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ��Id.ToString(), string.Empty);
				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.��ǰ����.ToString(), string.Empty);
			}
			catch { }
			try
			{
				Hardwarer.Iocer.OnReceived -= new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.ReceivedEventHandler(Iocer_Received);
				Hardwarer.Iocer.OnStatusChange -= new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.StatusChangeHandler(Iocer_StatusChange);

				Hardwarer.Iocer.CloseCom();
			}
			catch { }
			try
			{
				Hardwarer.Rwer1.CloseCom();
			}
			catch { }
			try
			{
				if (this.LED1ConnectStatus)
				{
					LED1.CloseListent();
				}
			}
			catch { }
			try
			{
				//iPCer_Identify1.OnReceived = null;
				iPCer_Identify1.CloseAlarm();
				iPCer_Identify1.LoginOut();
				IPCer.CleanupSDK();
			}
			catch { }
		}

		#endregion

		#region ��բ���ư�ť

		/// <summary>
		/// ��բ1����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate1Up_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate1Up();
		}

		/// <summary>
		/// ��բ1����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate1Down_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate1Down();
		}

		/// <summary>
		/// ��բ2����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate2Up_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate2Up();
		}

		/// <summary>
		/// ��բ2����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate2Down_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate2Down();
		}

		#endregion

		#region ����ҵ��

		/// <summary>
		/// ����������ʶ������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Stop();

			try
			{
				// ִ��Զ������
				ExecAppRemoteControlCmd();

				switch (this.CurrentFlowFlag)
				{
					case eFlowFlag.�ȴ�����:
						#region
						timer1.Interval = 500;

						// ����������ظ����źţ������������߳���ʶ��
						if (this.UseRwer)
						{
							List<string> tags = Hardwarer.Rwer1.ScanTags();
							if (tags.Count > 0)
							{
								passCarQueuer.Enqueue(tags[0]);
								FrmDebugConsole.GetInstance().Output("ʶ�𵽿���:" + tags[0]);
							}
						}
						if (passCarQueuer.Count > 0) this.CurrentFlowFlag = eFlowFlag.��֤����;

						#endregion
						break;
					case eFlowFlag.��֤����:
						#region

						// �������޳�ʱ���ȴ�����
						if (passCarQueuer.Count == 0)
						{
							this.CurrentFlowFlag = eFlowFlag.�ȴ�����;
							break;
						}

						this.CurrentImperfectCar = passCarQueuer.Dequeue();

						// ��ʽһ������ʶ��ĳ��ƺŲ��ҳ�����Ϣ
						if (this.CurrentAutotruck == null)
							this.CurrentAutotruck = carTransportDAO.GetAutotruckByCarNumber(this.CurrentImperfectCar.Voucher);
						if (this.CurrentAutotruck == null)
							// ��ʽ��������ʶ��ı�ǩ�����ҳ�����Ϣ
							this.CurrentAutotruck = carTransportDAO.GetAutotruckByTagId(this.CurrentImperfectCar.Voucher);

						if (this.CurrentAutotruck != null)
						{
							UpdateLedShow(this.CurrentAutotruck.CarNumber + "ʶ��ɹ�");
							this.voiceSpeaker.Speak(this.CurrentAutotruck.CarNumber + " ʶ��ɹ�", 1, false);

							if (this.CurrentAutotruck.IsUse == 1)
							{
								if (this.CurrentAutotruck.CarriageLength > 0 && this.CurrentAutotruck.CarriageWidth > 0 && this.CurrentAutotruck.CarriageBottomToFloor > 0)
								{
									// δ��������¼
									CmcsUnFinishTransport unFinishTransport = carTransportDAO.GetUnFinishTransportByAutotruckId(this.CurrentAutotruck.Id, eCarType.�볧ú.ToString());
									if (unFinishTransport != null)
									{
										this.CurrentBuyFuelTransport = carTransportDAO.GetBuyFuelTransportById(unFinishTransport.TransportId);
										if (this.CurrentBuyFuelTransport != null)
										{
											// �ж�·������
											string nextPlace;
											if (carTransportDAO.CheckNextTruckInFactoryWay(this.CurrentAutotruck.CarType, this.CurrentBuyFuelTransport.StepName, "����", CommonAppConfig.GetInstance().AppIdentifier, out nextPlace))
											{
												BackGateUp();

												btnSendSamplingPlan.Enabled = true;

												this.CurrentFlowFlag = eFlowFlag.�ȴ�ʻ��;
												timer1.Interval = 1000;

												UpdateLedShow(this.CurrentAutotruck.CarNumber, " ʻ����������");
												this.voiceSpeaker.Speak(this.CurrentAutotruck.CarNumber + " ����ʻ����������", 1, false);
											}
											else
											{
												UpdateLedShow("·�ߴ���", "��ֹͨ��");
												this.voiceSpeaker.Speak("·�ߴ��� ��ֹͨ�� " + (!string.IsNullOrEmpty(nextPlace) ? "��ǰ��" + nextPlace : ""), 1, false);
												this.CurrentImperfectCar = null;
												timer1.Interval = 20000;
											}
										}
										else
										{
											commonDAO.SelfDber.Delete<CmcsUnFinishTransport>(unFinishTransport.Id);
										}
									}
									else
									{
										this.UpdateLedShow(this.CurrentAutotruck.CarNumber, "δ�Ŷ�");
										this.voiceSpeaker.Speak("���ƺ� " + this.CurrentAutotruck.CarNumber + " δ�ҵ��ŶӼ�¼", 1, false);
										this.CurrentImperfectCar = null;
										timer1.Interval = 10000;
									}
								}
								else
								{
									this.UpdateLedShow(this.CurrentAutotruck.CarNumber, "����δ����");
									this.voiceSpeaker.Speak("���ƺ� " + this.CurrentAutotruck.CarNumber + " ����δ����", 1, false);
									this.CurrentImperfectCar = null;
									timer1.Interval = 10000;
								}
							}
							else
							{
								UpdateLedShow(this.CurrentAutotruck.CarNumber, "��ͣ��");
								this.voiceSpeaker.Speak("���ƺ� " + this.CurrentAutotruck.CarNumber + " ��ͣ�ã���ֹͨ��", 1, false);
								this.CurrentImperfectCar = null;
								timer1.Interval = 10000;
							}
						}
						else
						{
							UpdateLedShow("δ�Ǽ�", "��ֹͨ��");

							// ��ʽһ������ʶ��
							this.voiceSpeaker.Speak(" δ�Ǽǽ�ֹͨ��", 1, false);
							//// ��ʽ����ˢ����ʽ
							//this.voiceSpeaker.Speak("����δ�Ǽǣ���ֹͨ��", 1, false);
							ResetBuyFuel();
							timer1.Interval = 10000;
						}

						#endregion
						break;
					case eFlowFlag.�ȴ�ʻ��:
						#region
						if (this.InductorCoil1) this.InductorCoil1State = true;
						if (this.InductorCoil1State && !HasCarOnCurrentInWay())
						{
							BackGateDown();
							FrontGateDown();
							this.CurrentFlowFlag = eFlowFlag.�ȴ���λ;

							timer1.Interval = 3000;
						}
						#endregion
						break;
					case eFlowFlag.�ȴ���λ:
						#region
						bool IsArrive = false, ChaoShenBo = false;
						ChaoShenBo = commonDAO.GetSignalDataValue(this.SamplerMachineCode, eSignalDataName.��������λ.ToString()) == "0";
						if (this.CurrentAutotruck.CarriageLength > 11000 && this.InfraredSensor1)
						{
							IsArrive = true;
						}
						else if (this.CurrentAutotruck.CarriageLength <= 11000 && !this.InfraredSensor1 && this.InfraredSensor2)
						{
							IsArrive = true;
						}

						if (IsArrive && ChaoShenBo)
						{
							UpdateLedShow("��Ϩ���³� ��ʼ����");
							this.voiceSpeaker.Speak("��Ϩ���³� ��ʼ����");
							this.CurrentFlowFlag = eFlowFlag.���ͼƻ�;
							timer1.Interval = 2000;
						}
						else
						{
							if (!ChaoShenBo)
							{
								UpdateLedShow("���������ڵ�");
								this.voiceSpeaker.Speak("���������ڵ�");
							}
							else
							{
								UpdateLedShow("ͣ������λ","��ǰ��");
								this.voiceSpeaker.Speak("ͣ������λ ����ǰ��");
							}
							timer1.Interval = 4000;
						}
						#endregion
						break;
					case eFlowFlag.���ͼƻ�:
						#region

						if (this.SamplerSystemStatus == eEquInfSamplerSystemStatus.��������)
						{
							CmcsRCSampling sampling = carTransportDAO.GetRCSamplingById(this.CurrentBuyFuelTransport.SamplingId);
							if (sampling != null)
							{
								txtSampleCode.Text = sampling.SampleCode;

								this.CurrentSampleCMD = new InfQCJXCYSampleCMD()
								{
									MachineCode = this.SamplerMachineCode,
									CarNumber = this.CurrentBuyFuelTransport.CarNumber,
									InFactoryBatchId = this.CurrentBuyFuelTransport.InFactoryBatchId,
									SampleCode = sampling.SampleCode,
									RFID = this.CurrentAutotruck.TheEPCCard != null ? this.CurrentAutotruck.TheEPCCard.TagId : "",
									Mt = 0,
									// ����Ԥ��
									TicketWeight = 0,
									// ����Ԥ��
									CarCount = 0,
									// ����������������߼�����
									PointCount = (int)dbtxtPointCount.Value,
									CarriageLength = this.CurrentAutotruck.CarriageLength,
									CarriageWidth = this.CurrentAutotruck.CarriageWidth,
									CarriageBottomToFloor = this.CurrentAutotruck.CarriageBottomToFloor,
									Obstacle1 = this.CurrentAutotruck.LeftObstacle1.ToString(),
									Obstacle2 = this.CurrentAutotruck.LeftObstacle2.ToString(),
									Obstacle3 = this.CurrentAutotruck.LeftObstacle3.ToString(),
									Obstacle4 = this.CurrentAutotruck.LeftObstacle4.ToString(),
									Obstacle5 = this.CurrentAutotruck.LeftObstacle5.ToString(),
									Obstacle6 = this.CurrentAutotruck.LeftObstacle6.ToString(),
									ResultCode = eEquInfCmdResultCode.Ĭ��.ToString(),
									DataFlag = 0,
									SuoFen = (int)dbtxtSuoFen.Value * 10,
									TransportId = this.CurrentBuyFuelTransport.Id
								};
								//TruckMeasure truckMeasure = new TruckMeasure(200, this.CurrentAutotruck.CarriageWidth, this.CurrentAutotruck.CarriageLength)
								//{
								//	CarriageToFloor = this.CurrentAutotruck.CarriageBottomToFloor,
								//	Obstacle1 = this.CurrentAutotruck.LeftObstacle1,
								//	Obstacle2 = this.CurrentAutotruck.LeftObstacle2,
								//	Obstacle3 = this.CurrentAutotruck.LeftObstacle3,
								//	Obstacle4 = this.CurrentAutotruck.LeftObstacle4,
								//	Obstacle5 = this.CurrentAutotruck.LeftObstacle5,
								//	Obstacle6 = this.CurrentAutotruck.LeftObstacle6
								//};
								//TruckCarriagePositioner truckCarriagePositioner = new TruckCarriagePositioner(truckMeasure);
								//List<Point> points = truckCarriagePositioner.GetPoints(this.CurrentSampleCMD.PointCount, ePointBuildMode.Partition);
								//this.CurrentSampleCMD.PointCount = points.Count;

								//if (points.Count >= 1) this.CurrentSampleCMD.Point1 = string.Format("{0},{1},{2}", points[0].X, points[0].Y, truckMeasure.CarriageToFloor - 150);
								//if (points.Count >= 2) this.CurrentSampleCMD.Point2 = string.Format("{0},{1},{2}", points[1].X, points[1].Y, truckMeasure.CarriageToFloor - 150);
								//if (points.Count >= 3) this.CurrentSampleCMD.Point3 = string.Format("{0},{1},{2}", points[2].X, points[2].Y, truckMeasure.CarriageToFloor - 150);
								//if (points.Count >= 4) this.CurrentSampleCMD.Point4 = string.Format("{0},{1},{2}", points[3].X, points[3].Y, truckMeasure.CarriageToFloor - 150);
								//if (points.Count >= 5) this.CurrentSampleCMD.Point5 = string.Format("{0},{1},{2}", points[4].X, points[4].Y, truckMeasure.CarriageToFloor - 150);
								//if (points.Count >= 6) this.CurrentSampleCMD.Point6 = string.Format("{0},{1},{2}", points[5].X, points[5].Y, truckMeasure.CarriageToFloor - 150);

								// ���Ͳ����ƻ�
								if (commonDAO.SelfDber.Insert<InfQCJXCYSampleCMD>(CurrentSampleCMD) > 0)
									this.CurrentFlowFlag = eFlowFlag.�ȴ�����;
							}
							else
							{
								this.UpdateLedShow("δ�ҵ���������Ϣ");
								this.voiceSpeaker.Speak("δ�ҵ���������Ϣ ����ϵ����Ա", 1, false);

								timer1.Interval = 5000;
							}
						}
						else
						{
							this.UpdateLedShow("������δ����");
							this.voiceSpeaker.Speak("������δ����", 1, false);

							timer1.Interval = 5000;
						}

						#endregion
						break;

					case eFlowFlag.�ȴ�����:
						#region

						// �жϲ����Ƿ����
						InfQCJXCYSampleCMD qCJXCYSampleCMD = commonDAO.SelfDber.Get<InfQCJXCYSampleCMD>(this.CurrentSampleCMD.Id);
						if (qCJXCYSampleCMD.ResultCode == eEquInfCmdResultCode.�ɹ�.ToString())
						{
							if (jxSamplerDAO.SaveBuyFuelTransport(this.CurrentBuyFuelTransport.Id, DateTime.Now, CommonAppConfig.GetInstance().AppIdentifier))
							{
								FrontGateUp();

								this.UpdateLedShow("�������", " ���뿪");
								this.voiceSpeaker.Speak("������ϣ����뿪", 1, false);

								this.CurrentFlowFlag = eFlowFlag.�ȴ��뿪;
							}
						}

						// ����������
						timer1.Interval = 4000;

						#endregion
						break;

					case eFlowFlag.�ȴ��뿪:
						if (this.InductorCoil2) this.InductorCoil2State = true;
						// �������򼰵�բ �ظ����ź�
						if (this.InductorCoil2State && !HasCarOnCurrentOutWay()) ResetBuyFuel();

						// ����������
						timer1.Interval = 4000;

						break;
				}

				// ���еظ����ź�ʱ����
				if (this.AutoHandMode && !this.InductorCoil1 && !this.InductorCoil2 && !this.InfraredSensor1 && !this.InfraredSensor2 && this.CurrentFlowFlag != eFlowFlag.�ȴ�����
				   && passCarQueuer.Count == 0 && this.CurrentImperfectCar == null) ResetBuyFuel();

				RefreshEquStatus();
			}
			catch (Exception ex)
			{
				Log4Neter.Error("timer1_Tick", ex);
			}
			finally
			{
				timer1.Start();
			}

			timer1.Start();
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			timer2.Stop();
			// ������ִ��һ��
			timer2.Interval = 180000;

			try
			{


			}
			catch (Exception ex)
			{
				Log4Neter.Error("timer2_Tick", ex);
			}
			finally
			{
				timer2.Start();
			}
		}

		/// <summary>
		/// �л��ֶ�/�Զ�ģʽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void sbtnChangeAutoHandMode_ValueChanged(object sender, EventArgs e)
		{
			this.AutoHandMode = sbtnChangeAutoHandMode.Value;
		}

		/// <summary>
		/// ִ��Զ������
		/// </summary>
		void ExecAppRemoteControlCmd()
		{
			// ��ȡ���µ�����
			CmcsAppRemoteControlCmd appRemoteControlCmd = commonDAO.GetNewestAppRemoteControlCmd(CommonAppConfig.GetInstance().AppIdentifier);
			if (appRemoteControlCmd != null)
			{
				if (appRemoteControlCmd.CmdCode == "���Ƶ�բ")
				{
					Log4Neter.Info("����Զ�����" + appRemoteControlCmd.CmdCode + "��������" + appRemoteControlCmd.Param);

					if (appRemoteControlCmd.Param.Equals("Gate1Up", StringComparison.CurrentCultureIgnoreCase))
						this.iocControler.Gate1Up();
					else if (appRemoteControlCmd.Param.Equals("Gate1Down", StringComparison.CurrentCultureIgnoreCase))
						this.iocControler.Gate1Down();
					else if (appRemoteControlCmd.Param.Equals("Gate2Up", StringComparison.CurrentCultureIgnoreCase))
						this.iocControler.Gate2Up();
					else if (appRemoteControlCmd.Param.Equals("Gate2Down", StringComparison.CurrentCultureIgnoreCase))
						this.iocControler.Gate2Down();

					// ����ִ�н��
					commonDAO.SetAppRemoteControlCmdResultCode(appRemoteControlCmd, eEquInfCmdResultCode.�ɹ�);
				}
			}
		}

		/// <summary>
		/// ˢ���б�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			// �볧ú
			LoadTodayUnFinishBuyFuelTransport();
			LoadTodayFinishBuyFuelTransport();
		}

		#endregion

		#region �볧úҵ��

		/// <summary>
		/// �����볧ú�����¼
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSendSamplingPlan_Click(object sender, EventArgs e)
		{
			if (SendSamplingPlan()) MessageBoxEx.Show("����ʧ��", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// �����볧ú�����¼
		/// </summary>
		/// <returns></returns>
		bool SendSamplingPlan()
		{
			if (this.CurrentBuyFuelTransport == null)
			{
				MessageBoxEx.Show("��ѡ����", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}

			this.CurrentFlowFlag = eFlowFlag.���ͼƻ�;

			return false;
		}

		/// <summary>
		/// �����볧ú�����¼
		/// </summary>
		void ResetBuyFuel()
		{
			this.CurrentFlowFlag = eFlowFlag.�ȴ�����;

			this.CurrentAutotruck = null;
			this.CurrentBuyFuelTransport = null;

			txtTagId.ResetText();
			txtSampleCode.ResetText();

			btnSendSamplingPlan.Enabled = false;
			this.InductorCoil1State = false;
			this.InductorCoil2State = false;

			UpdateLedShow("  �ȴ�����");

			// �������
			this.CurrentImperfectCar = null;
		}

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnReset_Click(object sender, EventArgs e)
		{
			ResetBuyFuel();
		}

		/// <summary>
		/// ��ȡδ��ɵ��볧ú��¼
		/// </summary>
		void LoadTodayUnFinishBuyFuelTransport()
		{
			superGridControl1.PrimaryGrid.DataSource = jxSamplerDAO.GetUnFinishBuyFuelTransport();
		}

		/// <summary>
		/// ��ȡָ����������ɵ��볧ú��¼
		/// </summary>
		void LoadTodayFinishBuyFuelTransport()
		{
			superGridControl2.PrimaryGrid.DataSource = jxSamplerDAO.GetFinishedBuyFuelTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
		}

		#endregion

		#region ������Ϣ

		/// <summary>
		/// ѡ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSelectAutotruck_Click(object sender, EventArgs e)
		{
			FrmUnFinishTransport_Select frm = new FrmUnFinishTransport_Select("where CarType='" + eCarType.�볧ú.ToString() + "' order by CreationTime desc");
			if (frm.ShowDialog() == DialogResult.OK)
			{
				passCarQueuer.Enqueue(frm.Output.CarNumber);
				this.CurrentFlowFlag = eFlowFlag.��֤����;
			}
		}

		#endregion

		#region ����
		Pen redPen1 = new Pen(Color.Red, 1);
		Pen greenPen1 = new Pen(Color.Lime, 1);

		Pen redPen3 = new Pen(Color.Red, 3);
		Pen greenPen3 = new Pen(Color.Lime, 3);

		/// <summary>
		/// ��ǰ����������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panCurrentCarNumber_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				PanelEx panel = sender as PanelEx;

				int height = 12;

				// ���Ƶظ�1
				e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, 1, 15, panel.Height - 1);
				//e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, panel.Height - height, 15, panel.Height - 1);

				// ���Ƶظ�2
				e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, panel.Width - 15, 1, panel.Width - 15, panel.Height - 1);
				//e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, panel.Width - 15, panel.Height - height, panel.Width - 15, panel.Height - 1);

				// ���ƶ���1
				e.Graphics.DrawLine(this.InfraredSensor1 ? redPen1 : greenPen1, panel.Width - 25, 1, panel.Width - 25, height);
				e.Graphics.DrawLine(this.InfraredSensor1 ? redPen1 : greenPen1, panel.Width - 25, panel.Height - height, panel.Width - 25, panel.Height - 1);
				// ���ƶ���2
				e.Graphics.DrawLine(this.InfraredSensor2 ? redPen1 : greenPen1, panel.Width - 35, 1, panel.Width - 35, height);
				e.Graphics.DrawLine(this.InfraredSensor2 ? redPen1 : greenPen1, panel.Width - 35, panel.Height - height, panel.Width - 35, panel.Height - 1);
			}
			catch (Exception ex)
			{
				Log4Neter.Error("panCurrentCarNumber_Paint�쳣", ex);
			}
		}

		private void superGridControl_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
		{
			// ȡ������༭
			e.Cancel = true;
		}

		/// <summary>
		/// �����к�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void superGridControl_GetRowHeaderText(object sender, DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs e)
		{
			e.Text = (e.GridRow.RowIndex + 1).ToString();
		}

		/// <summary>
		/// Invoke��װ
		/// </summary>
		/// <param name="action"></param>
		public void InvokeEx(Action action)
		{
			if (this.IsDisposed || !this.IsHandleCreated) return;

			this.Invoke(action);
		}

		/// <summary>
		/// ���²�����״̬
		/// </summary>
		private void RefreshEquStatus()
		{
			string systemStatus = commonDAO.GetSignalDataValue(this.SamplerMachineCode, eSignalDataName.�豸״̬.ToString());
			eEquInfSamplerSystemStatus result;
			if (Enum.TryParse(systemStatus, out result)) SamplerSystemStatus = result;
		}

		#endregion

	}
}
