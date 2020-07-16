using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.SyncNetData.Entities
{
    /// <summary>
    /// 发车管理
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBDEPARTMANAGE")]
    public class DepartManage : EntityBase1
    {
        /// <summary>
        /// 调运计划ID
        /// </summary>
        public virtual string TransportPlanId { get; set; }
        /// <summary>
        /// 运输单位id
        /// </summary>
        public string TransportCompanyId { get; set; }
        /// <summary>
        /// 发车时间
        /// </summary>
        public DateTime DepartTime { get; set; }
        /// <summary>
        /// 计划到厂时间
        /// </summary>
        public DateTime PlanTime { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public int IsCheck { get; set; }
        /// <summary>
        /// 异常描述
        /// </summary>
        public String ReMark { get; set; }
        /// <summary>
        /// 流程状态
        /// </summary>
        public String WfName { get; set; }

        /// <summary>
        /// 同步标识 0未同步 1已同步
        /// </summary>
        public Int32 SyncFlag { get; set; }

        /// <summary>
        /// 发车管理详情
        /// </summary>
        [CMCS.DapperDber.Attrs.DapperIgnore]
        public virtual ICollection<DepartManageDetail> CarDetails { get; set; }
    }
}
