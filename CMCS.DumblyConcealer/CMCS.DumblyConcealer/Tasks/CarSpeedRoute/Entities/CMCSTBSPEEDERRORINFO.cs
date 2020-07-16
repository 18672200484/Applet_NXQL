using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarSpeedRoute.Entities
{
    /// <summary>
    /// 车速异常信息
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBSPEEDERRORINFO")]
    public class CMCSTBSPEEDERRORINFO : EntityBase
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
        /// 运输记录id
        /// </summary>
        public String TRANSPORTRECORDID { get; set; }
        /// <summary>
        /// 路段名称
        /// </summary>
        public String HIGHWAYNAME { get; set; }
        /// <summary>
        /// 异常车速
        /// </summary>
        public decimal SPEED { get; set; }
        /// <summary>
        /// 异常开始时间
        /// </summary>
        public DateTime STARTTIME { get; set; }
        /// <summary>
        /// 异常结束时间
        /// </summary>
        public DateTime ENDTIME { get; set; }
    }
}
