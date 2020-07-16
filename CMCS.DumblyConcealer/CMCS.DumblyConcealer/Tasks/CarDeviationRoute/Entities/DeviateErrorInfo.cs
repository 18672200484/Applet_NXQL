using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarDeviationRoute.Entities
{
    /// <summary>
    /// 距离偏移错误信息
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBDEVIATEERRORINFO")]
    public class DeviateErrorInfo : EntityBase1
    {
        /// <summary>
        /// 运输记录ID
        /// </summary>
        public virtual string TransportRecordId { get; set; }
        /// <summary>
        /// 偏离距离
        /// </summary>
        public decimal Distance { get; set; }
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
