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
		/// 窗体唯一标识符
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
		/// 语音播报
		/// </summary>
		VoiceSpeaker voiceSpeaker = new VoiceSpeaker();

		bool autoHandMode = true;
		/// <summary>
		/// 手动模式=true  手动模式=false
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
		/// 地感1状态 true=有信号  false=无信号
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

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地感1信号.ToString(), value ? "1" : "0");
			}
		}

		int inductorCoil1Port;
		/// <summary>
		/// 地感1端口
		/// </summary>
		public int InductorCoil1Port
		{
			get { return inductorCoil1Port; }
			set { inductorCoil1Port = value; }
		}

		bool inductorCoil2 = false;
		/// <summary>
		/// 地感2状态 true=有信号  false=无信号
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

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地感2信号.ToString(), value ? "1" : "0");
			}
		}

		int inductorCoil2Port;
		/// <summary>
		/// 地感2端口
		/// </summary>
		public int InductorCoil2Port
		{
			get { return inductorCoil2Port; }
			set { inductorCoil2Port = value; }
		}

		bool infraredSensor1 = false;
		/// <summary>
		/// 对射1状态 true=遮挡  false=连通
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

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.对射1信号.ToString(), value ? "1" : "0");
			}
		}

		int infraredSensor1Port;
		/// <summary>
		/// 对射1端口
		/// </summary>
		public int InfraredSensor1Port
		{
			get { return infraredSensor1Port; }
			set { infraredSensor1Port = value; }
		}

		bool infraredSensor2 = false;
		/// <summary>
		/// 对射2状态 true=遮挡  false=连通
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

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.对射2信号.ToString(), value ? "1" : "0");
			}
		}

		int infraredSensor2Port;
		/// <summary>
		/// 对射2端口
		/// </summary>
		public int InfraredSensor2Port
		{
			get { return infraredSensor2Port; }
			set { infraredSensor2Port = value; }
		}

		/// <summary>
		/// 地感1（道闸地感）状态
		/// </summary>
		public bool InductorCoil1State = false;

		/// <summary>
		/// 地感2（道闸地感）状态
		/// </summary>
		public bool InductorCoil2State = false;

		public static PassCarQueuer passCarQueuer = new PassCarQueuer();

		ImperfectCar currentImperfectCar;
		/// <summary>
		/// 识别或选择的车辆凭证
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
					panCurrentCarNumber.Text = "等待车辆";
			}
		}

		eFlowFlag currentFlowFlag = eFlowFlag.等待车辆;
		/// <summary>
		/// 当前业务流程标识
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
		/// 当前运输记录
		/// </summary>
		public CmcsBuyFuelTransport CurrentBuyFuelTransport
		{
			get { return currentBuyFuelTransport; }
			set
			{
				currentBuyFuelTransport = value;

				if (value != null)
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), value.Id);

					txtSupplierName.Text = value.SupplierName;
					txtMineName.Text = value.MineName;
					txtTicketWeight.Text = value.TicketWeight.ToString();
					txtTransportCompanyName.Text = value.TransportCompanyName;
					txtFuelKindName.Text = value.FuelKindName;
				}
				else
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), string.Empty);

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
		/// 当前车
		/// </summary>
		public CmcsAutotruck CurrentAutotruck
		{
			get { return currentAutotruck; }
			set
			{
				currentAutotruck = value;

				if (value != null)
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车Id.ToString(), value.Id);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车号.ToString(), value.CarNumber);

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
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车Id.ToString(), string.Empty);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车号.ToString(), string.Empty);

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
		/// 当前采样命令
		/// </summary>
		public InfQCJXCYSampleCMD CurrentSampleCMD
		{
			get { return currentSampleCMD; }
			set { currentSampleCMD = value; }
		}

		private eEquInfSamplerSystemStatus samplerSystemStatus;
		/// <summary>
		/// 采样机系统状态
		/// </summary>
		public eEquInfSamplerSystemStatus SamplerSystemStatus
		{
			get { return samplerSystemStatus; }
			set
			{
				samplerSystemStatus = value;

				if (value == eEquInfSamplerSystemStatus.就绪待机)
					slightSamplerStatus.LightColor = EquipmentStatusColors.BeReady;
				else if (value == eEquInfSamplerSystemStatus.正在运行)
					slightSamplerStatus.LightColor = EquipmentStatusColors.Working;
				else if (value == eEquInfSamplerSystemStatus.发生故障)
					slightSamplerStatus.LightColor = EquipmentStatusColors.Breakdown;
				else if (value == eEquInfSamplerSystemStatus.系统停止)
					slightSamplerStatus.LightColor = EquipmentStatusColors.Forbidden;
			}
		}

		/// <summary>
		/// 采样机设备编码
		/// </summary>
		public string SamplerMachineCode;
		/// <summary>
		/// 采样机设备名称
		/// </summary>
		public string SamplerMachineName;

		/// <summary>
		/// 启用读卡器
		/// </summary>
		public bool UseRwer = true;

		/// <summary>
		/// 启用识别相机
		/// </summary>
		public bool UseCamera = true;
		#endregion

		/// <summary>
		/// 窗体初始化
		/// </summary>
		private void InitForm()
		{
			FrmDebugConsole.GetInstance();

			// 采样机设备编码
			this.SamplerMachineCode = commonDAO.GetAppletConfigString("采样机设备编码");
			this.SamplerMachineName = commonDAO.GetMachineNameByCode(this.SamplerMachineCode);
			this.UseRwer = commonDAO.GetAppletConfigInt32("启用读卡器") == 1;
			this.UseCamera = commonDAO.GetAppletConfigInt32("启用识别相机") == 1;
			// 默认自动
			sbtnChangeAutoHandMode.Value = true;

			// 重置程序远程控制命令
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
			// 卸载设备
			UnloadHardware();
		}

		#region 设备相关

		#region IO控制器

		void Iocer_StatusChange(bool status)
		{
			// 接收IO控制器状态 
			InvokeEx(() =>
			{
				slightIOC.LightColor = (status ? Color.Green : Color.Red);

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.IO控制器_连接状态.ToString(), status ? "1" : "0");
			});
		}

		/// <summary>
		/// IO控制器接收数据时触发
		/// </summary>
		/// <param name="receiveValue"></param>
		void Iocer_Received(int[] receiveValue)
		{
			// 接收地感状态  
			InvokeEx(() =>
			  {
				  this.InductorCoil1 = (receiveValue[this.InductorCoil1Port - 1] == 1);
				  this.InductorCoil2 = (receiveValue[this.InductorCoil2Port - 1] == 1);
				  this.InfraredSensor1 = (receiveValue[this.InfraredSensor1Port - 1] == 1);
				  this.InfraredSensor2 = (receiveValue[this.InfraredSensor2Port - 1] == 1);
			  });
		}

		/// <summary>
		/// 前方升杆
		/// </summary>
		void FrontGateUp()
		{
			this.iocControler.Gate2Up();
			this.iocControler.GreenLight1();
		}

		/// <summary>
		/// 前方降杆
		/// </summary>
		void FrontGateDown()
		{
			if (!this.InductorCoil2)
				this.iocControler.Gate2Down();
			this.iocControler.RedLight1();
		}

		/// <summary>
		/// 后方升杆
		/// </summary>
		void BackGateUp()
		{
			this.iocControler.Gate1Up();
			this.iocControler.GreenLight1();
		}

		/// <summary>
		/// 后方降杆
		/// </summary>
		void BackGateDown()
		{
			if (!this.InductorCoil1)
				this.iocControler.Gate1Down();
			this.iocControler.RedLight1();
		}

		/// <summary>
		/// 有车辆在入口道路上
		/// </summary>
		/// <returns></returns>
		bool HasCarOnCurrentInWay()
		{
			if (this.CurrentImperfectCar == null) return false;
			return this.InductorCoil1;
		}

		/// <summary>
		/// 有车辆在出口道路上
		/// </summary>
		/// <returns></returns>
		bool HasCarOnCurrentOutWay()
		{
			if (this.CurrentImperfectCar == null) return false;
			return this.InductorCoil2;
		}

		#endregion

		#region 读卡器

		void Rwer1_OnScanError(Exception ex)
		{
			Log4Neter.Error("读卡器1", ex);
		}

		void Rwer1_OnStatusChange(bool status)
		{
			// 接收读卡器1状态 
			InvokeEx(() =>
			 {
				 slightRwer1.LightColor = (status ? Color.Green : Color.Red);

				 commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.读卡器1_连接状态.ToString(), status ? "1" : "0");
			 });
		}

		#endregion

		#region LED控制卡
		YB_Bx5K1 LED1 = new YB_Bx5K1();
		/// <summary>
		/// LED1更新标识
		/// </summary>
		bool LED1m_bSendBusy = false;

		private bool _LED1ConnectStatus;
		/// <summary>
		/// LED1连接状态
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

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.LED屏1_连接状态.ToString(), value ? "1" : "0");
			}
		}

		/// <summary>
		/// LED1上一次显示内容
		/// </summary>
		string LED1PrevLedFileContent = string.Empty;

		/// <summary>
		/// 更新LED1动态区域
		/// </summary>
		/// <param name="value1">第一行内容</param>
		/// <param name="value2">第二行内容</param>
		private void UpdateLedShow(string value1 = "", string value2 = "")
		{
			if (this.LED1PrevLedFileContent == value1 + value2) return;
			FrmDebugConsole.GetInstance().Output("更新LED1:|" + value1 + "|" + value2 + "|");
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

		#region 海康网络抓拍相机

		/// <summary>
		/// 海康网络摄像机
		/// </summary>
		IPCer iPCer_Identify1 = new IPCer();

		void ReceiveData1(string carNumber, string IP)
		{
			InvokeEx(() =>
			  {
				  this.UpdateLedShow("识别到车号:" + carNumber);
				  if (this.UseCamera && this.CurrentFlowFlag == eFlowFlag.等待车辆)
				  {
					  passCarQueuer.Enqueue(carNumber);
					  this.CurrentFlowFlag = eFlowFlag.验证车辆;
					  timer1_Tick(null, null);
				  }
			  });
		}

		#endregion
		#endregion

		#region 设备初始化与卸载

		/// <summary>
		/// 初始化外接设备
		/// </summary>
		private void InitHardware()
		{
			try
			{
				bool success = false;

				this.InductorCoil1Port = commonDAO.GetAppletConfigInt32("IO控制器_地感1端口");
				this.InductorCoil2Port = commonDAO.GetAppletConfigInt32("IO控制器_地感2端口");
				this.InfraredSensor1Port = commonDAO.GetAppletConfigInt32("IO控制器_对射1端口");
				this.InfraredSensor2Port = commonDAO.GetAppletConfigInt32("IO控制器_对射2端口");

				// IO控制器
				Hardwarer.Iocer.OnReceived += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.ReceivedEventHandler(Iocer_Received);
				Hardwarer.Iocer.OnStatusChange += new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer.StatusChangeHandler(Iocer_StatusChange);
				success = Hardwarer.Iocer.OpenCom(commonDAO.GetAppletConfigInt32("IO控制器_串口"), commonDAO.GetAppletConfigInt32("IO控制器_波特率"), commonDAO.GetAppletConfigInt32("IO控制器_数据位"), (StopBits)commonDAO.GetAppletConfigInt32("IO控制器_停止位"), (Parity)commonDAO.GetAppletConfigInt32("IO控制器_校验位"));
				if (!success) MessageBoxEx.Show("IO控制器连接失败！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				this.iocControler = new IocControler(Hardwarer.Iocer);

				// 读卡器1
				Hardwarer.Rwer1.StartWith = commonDAO.GetAppletConfigString("读卡器_标签过滤");
				Hardwarer.Rwer1.OnStatusChange += new RW.LZR12.Net.Lzr12Rwer.StatusChangeHandler(Rwer1_OnStatusChange);
				Hardwarer.Rwer1.OnScanError += new RW.LZR12.Net.Lzr12Rwer.ScanErrorEventHandler(Rwer1_OnScanError);
				success = CommonUtil.PingReplyTest(commonDAO.GetAppletConfigString("读卡器1_IP地址")) && Hardwarer.Rwer1.OpenCom(commonDAO.GetAppletConfigString("读卡器1_IP地址"), 500, Convert.ToByte(commonDAO.GetAppletConfigInt32("读卡器1_功率")));
				if (!success) MessageBoxEx.Show("读卡器1连接失败！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				#region LED控制卡1

				string led1SocketIP = commonDAO.GetAppletConfigString("LED显示屏1_IP地址");
				if (!string.IsNullOrEmpty(led1SocketIP))
				{
					if (CommonUtil.PingReplyTest(led1SocketIP))
					{
						if (LED1.CreateListent(led1SocketIP))
						{
							// 初始化成功
							this.LED1ConnectStatus = true;
							UpdateLedShow("  等待车辆");
						}
						else
						{
							this.LED1ConnectStatus = false;
							Log4Neter.Error("LED1控制卡连接失败", new Exception("连接失败"));
							MessageBoxEx.Show("LED1控制卡连接失败", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					}
					else
					{
						this.LED1ConnectStatus = false;
						Log4Neter.Error("初始化LED1控制卡，网络连接失败", new Exception("网络异常"));
						MessageBoxEx.Show("LED1控制卡网络连接失败！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}

				#endregion

				IPCer.InitSDK();
				//识别摄像机
				CmcsCamare video_Identify1 = commonDAO.SelfDber.Entity<CmcsCamare>("where Name=:Name", new { Name = CommonAppConfig.GetInstance().AppIdentifier + "车号识别1" });
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

				//语音设置
				voiceSpeaker.SetVoice(commonDAO.GetAppletConfigInt32("语速"), commonDAO.GetAppletConfigInt32("音量"), commonDAO.GetAppletConfigString("语音包"));

				timer1.Enabled = true;
			}
			catch (Exception ex)
			{
				Log4Neter.Error("设备初始化", ex);
			}
		}

		/// <summary>
		/// 卸载设备
		/// </summary>
		private void UnloadHardware()
		{
			// 注意此段代码
			Application.DoEvents();

			try
			{
				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车Id.ToString(), string.Empty);
				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车号.ToString(), string.Empty);
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

		#region 道闸控制按钮

		/// <summary>
		/// 道闸1升杆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate1Up_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate1Up();
		}

		/// <summary>
		/// 道闸1降杆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate1Down_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate1Down();
		}

		/// <summary>
		/// 道闸2升杆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate2Up_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate2Up();
		}

		/// <summary>
		/// 道闸2降杆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate2Down_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate2Down();
		}

		#endregion

		#region 公共业务

		/// <summary>
		/// 读卡、车号识别任务
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Stop();

			try
			{
				// 执行远程命令
				ExecAppRemoteControlCmd();

				switch (this.CurrentFlowFlag)
				{
					case eFlowFlag.等待车辆:
						#region
						timer1.Interval = 500;

						// 当读卡区域地感有信号，触发读卡或者车号识别
						if (this.UseRwer)
						{
							List<string> tags = Hardwarer.Rwer1.ScanTags();
							if (tags.Count > 0)
							{
								passCarQueuer.Enqueue(tags[0]);
								FrmDebugConsole.GetInstance().Output("识别到卡号:" + tags[0]);
							}
						}
						if (passCarQueuer.Count > 0) this.CurrentFlowFlag = eFlowFlag.验证车辆;

						#endregion
						break;
					case eFlowFlag.验证车辆:
						#region

						// 队列中无车时，等待车辆
						if (passCarQueuer.Count == 0)
						{
							this.CurrentFlowFlag = eFlowFlag.等待车辆;
							break;
						}

						this.CurrentImperfectCar = passCarQueuer.Dequeue();

						// 方式一：根据识别的车牌号查找车辆信息
						if (this.CurrentAutotruck == null)
							this.CurrentAutotruck = carTransportDAO.GetAutotruckByCarNumber(this.CurrentImperfectCar.Voucher);
						if (this.CurrentAutotruck == null)
							// 方式二：根据识别的标签卡查找车辆信息
							this.CurrentAutotruck = carTransportDAO.GetAutotruckByTagId(this.CurrentImperfectCar.Voucher);

						if (this.CurrentAutotruck != null)
						{
							UpdateLedShow(this.CurrentAutotruck.CarNumber + "识别成功");
							this.voiceSpeaker.Speak(this.CurrentAutotruck.CarNumber + " 识别成功", 1, false);

							if (this.CurrentAutotruck.IsUse == 1)
							{
								if (this.CurrentAutotruck.CarriageLength > 0 && this.CurrentAutotruck.CarriageWidth > 0 && this.CurrentAutotruck.CarriageBottomToFloor > 0)
								{
									// 未完成运输记录
									CmcsUnFinishTransport unFinishTransport = carTransportDAO.GetUnFinishTransportByAutotruckId(this.CurrentAutotruck.Id, eCarType.入厂煤.ToString());
									if (unFinishTransport != null)
									{
										this.CurrentBuyFuelTransport = carTransportDAO.GetBuyFuelTransportById(unFinishTransport.TransportId);
										if (this.CurrentBuyFuelTransport != null)
										{
											// 判断路线设置
											string nextPlace;
											if (carTransportDAO.CheckNextTruckInFactoryWay(this.CurrentAutotruck.CarType, this.CurrentBuyFuelTransport.StepName, "采样", CommonAppConfig.GetInstance().AppIdentifier, out nextPlace))
											{
												BackGateUp();

												btnSendSamplingPlan.Enabled = true;

												this.CurrentFlowFlag = eFlowFlag.等待驶入;
												timer1.Interval = 1000;

												UpdateLedShow(this.CurrentAutotruck.CarNumber, " 驶至采样区域");
												this.voiceSpeaker.Speak(this.CurrentAutotruck.CarNumber + " 请行驶至采样区域", 1, false);
											}
											else
											{
												UpdateLedShow("路线错误", "禁止通过");
												this.voiceSpeaker.Speak("路线错误 禁止通过 " + (!string.IsNullOrEmpty(nextPlace) ? "请前往" + nextPlace : ""), 1, false);
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
										this.UpdateLedShow(this.CurrentAutotruck.CarNumber, "未排队");
										this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 未找到排队记录", 1, false);
										this.CurrentImperfectCar = null;
										timer1.Interval = 10000;
									}
								}
								else
								{
									this.UpdateLedShow(this.CurrentAutotruck.CarNumber, "车厢未测量");
									this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 车厢未测量", 1, false);
									this.CurrentImperfectCar = null;
									timer1.Interval = 10000;
								}
							}
							else
							{
								UpdateLedShow(this.CurrentAutotruck.CarNumber, "已停用");
								this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 已停用，禁止通过", 1, false);
								this.CurrentImperfectCar = null;
								timer1.Interval = 10000;
							}
						}
						else
						{
							UpdateLedShow("未登记", "禁止通过");

							// 方式一：车号识别
							this.voiceSpeaker.Speak(" 未登记禁止通过", 1, false);
							//// 方式二：刷卡方式
							//this.voiceSpeaker.Speak("卡号未登记，禁止通过", 1, false);
							ResetBuyFuel();
							timer1.Interval = 10000;
						}

						#endregion
						break;
					case eFlowFlag.等待驶入:
						#region
						if (this.InductorCoil1) this.InductorCoil1State = true;
						if (this.InductorCoil1State && !HasCarOnCurrentInWay())
						{
							BackGateDown();
							FrontGateDown();
							this.CurrentFlowFlag = eFlowFlag.等待到位;

							timer1.Interval = 3000;
						}
						#endregion
						break;
					case eFlowFlag.等待到位:
						#region
						bool IsArrive = false, ChaoShenBo = false;
						ChaoShenBo = commonDAO.GetSignalDataValue(this.SamplerMachineCode, eSignalDataName.超声波就位.ToString()) == "0";
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
							UpdateLedShow("请熄火下车 开始采样");
							this.voiceSpeaker.Speak("请熄火下车 开始采样");
							this.CurrentFlowFlag = eFlowFlag.发送计划;
							timer1.Interval = 2000;
						}
						else
						{
							if (!ChaoShenBo)
							{
								UpdateLedShow("超声波被遮挡");
								this.voiceSpeaker.Speak("超声波被遮挡");
							}
							else
							{
								UpdateLedShow("停车不到位","请前移");
								this.voiceSpeaker.Speak("停车不到位 请往前开");
							}
							timer1.Interval = 4000;
						}
						#endregion
						break;
					case eFlowFlag.发送计划:
						#region

						if (this.SamplerSystemStatus == eEquInfSamplerSystemStatus.就绪待机)
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
									// 根据预报
									TicketWeight = 0,
									// 根据预报
									CarCount = 0,
									// 采样点数根据相关逻辑计算
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
									ResultCode = eEquInfCmdResultCode.默认.ToString(),
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

								// 发送采样计划
								if (commonDAO.SelfDber.Insert<InfQCJXCYSampleCMD>(CurrentSampleCMD) > 0)
									this.CurrentFlowFlag = eFlowFlag.等待采样;
							}
							else
							{
								this.UpdateLedShow("未找到采样单信息");
								this.voiceSpeaker.Speak("未找到采样单信息 请联系管理员", 1, false);

								timer1.Interval = 5000;
							}
						}
						else
						{
							this.UpdateLedShow("采样机未就绪");
							this.voiceSpeaker.Speak("采样机未就绪", 1, false);

							timer1.Interval = 5000;
						}

						#endregion
						break;

					case eFlowFlag.等待采样:
						#region

						// 判断采样是否完成
						InfQCJXCYSampleCMD qCJXCYSampleCMD = commonDAO.SelfDber.Get<InfQCJXCYSampleCMD>(this.CurrentSampleCMD.Id);
						if (qCJXCYSampleCMD.ResultCode == eEquInfCmdResultCode.成功.ToString())
						{
							if (jxSamplerDAO.SaveBuyFuelTransport(this.CurrentBuyFuelTransport.Id, DateTime.Now, CommonAppConfig.GetInstance().AppIdentifier))
							{
								FrontGateUp();

								this.UpdateLedShow("采样完毕", " 请离开");
								this.voiceSpeaker.Speak("采样完毕，请离开", 1, false);

								this.CurrentFlowFlag = eFlowFlag.等待离开;
							}
						}

						// 降低灵敏度
						timer1.Interval = 4000;

						#endregion
						break;

					case eFlowFlag.等待离开:
						if (this.InductorCoil2) this.InductorCoil2State = true;
						// 采样区域及道闸 地感无信号
						if (this.InductorCoil2State && !HasCarOnCurrentOutWay()) ResetBuyFuel();

						// 降低灵敏度
						timer1.Interval = 4000;

						break;
				}

				// 所有地感无信号时重置
				if (this.AutoHandMode && !this.InductorCoil1 && !this.InductorCoil2 && !this.InfraredSensor1 && !this.InfraredSensor2 && this.CurrentFlowFlag != eFlowFlag.等待车辆
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
			// 三分钟执行一次
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
		/// 切换手动/自动模式
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void sbtnChangeAutoHandMode_ValueChanged(object sender, EventArgs e)
		{
			this.AutoHandMode = sbtnChangeAutoHandMode.Value;
		}

		/// <summary>
		/// 执行远程命令
		/// </summary>
		void ExecAppRemoteControlCmd()
		{
			// 获取最新的命令
			CmcsAppRemoteControlCmd appRemoteControlCmd = commonDAO.GetNewestAppRemoteControlCmd(CommonAppConfig.GetInstance().AppIdentifier);
			if (appRemoteControlCmd != null)
			{
				if (appRemoteControlCmd.CmdCode == "控制道闸")
				{
					Log4Neter.Info("接收远程命令：" + appRemoteControlCmd.CmdCode + "，参数：" + appRemoteControlCmd.Param);

					if (appRemoteControlCmd.Param.Equals("Gate1Up", StringComparison.CurrentCultureIgnoreCase))
						this.iocControler.Gate1Up();
					else if (appRemoteControlCmd.Param.Equals("Gate1Down", StringComparison.CurrentCultureIgnoreCase))
						this.iocControler.Gate1Down();
					else if (appRemoteControlCmd.Param.Equals("Gate2Up", StringComparison.CurrentCultureIgnoreCase))
						this.iocControler.Gate2Up();
					else if (appRemoteControlCmd.Param.Equals("Gate2Down", StringComparison.CurrentCultureIgnoreCase))
						this.iocControler.Gate2Down();

					// 更新执行结果
					commonDAO.SetAppRemoteControlCmdResultCode(appRemoteControlCmd, eEquInfCmdResultCode.成功);
				}
			}
		}

		/// <summary>
		/// 刷新列表
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			// 入厂煤
			LoadTodayUnFinishBuyFuelTransport();
			LoadTodayFinishBuyFuelTransport();
		}

		#endregion

		#region 入厂煤业务

		/// <summary>
		/// 保存入厂煤运输记录
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSendSamplingPlan_Click(object sender, EventArgs e)
		{
			if (SendSamplingPlan()) MessageBoxEx.Show("发送失败", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// 保存入厂煤运输记录
		/// </summary>
		/// <returns></returns>
		bool SendSamplingPlan()
		{
			if (this.CurrentBuyFuelTransport == null)
			{
				MessageBoxEx.Show("请选择车辆", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}

			this.CurrentFlowFlag = eFlowFlag.发送计划;

			return false;
		}

		/// <summary>
		/// 重置入厂煤运输记录
		/// </summary>
		void ResetBuyFuel()
		{
			this.CurrentFlowFlag = eFlowFlag.等待车辆;

			this.CurrentAutotruck = null;
			this.CurrentBuyFuelTransport = null;

			txtTagId.ResetText();
			txtSampleCode.ResetText();

			btnSendSamplingPlan.Enabled = false;
			this.InductorCoil1State = false;
			this.InductorCoil2State = false;

			UpdateLedShow("  等待车辆");

			// 最后重置
			this.CurrentImperfectCar = null;
		}

		/// <summary>
		/// 重置
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnReset_Click(object sender, EventArgs e)
		{
			ResetBuyFuel();
		}

		/// <summary>
		/// 获取未完成的入厂煤记录
		/// </summary>
		void LoadTodayUnFinishBuyFuelTransport()
		{
			superGridControl1.PrimaryGrid.DataSource = jxSamplerDAO.GetUnFinishBuyFuelTransport();
		}

		/// <summary>
		/// 获取指定日期已完成的入厂煤记录
		/// </summary>
		void LoadTodayFinishBuyFuelTransport()
		{
			superGridControl2.PrimaryGrid.DataSource = jxSamplerDAO.GetFinishedBuyFuelTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
		}

		#endregion

		#region 基础信息

		/// <summary>
		/// 选择车辆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSelectAutotruck_Click(object sender, EventArgs e)
		{
			FrmUnFinishTransport_Select frm = new FrmUnFinishTransport_Select("where CarType='" + eCarType.入厂煤.ToString() + "' order by CreationTime desc");
			if (frm.ShowDialog() == DialogResult.OK)
			{
				passCarQueuer.Enqueue(frm.Output.CarNumber);
				this.CurrentFlowFlag = eFlowFlag.验证车辆;
			}
		}

		#endregion

		#region 其他
		Pen redPen1 = new Pen(Color.Red, 1);
		Pen greenPen1 = new Pen(Color.Lime, 1);

		Pen redPen3 = new Pen(Color.Red, 3);
		Pen greenPen3 = new Pen(Color.Lime, 3);

		/// <summary>
		/// 当前车号面板绘制
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panCurrentCarNumber_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				PanelEx panel = sender as PanelEx;

				int height = 12;

				// 绘制地感1
				e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, 1, 15, panel.Height - 1);
				//e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, panel.Height - height, 15, panel.Height - 1);

				// 绘制地感2
				e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, panel.Width - 15, 1, panel.Width - 15, panel.Height - 1);
				//e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, panel.Width - 15, panel.Height - height, panel.Width - 15, panel.Height - 1);

				// 绘制对射1
				e.Graphics.DrawLine(this.InfraredSensor1 ? redPen1 : greenPen1, panel.Width - 25, 1, panel.Width - 25, height);
				e.Graphics.DrawLine(this.InfraredSensor1 ? redPen1 : greenPen1, panel.Width - 25, panel.Height - height, panel.Width - 25, panel.Height - 1);
				// 绘制对射2
				e.Graphics.DrawLine(this.InfraredSensor2 ? redPen1 : greenPen1, panel.Width - 35, 1, panel.Width - 35, height);
				e.Graphics.DrawLine(this.InfraredSensor2 ? redPen1 : greenPen1, panel.Width - 35, panel.Height - height, panel.Width - 35, panel.Height - 1);
			}
			catch (Exception ex)
			{
				Log4Neter.Error("panCurrentCarNumber_Paint异常", ex);
			}
		}

		private void superGridControl_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
		{
			// 取消进入编辑
			e.Cancel = true;
		}

		/// <summary>
		/// 设置行号
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void superGridControl_GetRowHeaderText(object sender, DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs e)
		{
			e.Text = (e.GridRow.RowIndex + 1).ToString();
		}

		/// <summary>
		/// Invoke封装
		/// </summary>
		/// <param name="action"></param>
		public void InvokeEx(Action action)
		{
			if (this.IsDisposed || !this.IsHandleCreated) return;

			this.Invoke(action);
		}

		/// <summary>
		/// 更新采样机状态
		/// </summary>
		private void RefreshEquStatus()
		{
			string systemStatus = commonDAO.GetSignalDataValue(this.SamplerMachineCode, eSignalDataName.设备状态.ToString());
			eEquInfSamplerSystemStatus result;
			if (Enum.TryParse(systemStatus, out result)) SamplerSystemStatus = result;
		}

		#endregion

	}
}
