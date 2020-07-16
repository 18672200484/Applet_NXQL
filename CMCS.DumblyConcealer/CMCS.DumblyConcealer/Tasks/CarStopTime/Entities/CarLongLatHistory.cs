using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarStopTime.Entities
{
    /// <summary>
    /// 车辆经纬度历史记录表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("CMCSTBLONGITUDEANDLATITUDE")]
    public class CarLongLatHistory : EntityBase1
    {
        /// <summary>
        /// 运输记录ID
        /// </summary>
        public virtual string TransportRecordId { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
    }
}
