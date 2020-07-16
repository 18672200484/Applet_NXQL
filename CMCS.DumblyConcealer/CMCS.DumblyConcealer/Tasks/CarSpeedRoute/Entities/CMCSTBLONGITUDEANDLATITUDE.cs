using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarSpeedRoute.Entities
{
    /// <summary>
    /// 车辆信息
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBLONGITUDEANDLATITUDE")]
    public class CMCSTBLONGITUDEANDLATITUDE : EntityBase
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
        /// 车号
        /// </summary>
        public String TRANSPORTRECORDID { get; set; }
        /// <summary>
        /// 车辆类型，编码中配置
        /// </summary>
        public decimal LONGITUDE { get; set; }
        /// <summary>
        /// 内外部车辆:内部车/外部车
        /// </summary>
        public decimal LATITUDE { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public decimal SPEED { get; set; }
    }
}
