using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;
using CMCS.Common.Entities.Sys;

namespace CMCS.DumblyConcealer.Tasks.CarJXSampler.Entities
{
    /// <summary>
    /// 汽车机械采样机接口 - 卸样命令表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("EquTbQCJXCYJUnloadCmd")]
    public class EquQCJXCYJUnloadCmd : EntityBase2
    { 
        private string _SampleCode;
        private string _UnLoadType;
        private string _ResultCode;
        private int _DataFlag;
         

        private string samplingId;
        /// <summary>
        /// 采样单Id
        /// </summary>
        public string SamplingId
        {
            get { return samplingId; }
            set { samplingId = value; }
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
        /// 卸样方式
        /// </summary>
        public string UnLoadType
        {
            get { return _UnLoadType; }
            set { _UnLoadType = value; }
        }

        /// <summary>
        /// 采样结果
        /// </summary>
        public string ResultCode
        {
            get { return _ResultCode; }
            set { _ResultCode = value; }
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
