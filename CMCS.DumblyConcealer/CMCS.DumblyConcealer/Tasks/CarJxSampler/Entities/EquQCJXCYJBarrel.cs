using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarJXSampler.Entities
{
    /// <summary>
    /// 汽车机械采样机接口 - 实时集样罐表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("EquTbQCJXCYJBarrel")]
    public class EquQCJXCYJBarrel : EntityBase2
    {
        private string _BarrelNumber;
        private string _InFactoryBatchId;
        private string _SampleCode;
        private int _SampleCount;
        private int _IsCurrent;
        private string _BarrelStatus;
        private DateTime _UpdateTime;
        private int _DataFlag; 

        /// <summary>
        /// 罐号
        /// </summary>
        public string BarrelNumber
        {
            get { return _BarrelNumber; }
            set { _BarrelNumber = value; }
        }

        /// <summary>
        /// 批次Id
        /// </summary>
        public string InFactoryBatchId
        {
            get { return _InFactoryBatchId; }
            set { _InFactoryBatchId = value; }
        }

        /// <summary>
        /// 采样码
        /// </summary>
        public string SampleCode
        {
            get { return _SampleCode; }
            set { _SampleCode = value; }
        }

        /// <summary>
        /// 子样数
        /// </summary>
        public int SampleCount
        {
            get { return _SampleCount; }
            set { _SampleCount = value; }
        }

        /// <summary>
        /// 是当前进料罐
        /// </summary>
        public int IsCurrent
        {
            get { return _IsCurrent; }
            set { _IsCurrent = value; }
        }

        /// <summary>
        /// 桶满状态
        /// </summary>
        public string BarrelStatus
        {
            get { return _BarrelStatus; }
            set { _BarrelStatus = value; }
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

        private string _BarrelType;
        /// <summary>
        /// 样罐类型
        /// </summary>
        public string BarrelType
        {
            get { return _BarrelType; }
            set { _BarrelType = value; }
        }
    }
}
