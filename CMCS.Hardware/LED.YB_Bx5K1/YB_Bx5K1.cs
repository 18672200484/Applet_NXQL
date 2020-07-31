using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LED.YB_Bx5K1
{
    public class YB_Bx5K1
    {
        /// <summary>
        /// 当前连接状态
        /// </summary>
        bool ConnectStatus = false;

        void SetStatus(bool status)
        {
            this.ConnectStatus = status;
        }

        public YB_Bx5K1()
        {
            Led5kSDK.InitSdk(2, 2);
        }

        /// <summary>
        /// 当前连接句柄
        /// </summary>
        uint m_dwCurHand;

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool CreateListent(string ip, int port = 5005)
        {
            byte[] led_ip = System.Text.Encoding.ASCII.GetBytes(ip);
            uint led_port = Convert.ToUInt32(port);

            uint hand = Led5kSDK.CreateClient(led_ip, led_port, Led5kSDK.bx_5k_card_type.BX_5K1, 1, 1, null);
            m_dwCurHand = hand;
            if (hand == 0)
            {
                SetStatus(false);
                return false;
            }
            else
            {
                SetStatus(true);
                return true;
            }
        }

        /// <summary>
        /// 更新动态区域
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public bool UpdateArea(string value1, string value2 = "")
        {
            if (!this.ConnectStatus) return false;
            string value = value1 + value2;
            Led5kSDK.bx_5k_area_header bx_5k = new Led5kSDK.bx_5k_area_header();
            bx_5k.AreaType = 0x06;
            bx_5k.AreaX = 0;
            bx_5k.AreaY = 0;
            bx_5k.AreaWidth = 96 / 8;
            bx_5k.AreaHeight = 32;
            bx_5k.Lines_sizes = 1;//行间距
            bx_5k.RunMode = 0;//运行模式 0自动循环显示 1完成后停留在最后一页 2超时未完成删除该信息
            bx_5k.Timeout = 2;//超时时间 秒

            bx_5k.Reserved1 = 0;
            bx_5k.Reserved2 = 0;
            bx_5k.Reserved3 = 0;
            bx_5k.SingleLine = 0x02;
            bx_5k.NewLine = 0x02;//自动换行
            bx_5k.DisplayMode = 0x01;
            bx_5k.ExitMode = 0x00;
            bx_5k.Speed = 1;

            byte[] AreaText = System.Text.Encoding.Default.GetBytes(value);
            bx_5k.DataLen = AreaText.Length;

            int x = Led5kSDK.SCREEN_SendDynamicArea(m_dwCurHand, bx_5k, (ushort)bx_5k.DataLen, AreaText);
            return x == 0;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void CloseListent()
        {
            UpdateArea("  停止过磅");
            Led5kSDK.Destroy(m_dwCurHand);
            SetStatus(false);
        }
    }
}
