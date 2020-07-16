using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarStopTime.Entities
{
    /// <summary>
    /// 异常停留数据表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBSTOPERRORINFO")]
    public class StopErrorInfo : EntityBase1
    {
        /// <summary>
        /// 运输记录ID
        /// </summary>
        public virtual string TransportRecordId { get; set; }
        /// <summary>
        /// 停留位置
        /// </summary>
        public String StopPlace { get; set; }
        /// <summary>
        /// 停留地点
        /// </summary>
        public String StopPoint { get; set; }
        /// <summary>
        /// 停留数量(%)
        /// </summary>
        public decimal StopNum { get; set; }
        /// <summary>
        /// 停留时间
        /// </summary>
        public decimal StopTime { get; set; }
        /// <summary>
        /// 异常开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 异常结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 异常描述
        /// </summary>
        public String Remark { get; set; }
    }
}
