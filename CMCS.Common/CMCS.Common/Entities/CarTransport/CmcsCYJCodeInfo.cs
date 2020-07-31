// 此代码由 NhGenerator v1.0.9.0 工具生成。

using System;
using System.Collections;
using System.ComponentModel;
using CMCS.Common.DAO;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.CarTransport
{
    /// <summary>
    /// 汽车智能化-采样编码记录
    /// </summary>
    [Serializable]
    [Description("采样编码记录")]
    [CMCS.DapperDber.Attrs.DapperBind("CmcsTbCYJCodeInfo")]
    public class CmcsCYJCodeInfo : EntityBase1
    {
        private String _SamplingId;
        /// <summary>
        /// 采样Id
        /// </summary>
        public virtual String SamplingId { get { return _SamplingId; } set { _SamplingId = value; } }

        private String _SampleCode;
        /// <summary>
        /// 采样编码
        /// </summary>
        [Description("采样编码")]
        public virtual String SampleCode { get { return _SampleCode; } set { _SampleCode = value; } }

        private DateTime _ClearTime;
        /// <summary>
        /// 清样时间
        /// </summary>
        public virtual DateTime ClearTime { get { return _ClearTime; } set { _ClearTime = value; } }

        private String _SamplerName;
        /// <summary>
        /// 采样机名称
        /// </summary>
        public virtual String SamplerName { get { return _SamplerName; } set { _SamplerName = value; } }

        private string _BarrelCode;
        /// <summary>
        /// 桶号
        /// </summary>
        [Description("桶号")]
        public virtual string BarrelCode { get { return _BarrelCode; } set { _BarrelCode = value; } }

        private int _SampleCount;
        /// <summary>
        /// 子样数
        /// </summary>
        [Description("子样数")]
        public virtual int SampleCount { get { return _SampleCount; } set { _SampleCount = value; } }

        private int _CarCount;
        /// <summary>
        /// 车数
        /// </summary>
        [Description("车数")]
        public virtual int CarCount { get { return _CarCount; } set { _CarCount = value; } }

        private double _SampleWeight;
        /// <summary>
        /// 样重
        /// </summary>
        public virtual double SampleWeight { get { return _SampleWeight; } set { _SampleWeight = value; } }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 分样序号
        /// </summary>
        public int Splitorder { get; set; }
    }
}
