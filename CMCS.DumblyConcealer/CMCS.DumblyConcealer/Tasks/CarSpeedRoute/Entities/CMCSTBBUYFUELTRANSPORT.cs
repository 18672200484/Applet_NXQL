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
    [CMCS.DapperDber.Attrs.DapperBind("cmcstboutnettransport")]
    public class CMCSTBBUYFUELTRANSPORT : EntityBase
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
        /// 车辆id
        /// </summary>
        public String AUTOTRUCKID { get; set; }
        /// <summary>
        /// 毛重（吨）
        /// </summary>
        public decimal GROSSWEIGHT { get; set; }
        /// <summary>
        /// 皮重（吨）
        /// </summary>
        public decimal TAREWEIGHT { get; set; }
        /// <summary>
        /// 矿发量（吨）
        /// </summary>
        public decimal TICKETWEIGHT { get; set; }
        /// <summary>
        /// 净重（吨）
        /// </summary>
        public decimal SUTTLEWEIGHT { get; set; }
        /// <summary>
        /// 煤种id
        /// </summary>
        public string FUELKINDID { get; set; }
        /// <summary>
        /// 运输单位ID
        /// </summary>
        public string TRANSPORTCOMPANYID { get; set; }
        /// <summary>
        /// 矿点id
        /// </summary>
        public string MINEID { get; set; }
        /// <summary>
        /// 供应商id
        /// </summary>
        public string SUPPLIERID { get; set; }
        /// <summary>
        /// 发车管理表id
        /// </summary>
        public string DEPARTMANAGEDETAILID { get; set; }
        /// <summary>
        /// 车辆实时经度
        /// </summary>
        public decimal LONGITUDE { get; set; }
        /// <summary>
        /// 车辆实时纬度
        /// </summary>
        public decimal LATITUDE { get; set; }
        /// <summary>
        /// 车辆实时速度
        /// </summary>
        public decimal SPEED { get; set; }
        /// <summary>
        /// 当前方向
        /// </summary>
        public string DIRECTION { get; set; }
        /// <summary>
        /// 是否速度异常
        /// </summary>
        public int ISSPEEDERR { get; set; }
        /// <summary>
        /// 是否停车异常
        /// </summary>
        public int ISSTOPERR { get; set; }
        /// <summary>
        /// 是否路线偏离
        /// </summary>
        public int ISDEVIATEEERR { get; set; }
        /// <summary>
        /// 是否车辆保修
        /// </summary>
        public int ISREPAIRERR { get; set; }
        /// <summary>
        /// 定位时间
        /// </summary>
        public DateTime LOCATIONTIME { get; set; }
        /// <summary>
        /// 当前位置
        /// </summary>
        public string CURRENTLOCATION { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public DateTime ARRIVALTIME { get; set; }
        /// <summary>
        /// 流程状态
        /// </summary>
        public string STEPNAME { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public int ISFINISH { get; set; }

        /// <summary>
        /// 发车时间
        /// </summary>
        public DateTime STARTTIME { get; set; }
        /// <summary>
        /// 到厂时间
        /// </summary>
        public DateTime INFACTORYTIME { get; set; }
        /// <summary>
        /// 毛重时间
        /// </summary>
        public DateTime GROSSTIME { get; set; }
        /// <summary>
        /// 皮重时间
        /// </summary>
        public DateTime TARETIME { get; set; }

    }
}
