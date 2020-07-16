using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.SyncNetData.Entities
{
    /// <summary>
    /// 发车管理明细
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBDEPARTMANAGEDETAIL")]
    public class DepartManageDetail : EntityBase1
    {
        /// <summary>
        /// 主表id
        /// </summary>
        public string MainId { get; set; }
        /// <summary>
        /// 车辆id
        /// </summary>
        public string CarId { get; set; }
        /// <summary>
        /// 矿发时间
        /// </summary>
        public DateTime TicketTime { get; set; }
        /// <summary>
        /// 矿发量
        /// </summary>
        public decimal TicketQty { get; set; }
    }
}
