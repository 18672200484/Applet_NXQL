using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarRepairInfo.Entities
{
    /// <summary>
    /// 车辆报修表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBCARREPAIRINFO")]
    public class CarRepair:EntityBase1
    {
         /// <summary>
        /// 车辆id
        /// </summary>
        /// 
        public virtual string CarId { get; set; }

        /// <summary>
        /// 报修时间
        /// </summary>
        /// 
        public virtual DateTime RepairTime { get; set; }

        /// <summary>
        /// 报修人
        /// </summary>
        /// 
        public virtual string RepairPle { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        /// 
        public virtual string CheckPle { get; set; }

        /// <summary>
        /// 报修原因
        /// </summary>
        /// 
        public virtual string RepairReason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// 
        public virtual string Remark { get; set; }

        /// <summary>
        /// 修理状态（0：未修理  1：已修理）
        /// </summary>
        /// 
        public virtual Int32 RepairStatus { get; set; }
    }
}
