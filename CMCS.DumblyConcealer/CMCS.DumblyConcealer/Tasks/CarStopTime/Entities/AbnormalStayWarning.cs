using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarStopTime.Entities
{
    /// <summary>
    /// 异常停留设置
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBSTOPWARNING")]
    public class AbnormalStayWarning : EntityBase1
    {
        /// <summary>
        /// 停车数量
        /// </summary>
        /// 
        public virtual decimal StopNumber { get; set; }

        /// <summary>
        /// 停车时间（分钟）
        /// </summary>
        /// 
        public virtual decimal StopTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// 
        public virtual string Remark { get; set; }
    }
}
