using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Enums
{
    /// <summary>
    /// 第三方设备接口 - 采样机系统状态
    /// </summary>
    public enum eEquInfSamplerSystemStatus
    {
        系统停止 = 1,
        正在运行 = 2,
        就绪待机 = 3,
        发生故障 = 4
    }
}
