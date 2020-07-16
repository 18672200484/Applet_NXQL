using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarDeviationRoute.Entities
{
    /// <summary>
    /// 距离偏移预警
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBDEVIATEWARNING")]
    public class DeviateWarning : EntityBase1
    {
        /// <summary>
        /// 偏离距离
        /// </summary>
        public virtual decimal Distance { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
