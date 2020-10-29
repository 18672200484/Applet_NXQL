using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DumblyConcealer.Enums;
using CMCS.Common.DAO;
using CMCS.Common.DapperDber_etc;
using CMCS.Common;
using CMCS.DumblyConcealer.Tasks.Intelogistics.Entities;
using CMCS.DapperDber.Dbs.MySqlDb;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.BaseInfo;
using System.Data;

namespace CMCS.DumblyConcealer.Tasks.Intelogistics
{
	public class IntelogisticsDAO
	{
		private static IntelogisticsDAO instance;

		public static IntelogisticsDAO GetInstance()
		{
			if (instance == null)
			{
				instance = new IntelogisticsDAO();
			}
			return instance;
		}

		private IntelogisticsDAO()
		{
			string configValue = commonDAO.GetAppletConfigString("数据同步智能物流接口", "接口地址");
			if (!string.IsNullOrWhiteSpace(configValue))
				mysqlDber = new MySqlDapperDber(configValue);
		}

		CommonDAO commonDAO = CommonDAO.GetInstance();
		OracleDapperDber_iEAA SelfDber = Dbers.GetInstance().SelfDber;
		MySqlDapperDber mysqlDber = null;



		/// <summary>
		/// 获取智能物流始发表数据
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		public void GetData(Action<string, eOutputType> output)
		{
			if (mysqlDber == null)
			{
				output("未在【小程序参数配置】模块中添加配置，唯一标识：数据同步智能物流接口，配置名称：接口地址", eOutputType.Error);
				return;
			}

			int intervalValue = 7;
			string configValue = commonDAO.GetAppletConfigString("数据同步智能物流接口", "获取智能物流数据时间间隔（天）");
			if (!string.IsNullOrWhiteSpace(configValue))
				Int32.TryParse(configValue, out intervalValue);
			DateTime startTime = DateTime.Now.AddDays(-intervalValue);

			List<WL_CarSendInfo> sendEntities = mysqlDber.Entities<WL_CarSendInfo>(string.Format(" where (同步完成=0 or 同步完成 is NULL) and 创建时间>='{0}'", startTime));
			SaveCarSendData(sendEntities, output);

		}

