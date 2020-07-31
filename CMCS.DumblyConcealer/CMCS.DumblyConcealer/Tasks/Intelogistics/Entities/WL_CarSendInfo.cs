using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CMCS.DumblyConcealer.Tasks.Intelogistics.Entities
{
    /// <summary>
    /// 智能物流--始发表
    /// </summary>
    [Serializable]
    [Description("智能物流--始发表")]
    [CMCS.DapperDber.Attrs.DapperBind("始发表")]
    public class WL_CarSendInfo
    {
        [DapperDber.Attrs.DapperPrimaryKey]
        public int 编号 { get; set; }
        public string 物流矿发编号 { get; set; }
        public string 所属单位名称 { get; set; }
        public string 供应商名称 { get; set; }
        public string 矿点名称 { get; set; }
        public string 物料名称 { get; set; }
        public string 承运商名称 { get; set; }
        public string 车牌号 { get; set; }
        public string 驾驶员身份证 { get; set; }
        public decimal 矿发净重 { get; set; }
        public string 封签号 { get; set; }
        public string 预留1 { get; set; }
        public string 预留2 { get; set; }
        public string 预留3 { get; set; }
        public string 预留4 { get; set; }
        public string 预留5 { get; set; }
        public DateTime 创建时间 { get; set; }
        public int 同步完成 { get; set; }
        public DateTime 同步完成时间 { get; set; }
    }
}
