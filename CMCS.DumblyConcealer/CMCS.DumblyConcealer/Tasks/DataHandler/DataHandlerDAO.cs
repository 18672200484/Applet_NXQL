using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Entities.Sys;
using CMCS.DapperDber.Dbs.AccessDb;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.CarSynchronous.Enums;
using CMCS.DumblyConcealer.Tasks.DataHandler.Entities;
using CMCS.DumblyConcealer.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMCS.DumblyConcealer.Tasks.DataHandler
{
	/// <summary>
	/// 综合事件处理
	/// </summary>
	public class DataHandlerDAO
	{
		private static DataHandlerDAO instance;

		public static DataHandlerDAO GetInstance()
		{
			if (instance == null)
			{
				instance = new DataHandlerDAO();
			}
			return instance;
		}

		CommonDAO commonDAO = CommonDAO.GetInstance();
		WebApiHelper webApiHelper = new WebApiHelper();

		private DataHandlerDAO()
		{ }

		/// <summary>
		/// 将汽车入厂煤运输记录同步到批次明细中
		/// </summary>
		/// <param name="transportId">汽车入厂煤运输记录Id</param>
		/// <returns></returns>
		public void SyncToBatch(Action<string, eOutputType> output)
		{
			int res = 0;
			bool succes = false;

			//已完结的有效数据
			foreach (CmcsBuyFuelTransport transport in commonDAO.SelfDber.Entities<CmcsBuyFuelTransport>("where IsUse=1 and IsFinish=1 and IsSyncBatch=0 "))
			{
				if (transport.TareTime == null) continue;

				//CmcsInFactoryBatch batch = commonDAO.SelfDber.Entity<CmcsInFactoryBatch>("where CreationTime like '%" + transport.InFactoryTime.ToString("yyyy-MM-dd") + "%' and SupplierId=:SupplierId and MineId=:MineId and FuelKindId=:FuelKindId and IsDeleted=0",
				//    new { SupplierId = transport.SupplierId, MineId = transport.MineId, FuelKindId = transport.FuelKindId });

				CmcsInFactoryBatch batch = commonDAO.SelfDber.Get<CmcsInFactoryBatch>(transport.InFactoryBatchId);
				if (batch == null) continue;

				CmcsTransport truck = commonDAO.SelfDber.Entity<CmcsTransport>("where PKID=:PKID and IsDeleted=0", new { PKID = transport.Id });
				if (truck != null)
				{
					truck.TransportNo = transport.CarNumber;
					truck.LastModificAtionTime = transport.LastModificAtionTime;
					truck.InfactoryTime = transport.InFactoryTime;
					truck.ArriveDate = transport.GrossTime;
					truck.TareDate = transport.TareTime;
					truck.OutfactoryTime = transport.OutFactoryTime == null ? DateTime.MinValue : transport.OutFactoryTime;
					truck.TicketQty = transport.TicketWeight;
					truck.GrossQty = transport.GrossWeight;
					truck.SkinQty = transport.TareWeight;
					truck.SuttleQty = transport.SuttleWeight;
					truck.KgQty = transport.DeductWeight;
					truck.CheckQty = transport.SuttleWeight;
					truck.MarginQty = transport.SuttleWeight - transport.TicketWeight;
					truck.InFactoryBatchId = batch.Id;
					truck.DataFrom = "汽车智能化";
					succes = commonDAO.SelfDber.Update(truck) > 0;
				}
				else
				{
					truck = new CmcsTransport()
					{
						TransportNo = transport.CarNumber,
						LastModificAtionTime = transport.LastModificAtionTime,
						InfactoryTime = transport.InFactoryTime,
						ArriveDate = transport.GrossTime,
						TareDate = transport.TareTime,
						OutfactoryTime = transport.OutFactoryTime == null ? DateTime.MinValue : transport.OutFactoryTime,
						TicketQty = transport.TicketWeight,
						GrossQty = transport.GrossWeight,
						SkinQty = transport.TareWeight,
						SuttleQty = transport.SuttleWeight,
						KgQty = transport.DeductWeight,
						CheckQty = transport.SuttleWeight,
						MarginQty = transport.SuttleWeight - transport.TicketWeight,
						InFactoryBatchId = batch.Id,
						PKID = transport.Id,
						DataFrom = "汽车智能化"
					};

					succes = commonDAO.SelfDber.Insert(truck) > 0;
				}

				if (succes)
				{
					//更新智能化运输记录
					transport.IsSyncBatch = 1;
					transport.InFactoryBatchId = batch.Id;
					Dbers.GetInstance().SelfDber.Update<CmcsBuyFuelTransport>(transport);

					// 更新批次的量 
					List<CmcsTransport> listTransport = commonDAO.SelfDber.Entities<CmcsTransport>("where InFactoryBatchId=:InFactoryBatchId and IsDeleted=0", new { InFactoryBatchId = batch.Id });

					batch.SuttleQty = listTransport.Sum(a => a.SuttleQty);
					batch.TicketQty = listTransport.Sum(a => a.TicketQty);
					batch.CheckQty = listTransport.Sum(a => a.CheckQty);
					batch.MarginQty = listTransport.Sum(a => a.MarginQty);
					batch.TransportNumber = listTransport.Count;

					Dbers.GetInstance().SelfDber.Update<CmcsInFactoryBatch>(batch);

					res++;
				}

			}

			output(string.Format("同步批次明细数据 {0} 条", res), eOutputType.Normal);
		}

		/// <summary>
		/// 同步外网矿发运输记录信息
		/// </summary>
		/// <param name="output"></param>
		public void SyncOutNetTransport(Action<string, eOutputType> output, string outNetWebApi)
		{
			int res = 0;

			try
			{
				string str = webApiHelper.HttpApi(outNetWebApi + "api/services/report/DepartSituation/GetSyncList", "", "post");
				ApiList<CmcsInNetTransport> result = JsonConvert.DeserializeObject<ApiList<CmcsInNetTransport>>(str);
				if (result.success && result.result != null && result.result.Count > 0)
				{
					foreach (CmcsInNetTransport item in result.result)
					{
						if (Dbers.GetInstance().SelfDber.Get<CmcsInNetTransport>(item.Id) == null)
							Dbers.GetInstance().SelfDber.Insert(item);
						else
						{
							item.LastModificAtionTime = DateTime.Now;
							Dbers.GetInstance().SelfDber.Update(item);
						}

						//更新已同步标识
						JObject obj = new JObject() { { "SyncFlag", 1 }, { "Id", item.Id }, { "StepName", item.StepName } };
						webApiHelper.HttpApi(outNetWebApi + "api/services/report/DepartSituation/UpdateStepName", obj.ToString(), "post");

						res++;
					}
				}
			}
			catch (Exception ex)
			{
				output(string.Format("同步外网矿发运输记录信息错误" + ex.Message), eOutputType.Error);
				return;
			}

			output(string.Format("同步外网矿发运输记录信息 {0} 条", res), eOutputType.Normal);
		}

		/// <summary>
		/// 同步更新外网矿发运输记录节点状态
		/// </summary>
		/// <param name="output"></param>
		public void SyncUpdateTransportStepName(Action<string, eOutputType> output, string outNetWebApi)
		{
			int res = 0;

			try
			{
				List<CmcsInNetTransport> list = Dbers.GetInstance().SelfDber.Entities<CmcsInNetTransport>("where SyncFlag=0 and StepName='入厂'");
				foreach (CmcsInNetTransport item in list)
				{
					//更新已同步标识
					JObject obj = new JObject() { { "SyncFlag", 1 }, { "Id", item.Id }, { "StepName", item.StepName }, { "InfactoryTime", item.Infactorytime } };
					webApiHelper.HttpApi(outNetWebApi + "api/services/report/DepartSituation/UpdateStepName", obj.ToString(), "post");

					item.SyncFlag = 1;
					item.LastModificAtionTime = DateTime.Now;
					res += Dbers.GetInstance().SelfDber.Update(item);
				}
			}
			catch (Exception ex)
			{
				output(string.Format("同步更新外网矿发运输记录节点状态错误" + ex.Message), eOutputType.Error);
				return;
			}

			output(string.Format("同步更新外网矿发运输记录节点状态 {0} 条", res), eOutputType.Normal);
		}
	}
}
