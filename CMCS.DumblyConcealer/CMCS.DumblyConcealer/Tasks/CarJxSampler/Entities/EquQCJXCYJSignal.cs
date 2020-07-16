using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarJXSampler.Entities
{
    /// <summary>
    /// 汽车机械采样机接口 - 实时信号表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("EquTbQCJXCYJSignal")]
    public class EquQCJXCYJSignal : EntityBase2
    {
        private string _TagName;
        private string _TagValue;
        private DateTime _UpdateTime;
        private int _DataFlag;

        /// <summary>
        /// 信号名
        /// </summary>
        public string TagName
        {
            get { return _TagName; }
            set { _TagName = value; }
        }

        /// <summary>
        /// 信号值
        /// </summary>
        public string TagValue
        {
            get { return _TagValue; }
            set { _TagValue = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; }
        }

        /// <summary>
        /// 标识符
        /// </summary>
        public int DataFlag
        {
            get { return _DataFlag; }
            set { _DataFlag = value; }
        }
    }
}