		/// <summary>
		/// 保存矿发数据
		/// </summary>
		/// <param name="sendEntities"></param>
		/// <param name="output"></param>
		public void SaveCarSendData(List<WL_CarSendInfo> sendEntities, Action<string, eOutputType> output)
		{
			foreach (var item in sendEntities)
			{
				if (string.IsNullOrWhiteSpace(item.物流矿发编号)) continue;

				var entity = SelfDber.Entity<CmcsInNetTransport>(" where SerialNumber=:SerialNumber", new { SerialNumber = item.物流矿发编号 });
				if (entity == null)
				{
					#region 新增
					entity = new CmcsInNetTransport();
					entity.SerialNumber = item.物流矿发编号;
					entity.FactoryName = item.所属单位名称;

					var supplier = SelfDber.Entity<CmcsSupplier>(" where Name=:Name", new { Name = item.供应商名称 });
					if (supplier != null)
						entity.SupplierId = supplier.Id;
					entity.SupplierName = item.供应商名称;

					var mine = SelfDber.Entity<CmcsMine>(" where Name=:Name", new { Name = item.矿点名称 });
					if (mine != null)
						entity.MineId = mine.Id;
					entity.MineName = item.矿点名称;

					var fuelkind = SelfDber.Entity<CmcsFuelKind>(" where Name=:Name", new { Name = item.品种名称 });
					if (fuelkind != null)
						entity.FuelKindId = fuelkind.Id;
					entity.FuelKindName = item.品种名称;

					var transCompany = SelfDber.Entity<CmcsTransportCompany>(" where Name=:Name", new { Name = item.承运商名称 });
					if (transCompany != null)
						entity.TransportCompanyId = transCompany.Id;
					entity.TransportCompanyName = item.承运商名称;

					var autoTruck = SelfDber.Entity<CmcsAutotruck>(" where CarNumber=:CarNumber", new { CarNumber = item.车牌号 });
					if (autoTruck != null)
						entity.AutoTruckId = autoTruck.Id;
					entity.CarNumber = item.车牌号;

					entity.IDCard = item.驾驶员身份证;
					entity.TicketWeight = item.矿发净重;
					entity.SealNumber = item.封签号;
					entity.CreationTime = item.创建时间;
					entity.StepName = "在途";
					entity.IsFinish = 0;
					entity.SyncFlag = 0;
					entity.Remark = "智能物流同步";

					if (SelfDber.Insert<CmcsInNetTransport>(entity) > 0)
					{
						output("始发表数据获取成功，操作：新增，数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(item), eOutputType.Normal);
						item.同步完成 = 1;
						item.同步完成时间 = DateTime.Now;
						//if (mysqlDber.Update<WL_CarSendInfo>(item) > 0)
						if (mysqlDber.Execute(string.Format("update 始发表 set 同步完成=1,同步完成时间='{0}' where 编号={1}", DateTime.Now, item.编号)) > 0)
							output("始发表数据获取后回写成功，数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(item), eOutputType.Normal);
					}

					#endregion
				}
				else
				{
					#region 修改
					entity.FactoryName = item.所属单位名称;

					var supplier = SelfDber.Entity<CmcsSupplier>(" where Name=:Name", new { Name = item.供应商名称 });
					if (supplier != null)
						entity.SupplierId = supplier.Id;
					entity.SupplierName = item.供应商名称;

					var mine = SelfDber.Entity<CmcsMine>(" where Name=:Name", new { Name = item.矿点名称 });
					if (mine != null)
						entity.MineId = mine.Id;
					entity.MineName = item.矿点名称;

					var fuelkind = SelfDber.Entity<CmcsFuelKind>(" where Name=:Name", new { Name = item.品种名称 });
					if (fuelkind != null)
						entity.FuelKindId = fuelkind.Id;
					entity.FuelKindName = item.品种名称;

					var transCompany = SelfDber.Entity<CmcsTransportCompany>(" where Name=:Name", new { Name = item.承运商名称 });
					if (transCompany != null)
						entity.TransportCompanyId = transCompany.Id;
					entity.TransportCompanyName = item.承运商名称;

					var autoTruck = SelfDber.Entity<CmcsAutotruck>(" where CarNumber=:CarNumber", new { CarNumber = item.车牌号 });
					if (autoTruck != null)
						entity.AutoTruckId = autoTruck.Id;
					entity.CarNumber = item.车牌号;

					entity.IDCard = item.驾驶员身份证;
					entity.TicketWeight = item.矿发净重;
					entity.SealNumber = item.封签号;
					entity.SyncFlag = 0;

					if (SelfDber.Update<CmcsInNetTransport>(entity) > 0)
					{
						output("始发表数据获取成功，操作：更新，数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(item), eOutputType.Normal);
						item.同步完成 = 1;
						item.同步完成时间 = DateTime.Now;
						//if (mysqlDber.Update<WL_CarSendInfo>(item) > 0)
						if (mysqlDber.Execute(string.Format("update 始发表 set 同步完成=1,同步完成时间='{0}' where 编号={1}", DateTime.Now, item.编号)) > 0)
							output("始发表数据获取后回写成功，数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(item), eOutputType.Normal);
					}
					#endregion
				}
			}
		}

		/// <summary>
		/// 推送运输记录给智能物流
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		public void SendTransData(Action<string, eOutputType> output)
		{
			if (mysqlDber == null)
			{
				output("未在【小程序参数配置】模块中添加配置，唯一标识：数据同步智能物流接口，配置名称：接口地址", eOutputType.Error);
				return;
			}

			int intervalValue = 7;
			string configValue = commonDAO.GetAppletConfigString("数据同步智能物流接口", "数据上传时间间隔（天）");
			if (!string.IsNullOrWhiteSpace(configValue))
				Int32.TryParse(configValue, out intervalValue);
			DateTime startTime = DateTime.Now.AddDays(-intervalValue);

			DataTable dt = SelfDber.ExecuteDataTable(string.Format("select * from View_ZNWL_Transport where factArriveDate>=to_date('{0}','yyyy-MM-dd hh24:mi:ss')", startTime.ToString("yyyy-MM-dd HH:mm:ss")));
			if (dt == null || dt.Rows.Count <= 0) return;

			foreach (DataRow item in dt.Rows)
			{
				var entity = mysqlDber.Entity<WL_TransportInfo>(" where 检斤编号=@SerialNumber", new { SerialNumber = item["检斤编号"].ToString() });
				if (entity == null)
				{
					#region 新增
					entity = new WL_TransportInfo();
					entity.物流矿发编号 = item["物流矿发编号"].ToString();
					entity.检斤编号 = item["检斤编号"].ToString();
					entity.所属单位名称 = item["所属单位名称"].ToString();
					entity.车牌号 = item["车牌号"].ToString();
					entity.门禁编号 = item["门禁编号"].ToString();
					entity.化验编号 = item["化验表编号"].ToString();

					DateTime grossTime = DateTime.Now;
					if (string.IsNullOrWhiteSpace(item["重车时间"].ToString()) || !DateTime.TryParse(item["重车时间"].ToString(), out grossTime))
					{
						output(string.Format("物流矿发编号：{0}，车牌号：{1}，毛重时间格式不正确。", entity.物流矿发编号, entity.车牌号), eOutputType.Error);
						continue;
					}
					entity.重车时间 = DateTime.Parse(item["重车时间"].ToString());

					DateTime skinTime = DateTime.Now;
					if (string.IsNullOrWhiteSpace(item["轻车时间"].ToString()) || !DateTime.TryParse(item["轻车时间"].ToString(), out skinTime))
					{
						output(string.Format("物流矿发编号：{0}，车牌号：{1}，皮重时间格式不正确。", entity.物流矿发编号, entity.车牌号), eOutputType.Error);
						continue;
					}
					entity.轻车时间 = DateTime.Parse(item["轻车时间"].ToString());
					entity.物料编号 = item["煤种编号"].ToString();
					entity.物料名称 = item["煤种名称"].ToString();

					try
					{
						entity.毛重 = item["毛重"] != DBNull.Value ? Decimal.Parse(item["毛重"].ToString()) : 0;
						entity.皮重 = item["皮重"] != DBNull.Value ? Decimal.Parse(item["皮重"].ToString()) : 0;
						entity.净重 = item["净重"] != DBNull.Value ? Decimal.Parse(item["净重"].ToString()) : 0;
						entity.矿发毛重 = item["矿发毛重"] != DBNull.Value ? Decimal.Parse(item["矿发毛重"].ToString()) : 0;
						entity.矿发皮重 = item["矿发皮重"] != DBNull.Value ? Decimal.Parse(item["矿发皮重"].ToString()) : 0;
						entity.矿发净重 = item["矿发净重"] != DBNull.Value ? Decimal.Parse(item["矿发净重"].ToString()) : 0;
						entity.扣吨 = item["扣吨"] != DBNull.Value ? Decimal.Parse(item["扣吨"].ToString()) : 0;
					}
					catch
					{
						output(string.Format("物流矿发编号：{0}，车牌号：{1}，重量信息存在格式不正确，转换失败。", entity.物流矿发编号, entity.车牌号), eOutputType.Error);
						continue;
					}



					entity.检斤员名字 = item["检斤员名字"].ToString();
					entity.重车衡号 = item["重车衡号"].ToString();
					entity.轻车衡号 = item["轻车衡号"].ToString();
					entity.煤场名称 = item["煤场名称"].ToString();
					entity.发货方编号 = item["发货方编号"].ToString();
					entity.发货方名称 = item["发货方名称"].ToString();
					entity.承运商编号 = item["承运商编号"].ToString();
					entity.承运商名称 = item["承运商名称"].ToString();
					entity.矿点名称 = item["矿点名称"].ToString();
					entity.采样表编号 = item["采样表编号"].ToString();

					DateTime samplingTime = DateTime.Now;
					if (string.IsNullOrWhiteSpace(item["采样时间"].ToString()) || !DateTime.TryParse(item["采样时间"].ToString(), out skinTime))
					{
						output(string.Format("物流矿发编号：{0}，车牌号：{1}，采样时间格式不正确。", entity.物流矿发编号, entity.车牌号), eOutputType.Error);
						continue;
					}
					entity.采样时间 = DateTime.Parse(item["采样时间"].ToString());
					entity.采样人 = item["采样人"].ToString();
					entity.检斤备注 = item["检斤备注"].ToString();
					entity.创建时间 = DateTime.Now;
					entity.同步完成 = 0;
					entity.同步完成时间 = DateTime.Now;

					if (mysqlDber.Insert<WL_TransportInfo>(entity) > 0)
					{
						output("推送运输记录给智能物流成功，操作：新增，数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(entity), eOutputType.Normal);
					}

					#endregion
				}
				else
				{
					#region 修改
					entity.物流矿发编号 = item["物流矿发编号"].ToString();
					entity.检斤编号 = item["检斤编号"].ToString();
					entity.所属单位名称 = item["所属单位名称"].ToString();
					entity.车牌号 = item["车牌号"].ToString();
					entity.门禁编号 = item["门禁编号"].ToString();
					entity.化验编号 = item["化验表编号"].ToString();

					DateTime grossTime = DateTime.Now;
					if (string.IsNullOrWhiteSpace(item["重车时间"].ToString()) || !DateTime.TryParse(item["重车时间"].ToString(), out grossTime))
					{
						output(string.Format("物流矿发编号：{0}，车牌号：{1}，毛重时间格式不正确。", entity.物流矿发编号, entity.车牌号), eOutputType.Error);
						continue;
					}
					entity.重车时间 = DateTime.Parse(item["重车时间"].ToString());

					DateTime skinTime = DateTime.Now;
					if (string.IsNullOrWhiteSpace(item["轻车时间"].ToString()) || !DateTime.TryParse(item["轻车时间"].ToString(), out skinTime))
					{
						output(string.Format("物流矿发编号：{0}，车牌号：{1}，皮重时间格式不正确。", entity.物流矿发编号, entity.车牌号), eOutputType.Error);
						continue;
					}
					entity.轻车时间 = DateTime.Parse(item["轻车时间"].ToString());
					entity.物料编号 = item["煤种编号"].ToString();
					entity.物料名称 = item["煤种名称"].ToString();

					try
					{
						entity.毛重 = item["毛重"] != DBNull.Value ? Decimal.Parse(item["毛重"].ToString()) : 0;
						entity.皮重 = item["皮重"] != DBNull.Value ? Decimal.Parse(item["皮重"].ToString()) : 0;
						entity.净重 = item["净重"] != DBNull.Value ? Decimal.Parse(item["净重"].ToString()) : 0;
						entity.矿发毛重 = item["矿发毛重"] != DBNull.Value ? Decimal.Parse(item["矿发毛重"].ToString()) : 0;
						entity.矿发皮重 = item["矿发皮重"] != DBNull.Value ? Decimal.Parse(item["矿发皮重"].ToString()) : 0;
						entity.矿发净重 = item["矿发净重"] != DBNull.Value ? Decimal.Parse(item["矿发净重"].ToString()) : 0;
						entity.扣吨 = item["扣吨"] != DBNull.Value ? Decimal.Parse(item["扣吨"].ToString()) : 0;
					}
					catch
					{
						output(string.Format("物流矿发编号：{0}，车牌号：{1}，重量信息存在格式不正确，转换失败。", entity.物流矿发编号, entity.车牌号), eOutputType.Error);
						continue;
					}

					entity.检斤员名字 = item["检斤员名字"].ToString();
					entity.重车衡号 = item["重车衡号"].ToString();
					entity.轻车衡号 = item["轻车衡号"].ToString();
					entity.煤场名称 = item["煤场名称"].ToString();
					entity.发货方编号 = item["发货方编号"].ToString();
					entity.发货方名称 = item["发货方名称"].ToString();
					entity.承运商编号 = item["承运商编号"].ToString();
					entity.承运商名称 = item["承运商名称"].ToString();
					entity.采样表编号 = item["采样表编号"].ToString();

					DateTime samplingTime = DateTime.Now;
					if (string.IsNullOrWhiteSpace(item["采样时间"].ToString()) || !DateTime.TryParse(item["采样时间"].ToString(), out skinTime))
					{
						output(string.Format("物流矿发编号：{0}，车牌号：{1}，采样时间格式不正确。", entity.物流矿发编号, entity.车牌号), eOutputType.Error);
						continue;
					}
					entity.采样时间 = DateTime.Parse(item["采样时间"].ToString());
					entity.采样人 = item["采样人"].ToString();
					entity.检斤备注 = item["检斤备注"].ToString();
					//entity.同步完成 = 0;
					//entity.同步完成时间 = DateTime.Now;

					if (mysqlDber.Update<WL_TransportInfo>(entity) > 0)
					{
						output("推送运输记录给智能物流成功，操作：更新，数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(entity), eOutputType.Normal);
					}
					#endregion
				}
			}

		}

		/// <summary>
		/// 推送化验信息至智能物流
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		public void SendAssayData(Action<string, eOutputType> output)
		{
			if (mysqlDber == null)
			{
				output("未在【小程序参数配置】模块中添加配置，唯一标识：数据同步智能物流接口，配置名称：接口地址", eOutputType.Error);
				return;
			}

			int intervalValue = 7;
			string configValue = commonDAO.GetAppletConfigString("数据同步智能物流接口", "数据上传时间间隔（天）");
			if (!string.IsNullOrWhiteSpace(configValue))
				Int32.TryParse(configValue, out intervalValue);
			DateTime startTime = DateTime.Now.AddDays(-intervalValue);

			DataTable dt = SelfDber.ExecuteDataTable(string.Format("select * from View_ZNWL_AssayInfo where factArriveDate>=to_date('{0}','yyyy-MM-dd hh24:mi:ss')", startTime.ToString("yyyy-MM-dd HH:mm:ss")));
			if (dt == null || dt.Rows.Count <= 0) return;

			foreach (DataRow item in dt.Rows)
			{
				var entity = mysqlDber.Entity<WL_AssayInfo>(" where 化验编号=@AssayCode and 车牌号=@CarNumber", new { AssayCode = item["化验编号"].ToString(), CarNumber = item["车牌号"].ToString() });
				if (entity == null)
				{
					#region 新增
					entity = new WL_AssayInfo();
					entity.化验编号 = item["化验编号"].ToString();

					DateTime assayTime = DateTime.Now;
					if (string.IsNullOrWhiteSpace(item["化验时间"].ToString()) || !DateTime.TryParse(item["化验时间"].ToString(), out assayTime))
					{
						output(string.Format("化验编号：{0}，化验时间格式不正确。", entity.化验编号), eOutputType.Error);
						continue;
					}
					entity.化验时间 = DateTime.Parse(item["化验时间"].ToString());

					try
					{
						entity.高位热值 = item["高位热值"] != DBNull.Value ? Decimal.Parse(item["高位热值"].ToString()) : 0;
						entity.低位热值 = item["低位热值"] != DBNull.Value ? Decimal.Parse(item["低位热值"].ToString()) : 0;
						entity.收到基灰分 = item["收到基灰分"] != DBNull.Value ? Decimal.Parse(item["收到基灰分"].ToString()) : 0;
						entity.干基灰 = item["干基灰"] != DBNull.Value ? Decimal.Parse(item["干基灰"].ToString()) : 0;
						entity.挥发分 = item["挥发分"] != DBNull.Value ? Decimal.Parse(item["挥发分"].ToString()) : 0;
						entity.全水 = item["全水"] != DBNull.Value ? Decimal.Parse(item["全水"].ToString()) : 0;
						entity.空干基硫 = item["空干基硫"] != DBNull.Value ? Decimal.Parse(item["空干基硫"].ToString()) : 0;
						entity.空干基灰 = item["空干基灰"] != DBNull.Value ? Decimal.Parse(item["空干基灰"].ToString()) : 0;
						entity.空干基水 = item["空干基水"] != DBNull.Value ? Decimal.Parse(item["空干基水"].ToString()) : 0;
						entity.空干基氢 = item["空干基氢"] != DBNull.Value ? Decimal.Parse(item["空干基氢"].ToString()) : 0;
						entity.空干基挥发分 = item["空干基挥发分"] != DBNull.Value ? Decimal.Parse(item["空干基挥发分"].ToString()) : 0;
						entity.空干基高位热 = item["空干基高位热"] != DBNull.Value ? Decimal.Parse(item["空干基高位热"].ToString()) : 0;
						entity.弹筒热 = item["弹筒热"] != DBNull.Value ? Decimal.Parse(item["弹筒热"].ToString()) : 0;
						entity.固定碳 = item["固定碳"] != DBNull.Value ? Decimal.Parse(item["固定碳"].ToString()) : 0;
						entity.干基硫 = item["干基硫"] != DBNull.Value ? Decimal.Parse(item["干基硫"].ToString()) : 0;
						entity.干燥无灰基氢 = item["干燥无灰基氢"] != DBNull.Value ? Decimal.Parse(item["干燥无灰基氢"].ToString()) : 0;
						entity.干燥无灰基高位热 = item["干燥无灰基高位热"] != DBNull.Value ? Decimal.Parse(item["干燥无灰基高位热"].ToString()) : 0;
						entity.干燥无灰基硫 = item["干燥无灰基硫"] != DBNull.Value ? Decimal.Parse(item["干燥无灰基硫"].ToString()) : 0;
						entity.收到基挥发份 = item["收到基挥发份"] != DBNull.Value ? Decimal.Parse(item["收到基挥发份"].ToString()) : 0;
						entity.收到基氢 = item["收到基氢"] != DBNull.Value ? Decimal.Parse(item["收到基氢"].ToString()) : 0;
						entity.干燥基高位热值 = item["干燥基高位热值"] != DBNull.Value ? Decimal.Parse(item["干燥基高位热值"].ToString()) : 0;
						entity.计价水分 = item["计价水分"] != DBNull.Value ? Decimal.Parse(item["计价水分"].ToString()) : 0;
						entity.计价热量 = item["计价热量"] != DBNull.Value ? Decimal.Parse(item["计价热量"].ToString()) : 0;
					}
					catch
					{
						output(string.Format("化验编号：{0}，煤质存在数据格式不正确。", entity.化验编号), eOutputType.Error);
						continue;
					}



					entity.化验员 = item["化验员"].ToString();
					entity.化验录入员 = item["化验录入员"].ToString();
					entity.化验结果 = item["化验结果"].ToString();
					entity.化验备注 = item["化验备注"].ToString();

					try
					{
						entity.煤量 = Decimal.Parse(item["煤量"].ToString());
						entity.车数 = Decimal.Parse(item["车数"].ToString());
					}
					catch
					{
						output(string.Format("化验编号：{0}，煤量/车数数据格式不正确。", entity.化验编号), eOutputType.Error);
						continue;
					}


					entity.车牌号 = item["车牌号"].ToString();

					DateTime samplingTime = DateTime.Now;
					if (string.IsNullOrWhiteSpace(item["采样日期"].ToString()) || !DateTime.TryParse(item["采样日期"].ToString(), out samplingTime))
					{
						output(string.Format("化验编号：{0}，采样日期格式不正确。", entity.化验编号), eOutputType.Error);
						continue;
					}
					entity.采样日期 = DateTime.Parse(item["采样日期"].ToString());
					entity.创建时间 = DateTime.Now;
					entity.同步完成 = 0;
					entity.同步完成时间 = DateTime.Now;


					if (mysqlDber.Insert<WL_AssayInfo>(entity) > 0)
					{
						output("推送化验信息至智能物流成功，操作：新增，数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(entity), eOutputType.Normal);
					}

					#endregion
				}
				else
				{
					#region 修改
					DateTime assayTime = DateTime.Now;
					if (string.IsNullOrWhiteSpace(item["化验时间"].ToString()) || !DateTime.TryParse(item["化验时间"].ToString(), out assayTime))
					{
						output(string.Format("化验编号：{0}，化验时间格式不正确。", entity.化验编号), eOutputType.Error);
						continue;
					}
					entity.化验时间 = DateTime.Parse(item["化验时间"].ToString());

					try
					{
						entity.高位热值 = item["高位热值"] != DBNull.Value ? Decimal.Parse(item["高位热值"].ToString()) : 0;
						entity.低位热值 = item["低位热值"] != DBNull.Value ? Decimal.Parse(item["低位热值"].ToString()) : 0;
						entity.收到基灰分 = item["收到基灰分"] != DBNull.Value ? Decimal.Parse(item["收到基灰分"].ToString()) : 0;
						entity.干基灰 = item["干基灰"] != DBNull.Value ? Decimal.Parse(item["干基灰"].ToString()) : 0;
						entity.挥发分 = item["挥发分"] != DBNull.Value ? Decimal.Parse(item["挥发分"].ToString()) : 0;
						entity.全水 = item["全水"] != DBNull.Value ? Decimal.Parse(item["全水"].ToString()) : 0;
						entity.空干基硫 = item["空干基硫"] != DBNull.Value ? Decimal.Parse(item["空干基硫"].ToString()) : 0;
						entity.空干基灰 = item["空干基灰"] != DBNull.Value ? Decimal.Parse(item["空干基灰"].ToString()) : 0;
						entity.空干基水 = item["空干基水"] != DBNull.Value ? Decimal.Parse(item["空干基水"].ToString()) : 0;
						entity.空干基氢 = item["空干基氢"] != DBNull.Value ? Decimal.Parse(item["空干基氢"].ToString()) : 0;
						entity.空干基挥发分 = item["空干基挥发分"] != DBNull.Value ? Decimal.Parse(item["空干基挥发分"].ToString()) : 0;
						entity.空干基高位热 = item["空干基高位热"] != DBNull.Value ? Decimal.Parse(item["空干基高位热"].ToString()) : 0;
						entity.弹筒热 = item["弹筒热"] != DBNull.Value ? Decimal.Parse(item["弹筒热"].ToString()) : 0;
						entity.固定碳 = item["固定碳"] != DBNull.Value ? Decimal.Parse(item["固定碳"].ToString()) : 0;
						entity.干基硫 = item["干基硫"] != DBNull.Value ? Decimal.Parse(item["干基硫"].ToString()) : 0;
						entity.干燥无灰基氢 = item["干燥无灰基氢"] != DBNull.Value ? Decimal.Parse(item["干燥无灰基氢"].ToString()) : 0;
						entity.干燥无灰基高位热 = item["干燥无灰基高位热"] != DBNull.Value ? Decimal.Parse(item["干燥无灰基高位热"].ToString()) : 0;
						entity.干燥无灰基硫 = item["干燥无灰基硫"] != DBNull.Value ? Decimal.Parse(item["干燥无灰基硫"].ToString()) : 0;
						entity.收到基挥发份 = item["收到基挥发份"] != DBNull.Value ? Decimal.Parse(item["收到基挥发份"].ToString()) : 0;
						entity.收到基氢 = item["收到基氢"] != DBNull.Value ? Decimal.Parse(item["收到基氢"].ToString()) : 0;
						entity.干燥基高位热值 = item["干燥基高位热值"] != DBNull.Value ? Decimal.Parse(item["干燥基高位热值"].ToString()) : 0;
						entity.计价水分 = item["计价水分"] != DBNull.Value ? Decimal.Parse(item["计价水分"].ToString()) : 0;
						entity.计价热量 = item["计价热量"] != DBNull.Value ? Decimal.Parse(item["计价热量"].ToString()) : 0;
					}
					catch
					{
						output(string.Format("化验编号：{0}，煤质存在数据格式不正确。", entity.化验编号), eOutputType.Error);
						continue;
					}



					entity.化验员 = item["化验员"].ToString();
					entity.化验录入员 = item["化验录入员"].ToString();
					entity.化验结果 = item["化验结果"].ToString();
					entity.化验备注 = item["化验备注"].ToString();

					try
					{
						entity.煤量 = Decimal.Parse(item["煤量"].ToString());
						entity.车数 = Decimal.Parse(item["车数"].ToString());
					}
					catch
					{
						output(string.Format("化验编号：{0}，煤量/车数数据格式不正确。", entity.化验编号), eOutputType.Error);
						continue;
					}


					entity.车牌号 = item["车牌号"].ToString();

					DateTime samplingTime = DateTime.Now;
					if (string.IsNullOrWhiteSpace(item["采样日期"].ToString()) || !DateTime.TryParse(item["采样日期"].ToString(), out samplingTime))
					{
						output(string.Format("化验编号：{0}，采样日期格式不正确。", entity.化验编号), eOutputType.Error);
						continue;
					}
					entity.采样日期 = DateTime.Parse(item["采样日期"].ToString());
					//entity.同步完成 = 0;
					//entity.同步完成时间 = DateTime.Now;

					if (mysqlDber.Update<WL_AssayInfo>(entity) > 0)
					{
						output("推送化验信息至智能物流成功，操作：更新，数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(entity), eOutputType.Normal);
					}
					#endregion
				}
			}

		}


	}
}
