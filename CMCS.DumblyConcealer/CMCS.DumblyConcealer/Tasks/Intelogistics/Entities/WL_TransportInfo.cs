using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CMCS.DapperDber.Attrs;

namespace CMCS.DumblyConcealer.Tasks.Intelogistics.Entities
{
    /// <summary>
    /// 智能物流--检斤表
    /// </summary>
    [Serializable]
    [Description("智能物流--检斤表")]
    [CMCS.DapperDber.Attrs.DapperBind("检斤表")]
    public class WL_TransportInfo
    {
        [DapperAutoPrimaryKeyAttribute]
        public Int32 编号 { get; set; }
        public string 物流矿发编号 { get; set; }
        public string 所属单位名称 { get; set; }
        public string 车牌号 { get; set; }
        public string 门禁编号 { get; set; }
        public string 化验表编号 { get; set; }
        public DateTime 重车时间 { get; set; }
        public DateTime 轻车时间 { get; set; }
        public string 物料编号 { get; set; }
        public string 物料名称 { get; set; }
        public decimal 毛重 { get; set; }
        public decimal 皮重 { get; set; }
        public decimal 净重 { get; set; }
        public decimal 矿发毛重 { get; set; }
        public decimal 矿发皮重 { get; set; }
        public decimal 矿发净重 { get; set; }
        public decimal 扣吨 { get; set; }
        public string 检斤员名字 { get; set; }
        public string 重车衡号 { get; set; }
        public string 轻车衡号 { get; set; }
        public string 煤场名称 { get; set; }
        public string 发货方编号 { get; set; }
        public string 发货方名称 { get; set; }
        public string 承运商编号 { get; set; }
        public string 承运商名称 { get; set; }
        public string 采样表编号 { get; set; }
        public DateTime 采样时间 { get; set; }
        public string 采样人 { get; set; }
        public string 检斤备注 { get; set; }
        public DateTime 创建时间 { get; set; }
        public Int32 同步完成 { get; set; }
        public DateTime 同步完成时间 { get; set; }
    }
}
