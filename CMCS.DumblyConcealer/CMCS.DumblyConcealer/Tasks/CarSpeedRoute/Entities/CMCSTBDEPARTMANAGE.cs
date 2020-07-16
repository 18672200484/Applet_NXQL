using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarSpeedRoute.Entities
{
    /// <summary>
    /// 车速预警
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBDEPARTMANAGE")]
    public class CMCSTBDEPARTMANAGE : EntityBase
    {
        //
        // 摘要:
        //     Is this entity Deleted?
        public virtual int IsDeleted { get; set; }
        //
        // 摘要:
        //     Which user deleted this entity?
        public virtual long? DeleterUserId { get; set; }
        //
        // 摘要:
        //     Deletion time of this entity.
        public virtual DateTime? DeletionTime { get; set; }
        /// <summary>
        /// 路段
        /// </summary>
        public String HIGHWAY { get; set; }
        /// <summary>
        /// 路段名称
        /// </summary>
        public String HIGHWAYNAME { get; set; }
        /// <summary>
        /// 经纬度集合
        /// </summary>
        public String POINTS { get; set; }
        /// <summary>
        /// 车速低于同路段车车速比例(%)
        /// </summary>
        public decimal SPEEDRANGE { get; set; }
        /// <summary>
        /// 最低车速
        /// </summary>
        public decimal MINSPEED { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String REMARK { get; set; }
    }
}
