using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CMCS.DapperDber.Attrs;

namespace CMCS.DumblyConcealer.Tasks.Intelogistics.Entities
{
	/// <summary>
	/// 智能物流--电厂化验表
	/// </summary>
	[Serializable]
	[Description("智能物流--电厂化验表")]
	[CMCS.DapperDber.Attrs.DapperBind("电厂化验表")]
	public class WL_AssayInfo
	{
		[DapperAutoPrimaryKeyAttribute]
		public Int32 编号 { get; set; }
		public string 化验编号 { get; set; }

		private string _所属单位名称 = "青铝发电";
		public string 所属单位名称
		{
			get { return _所属单位名称; }
			set { _所属单位名称 = value; }
		}

		public DateTime 化验时间 { get; set; }
		public decimal 高位热值 { get; set; }
		public decimal 低位热值 { get; set; }
		public decimal 收到基灰分 { get; set; }
		public decimal 干基灰 { get; set; }
		public decimal 挥发分 { get; set; }
		public decimal 全水 { get; set; }
		public decimal 空干基硫 { get; set; }
		public decimal 空干基灰 { get; set; }
		public decimal 空干基水 { get; set; }
		public decimal 空干基氢 { get; set; }
		public decimal 空干基挥发分 { get; set; }
		public decimal 空干基高位热 { get; set; }
		public decimal 弹筒热 { get; set; }
		public decimal 固定碳 { get; set; }
		public decimal 干基硫 { get; set; }
		public decimal 干燥无灰基氢 { get; set; }
		public decimal 干燥无灰基高位热 { get; set; }
		public decimal 干燥无灰基硫 { get; set; }
		public decimal 收到基挥发份 { get; set; }
		public decimal 收到基氢 { get; set; }
		public decimal 干燥基高位热值 { get; set; }
		public decimal 计价水分 { get; set; }
		public decimal 计价热量 { get; set; }
		public string 化验员 { get; set; }
		public string 化验录入员 { get; set; }
		public string 化验结果 { get; set; }
		public string 化验备注 { get; set; }
		public decimal 煤量 { get; set; }
		public decimal 车数 { get; set; }
		public string 车牌号 { get; set; }
		public DateTime 采样日期 { get; set; }
		public DateTime 创建时间 { get; set; }
		public Int32 同步完成 { get; set; }
		public DateTime 同步完成时间 { get; set; }

	}
}
