using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.CarTransport
{
    /// <summary>
    /// 矿点来煤量
    /// </summary>
    [Serializable]
    [Description("矿点来煤量")]
    [CMCS.DapperDber.Attrs.DapperBind("CmcsTbDayMineFuelqty")]
    public class CmcsDayMineFuelqty : EntityBase1
    {
        /// <summary>
        /// 矿点Id
        /// </summary>
        public string MineId { get; set; }

        /// <summary>
        /// 来煤量
        /// </summary>
        public decimal Fuelqty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
