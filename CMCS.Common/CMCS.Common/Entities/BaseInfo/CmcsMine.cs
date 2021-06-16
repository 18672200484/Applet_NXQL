using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.BaseInfo
{
	/// <summary>
	/// 基础信息-矿点
	/// </summary>
	[CMCS.DapperDber.Attrs.DapperBind("fultbmine")]
	public class CmcsMine : EntityBase1
	{
		/// <summary>
		/// 编码
		/// </summary>
		public String Code { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		public String Name { get; set; }

		private string _ParentId;
		/// <summary>
		/// 父Id
		/// </summary>
		public string ParentId
		{
			get { return _ParentId; }
			set { _ParentId = value; }
		}

		public int IsStop { get; set; }

		public String ReMark { get; set; }

		public Int32 Sort { get; set; }

		public string DataFrom { get; set; }

		public string Address { get; set; }

		public Int32 SyncFlag { get; set; }

		/// <summary>
		/// 采购类型
		/// </summary>
		public string PurcHaseType { get; set; }

		/// <summary>
		/// 卸煤区域
		/// </summary>
		public string UnloadingArea { get; set; }
	}
}
