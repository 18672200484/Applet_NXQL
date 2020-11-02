// 此代码由 NhGenerator v1.0.9.0 工具生成。

using System;
using System.Collections;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.CarTransport
{
	/// <summary>
	/// 汽车智能化-设置采样管理
	/// </summary>
	[Serializable]
	[CMCS.DapperDber.Attrs.DapperBind("CmcsTbSetSampler")]
	public class CmcsSetSampler : EntityBase1
	{
		public string MineId { get; set; }

		public string MineName { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public string Sampler { get; set; }
	}
}
