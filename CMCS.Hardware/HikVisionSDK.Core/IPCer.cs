using HikVisionSDK.Core.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace HikVisionSDK.Core
{
    /// <summary>
    /// 网络摄像机 预览、抓拍、录像功能封装
    /// </summary>
    public class IPCer
    {
        /// <summary>
        /// 用户登录ID
        /// </summary>
        int m_lUserID = -1;
        /// <summary>
        /// 预览句柄ID
        /// </summary>
        int lRealHandle = -1;

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP = string.Empty;
        /// <summary>
        /// 初始化 SDK
        /// </summary>
        /// <returns></returns>
        public static bool InitSDK()
        {
            bool res = CHCNetSDK.NET_DVR_Init();
            CHCNetSDK.NET_DVR_SetConnectTime(20000, 1);
            CHCNetSDK.NET_DVR_SetReconnect(100000, 1);
            return res;
        }

        /// <summary>
        /// 卸载 SDK
        /// </summary>
        /// <returns></returns>
        public static bool CleanupSDK()
        {
            return CHCNetSDK.NET_DVR_Cleanup();
        }

        /// <summary>
        /// 返回最后操作的错误码 
        /// </summary>
        /// <returns></returns>
        public static uint GetLastErrorCode()
        {
            return CHCNetSDK.NET_DVR_GetLastError();
        }

        /// <summary>
        /// 登录摄像机
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="userAccount"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(string ipAddress, int port, string userAccount, string password)
        {
            this.IP = ipAddress;
            CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

            m_lUserID = CHCNetSDK.NET_DVR_Login_V30(ipAddress, port, userAccount, password, ref DeviceInfo);
            return m_lUserID >= 0;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public bool LoginOut()
        {
            if (m_lUserID < 0) return false;

            bool res = CHCNetSDK.NET_DVR_Logout(m_lUserID);
            if (res) this.m_lUserID = -1;

            return res;
        }

        /// <summary>
        /// 开始预览
        /// </summary>
        /// <param name="previewInfo"></param>
        /// <returns></returns>
        public bool StartPreview(CHCNetSDK.NET_DVR_PREVIEWINFO previewInfo)
        {
            if (m_lUserID < 0) return false;

            lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref previewInfo, null, new IntPtr());
            return lRealHandle < 0;
        }

        /// <summary>
        /// 开始预览
        /// </summary>
        /// <param name="previewHandle">预览句柄</param>
        /// <param name="channel">通道号</param>
        /// <param name="linkMode">连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP </param>
        /// <param name="streamType">码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推</param>
        /// <returns></returns>
        public bool StartPreview(IntPtr previewHandle, int channel, int linkMode = 0, int streamType = 0)
        {
            CHCNetSDK.NET_DVR_PREVIEWINFO previewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            // 预览窗口
            previewInfo.hPlayWnd = previewHandle;
            // 预览的设备通道
            previewInfo.lChannel = channel;
            // 码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
            previewInfo.dwStreamType = 0;
            // 连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
            previewInfo.dwLinkMode = 0;
            // 0- 非阻塞取流，1- 阻塞取流
            previewInfo.bBlocked = true;
            // 播放库播放缓冲区最大缓冲帧数
            previewInfo.dwDisplayBufNum = 15;
            return StartPreview(previewInfo);
        }

        /// <summary>
        /// 停止预览
        /// </summary>
        /// <returns></returns>
        public bool StopPreview()
        {
            if (m_lUserID < 0 || lRealHandle < 0) return false;

            bool res = CHCNetSDK.NET_DVR_StopRealPlay(lRealHandle);
            if (res) this.lRealHandle = -1;

            return res;
        }

        /// <summary>
        /// 抓拍照片
        /// </summary>
        /// <param name="fileName">保存文件路径</param>
        /// <returns></returns>
        public bool CapturePicture(string fileName)
        {
            if (m_lUserID < 0 || lRealHandle < 0) return false;

            return CHCNetSDK.NET_DVR_CapturePicture(lRealHandle, fileName);
        }

        /// <summary>
        /// 开始录像
        /// </summary>
        /// <param name="fileName">录像文件路径 *.mp4</param>
        /// <param name="channel">通道号</param>
        /// <returns></returns>
        public bool StartRecord(string fileName, int channel)
        {
            if (m_lUserID < 0 || lRealHandle < 0) return false;

            CHCNetSDK.NET_DVR_MakeKeyFrame(m_lUserID, channel);

            return CHCNetSDK.NET_DVR_SaveRealData(this.lRealHandle, fileName);
        }

        /// <summary>
        /// 停止录像
        /// </summary>
        /// <returns></returns>
        public bool StopRecord()
        {
            if (m_lUserID < 0 || lRealHandle < 0) return false;

            return CHCNetSDK.NET_DVR_StopSaveRealData(lRealHandle);
        }

        #region 布防信息 没有布防功能的摄像机无需调用该方法
        private CHCNetSDK.MSGCallBack_V31 m_falarmData_V31 = null;

        /// <summary>
        /// 布防接收车号数据
        /// </summary>
        /// <param name="carNumber"></param>
        public delegate void ReceivedEventHandler(string carNumber, string IP);
        public event ReceivedEventHandler OnReceived;
        //public Func<string> OnReceived(string ip);

        //public Func<string> OnReceived2(string ip);
        public int alarmResult;

        /// <summary>
        /// 设置布防回调函数
        /// </summary>
        /// <returns></returns>
        public bool SetDVRCallBack()
        {
            if (m_falarmData_V31 == null)
                m_falarmData_V31 = new CHCNetSDK.MSGCallBack_V31(MsgCallback_V31);
            return CHCNetSDK.NET_DVR_SetDVRMessageCallBack_V31(m_falarmData_V31, IntPtr.Zero);
        }

        #region 布放回调

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="lCommand">报警消息类型</param>
        /// <param name="pAlarmer">报警设备信息</param>
        /// <param name="pAlarmInfo">报警信息</param>
        /// <param name="dwBufLen">报警信息缓存大小</param>
        /// <param name="pUser">用户信息</param>
        /// <returns></returns>
        private bool MsgCallback_V31(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            //通过lCommand来判断接收到的报警信息类型，不同的lCommand对应不同的pAlarmInfo内容
            AlarmMessageHandle(lCommand, ref pAlarmer, pAlarmInfo, dwBufLen, pUser);

            return true; //回调函数需要有返回，表示正常接收到数据
        }

        /// <summary>
        /// 判断报警类型
        /// </summary>
        /// <param name="lCommand"></param>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        private void AlarmMessageHandle(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            //通过lCommand来判断接收到的报警信息类型，不同的lCommand对应不同的pAlarmInfo内容
            switch (lCommand)
            {
                case CHCNetSDK.COMM_UPLOAD_PLATE_RESULT://交通抓拍结果上传(老报警信息类型)
                    ProcessCommAlarm_Plate(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                default:
                    break;
            }
        }

        private void ProcessCommAlarm_Plate(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            CHCNetSDK.NET_DVR_PLATE_RESULT struPlateResultInfo = new CHCNetSDK.NET_DVR_PLATE_RESULT();
            uint dwSize = (uint)Marshal.SizeOf(struPlateResultInfo);

            struPlateResultInfo = (CHCNetSDK.NET_DVR_PLATE_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_PLATE_RESULT));

            ////保存抓拍图片
            //string str = "";
            //if (struPlateResultInfo.byResultType == 1 && struPlateResultInfo.dwPicLen != 0)
            //{
            //    str = ".\\picture\\Plate_UserID_" + pAlarmer.lUserID + "_近景图_" + iFileNumber + ".jpg";
            //    FileStream fs = new FileStream(str, FileMode.Create);
            //    int iLen = (int)struPlateResultInfo.dwPicLen;
            //    byte[] by = new byte[iLen];
            //    Marshal.Copy(struPlateResultInfo.pBuffer1, by, 0, iLen);
            //    fs.Write(by, 0, iLen);
            //    fs.Close();
            //    iFileNumber++;
            //}
            //if (struPlateResultInfo.dwPicPlateLen != 0)
            //{
            //    str = ".\\picture\\Plate_UserID_" + pAlarmer.lUserID + "_车牌图_" + iFileNumber + ".jpg";
            //    FileStream fs = new FileStream(str, FileMode.Create);
            //    int iLen = (int)struPlateResultInfo.dwPicPlateLen;
            //    byte[] by = new byte[iLen];
            //    Marshal.Copy(struPlateResultInfo.pBuffer2, by, 0, iLen);
            //    fs.Write(by, 0, iLen);
            //    fs.Close();
            //    iFileNumber++;
            //}
            //if (struPlateResultInfo.dwFarCarPicLen != 0)
            //{
            //    str = ".\\picture\\Plate_UserID_" + pAlarmer.lUserID + "_远景图_" + iFileNumber + ".jpg";
            //    FileStream fs = new FileStream(str, FileMode.Create);
            //    int iLen = (int)struPlateResultInfo.dwFarCarPicLen;
            //    byte[] by = new byte[iLen];
            //    Marshal.Copy(struPlateResultInfo.pBuffer5, by, 0, iLen);
            //    fs.Write(by, 0, iLen);
            //    fs.Close();
            //    iFileNumber++;
            //}

            //抓拍时间：年月日时分秒
            //string strTimeYear = System.Text.Encoding.UTF8.GetString(struPlateResultInfo.byAbsTime).TrimEnd('\0');

            //报警设备IP地址
            string strIP = System.Text.Encoding.UTF8.GetString(pAlarmer.sDeviceIP).TrimEnd('\0');

            //车牌信息
            string stringPlateLicense = System.Text.Encoding.GetEncoding("GBK").GetString(struPlateResultInfo.struPlateInfo.sLicense).TrimEnd('\0').Replace("无车牌", "");
            if (!string.IsNullOrEmpty(stringPlateLicense))
            {
                if (stringPlateLicense.Length > 7)
                    stringPlateLicense = stringPlateLicense.Substring(stringPlateLicense.Length - 7, 7);
                if (OnReceived != null)
                    OnReceived(stringPlateLicense, strIP);
            }
        }

        #endregion

        /// <summary>
        /// 启动布防
        /// </summary>
        /// <returns></returns>
        public bool SetupAlarm()
        {
            CHCNetSDK.NET_DVR_SETUPALARM_PARAM struAlarmParam = new CHCNetSDK.NET_DVR_SETUPALARM_PARAM();
            struAlarmParam.dwSize = (uint)Marshal.SizeOf(struAlarmParam);
            struAlarmParam.byLevel = 1; //0- 一级布防,1- 二级布防
            struAlarmParam.byAlarmInfoType = 0;//智能交通设备有效，新报警信息类型
            struAlarmParam.byDeployType = 0;
            struAlarmParam.byFaceAlarmDetection = 0;//1-人脸侦测

            int m_lAlarmHandle = CHCNetSDK.NET_DVR_SetupAlarmChan_V41(m_lUserID, ref struAlarmParam);
            if (m_lAlarmHandle < 0)
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 关闭布防
        /// </summary>
        public bool CloseAlarm()
        {
            return CHCNetSDK.NET_DVR_CloseAlarmChan_V30(this.alarmResult);
        }
        #endregion
    }
}
