using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CMCS.DapperDber.Attrs;

namespace CMCS.DumblyConcealer.Tasks.Intelogistics.Entities
{
	/// <summary>
	/// 智能物流--检斤修改记录表
	/// </summary>
	[Serializable]
	[Description("智能物流--检斤修改记录表")]
	[CMCS.DapperDber.Attrs.DapperBind("检斤修改记录表")]
	public class WL_TransportUpdateInfo : WL_TransportInfo
	{

	}
}
