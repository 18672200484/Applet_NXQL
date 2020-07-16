using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.SyncNetData.Entities
{
    /// <summary>
    /// 调运计划
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBTRANSPORTPLAN")]
    public class TransportPlan : EntityBase1
    {
        /// <summary>
        /// 供应商id
        /// </summary>
        public virtual string SupplierId { get; set; }
        /// <summary>
        /// 矿点id
        /// </summary>
        public virtual string MineId { get; set; }
        /// <summary>
        /// 煤种id
        /// </summary>
        public virtual string FuelKindId { get; set; }
        /// <summary>
        /// 调运计划编号
        /// </summary>
        public String PlanNum { get; set; }
        /// <summary>
        /// 计划到厂时间
        /// </summary>
        public DateTime PlanTime { get; set; }
        /// <summary>
        /// 装车时间
        /// </summary>
        public DateTime LoadTime { get; set; }
        /// <summary>
        /// 车数
        /// </summary>
        public int CarCounts { get; set; }
        /// <summary>
        /// 来煤量（吨）
        /// </summary>
        public Decimal CoalQty { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String ReMark { get; set; }
        /// <summary>
        /// 同步标识  0未同步  1已同步
        /// </summary>
        public Int32 SyncFlag { get; set; }
    }
}
