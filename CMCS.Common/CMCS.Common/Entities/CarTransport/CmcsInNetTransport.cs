using System;
using System.Collections;
using System.ComponentModel;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.CarTransport
{
    /// <summary>
    /// 汽车智能化-外网运输记录
    /// </summary>
    [Serializable]
    [Description("外网运输记录")]
    [CMCS.DapperDber.Attrs.DapperBind("CmcsTbInNetTransport")]
    public class CmcsInNetTransport : EntityBase1
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNumber { get; set; }
        /// <summary>
        /// 关联：车辆管理  
        /// </summary>
        public string AutoTruckId { get; set; }
        /// <summary>
        /// 进厂流水号  
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 矿发量（吨） 
        /// </summary>
        public decimal TicketWeight { get; set; }
        /// <summary>
        /// 皮重（吨）
        /// </summary>
        public decimal? TareWeight { get; set; }
        /// <summary>
        /// 毛重（吨）
        /// </summary>
        public decimal? GrossWeight { get; set; }
        /// <summary>
        /// 货物净重（吨）
        /// </summary>
        public decimal? SuttleWeight { get; set; }
        /// <summary>
        /// 发车时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 到厂时间
        /// </summary>
        public DateTime? Infactorytime { get; set; }
        /// <summary>
        /// 毛重时间
        /// </summary>
        public DateTime? GrossTime { get; set; }
        /// <summary>
        /// 皮重时间
        /// </summary>
        public DateTime? TareTime { get; set; }
        /// <summary>
        /// 煤种名称
        /// </summary>
        public string FuelKindName { get; set; }
        /// <summary>
        /// 煤种Id
        /// </summary>
        public string FuelKindId { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 供应商Id
        /// </summary>
        public string SupplierId { get; set; }
        /// <summary>
        /// 运输单位ID
        /// </summary>
        public virtual string TransportCompanyId { get; set; }
        /// <summary>
        /// 运输单位ID
        /// </summary>
        public virtual string MineId { get; set; }
        /// <summary>
        /// 皮重地点
        /// </summary>
        public string TarePlace { get; set; }
        /// <summary>
        /// 毛重地点
        /// </summary>
        public string GrossPlace { get; set; }
        /// <summary>
        /// 进厂地点  
        /// </summary>
        public string InfactoryPlace { get; set; }
        /// <summary>
        /// 出厂时间
        /// </summary>
        public DateTime? OutfactoryTime { get; set; }
        /// <summary>
        /// 出厂地点  
        /// </summary>
        public string OutfactoryPlace { get; set; }
        /// <summary>
        /// 发车管理详情表id  
        /// </summary>
        public string DepartManageDetailId { get; set; }
        /// <summary>
        /// 经度  
        /// </summary>
        public Decimal Longitude { get; set; }
        /// <summary>
        /// 纬度  
        /// </summary>
        public Decimal Latitude { get; set; }
        /// <summary>
        /// 速度  
        /// </summary>
        public Decimal Speed { get; set; }
        /// <summary>
        /// 方向  
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 定位时间
        /// </summary>
        public DateTime? LocationTime { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string CurrentLocation { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public DateTime? ArrivalTime { get; set; }
        /// <summary>
        /// 是否速度异常  
        /// </summary>
        public int IsSpeedErr { get; set; }
        /// <summary>
        /// 是否停车异常
        /// </summary>
        public int IsStopErr { get; set; }
        /// <summary>
        /// 是否路线偏离  
        /// </summary>
        public int IsDeviateeErr { get; set; }
        /// <summary>
        /// 是否车辆保修  
        /// </summary>
        public int IsRepairErr { get; set; }
        /// <summary>
        /// 流程状态(矿发，在途，入厂、重车、采样、轻车、出厂）
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// 是否完结  是否完结0：否 1：是 默认0
        /// </summary>
        public int IsFinish { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 数据同步标识 0未同步 1已同步
        /// </summary>
        public int SyncFlag { get; set; }
    }
}
