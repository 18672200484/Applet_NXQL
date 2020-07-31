
namespace CMCS.CarTransport.Weighter.Core
{
    /// <summary>
    /// 硬件设备类
    /// </summary>
    public class Hardwarer
    {
        static IOC.JMDM20DIOV2.JMDM20DIOV2Iocer iocer = new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer();
        /// <summary>
        /// IO控制器
        /// </summary>
        public static IOC.JMDM20DIOV2.JMDM20DIOV2Iocer Iocer
        {
            get { return iocer; }
        }
        static IOC.JMDM20DIOV2.JMDM20DIOV2Iocer iocer2 = new IOC.JMDM20DIOV2.JMDM20DIOV2Iocer();
        /// <summary>
        /// IO控制器
        /// </summary>
        public static IOC.JMDM20DIOV2.JMDM20DIOV2Iocer Iocer2
        {
            get { return iocer2; }
        }

        static WB.JinZhong.JinZhongWber wber = new WB.JinZhong.JinZhongWber(4);
        /// <summary>
        /// 地磅仪表
        /// </summary>
        public static WB.JinZhong.JinZhongWber Wber
        {
            get { return wber; }
        }

        static RW.LZR12.Net.Lzr12Rwer rwer1 = new RW.LZR12.Net.Lzr12Rwer();
        /// <summary>
        /// 读卡器1
        /// </summary>
        public static RW.LZR12.Net.Lzr12Rwer Rwer1
        {
            get { return rwer1; }
        }

        static RW.LZR12.Net.Lzr12Rwer rwer2 = new RW.LZR12.Net.Lzr12Rwer();
        /// <summary>
        /// 读卡器2
        /// </summary>
        public static RW.LZR12.Net.Lzr12Rwer Rwer2
        {
            get { return rwer2; }
        }

        static HikVisionSDK.Core.IPCer iPCer_Identify1 = new HikVisionSDK.Core.IPCer();
        /// <summary>
        /// 车号识别1
        /// </summary>
        public static HikVisionSDK.Core.IPCer IPCer_Identify1
        {
            get { return iPCer_Identify1; }
        }

        static HikVisionSDK.Core.IPCer iPCer_Identify2 = new HikVisionSDK.Core.IPCer();
        /// <summary>
        /// 车号识别2
        /// </summary>
        public static HikVisionSDK.Core.IPCer IPCer_Identify2
        {
            get { return iPCer_Identify2; }
        }
    }
}
