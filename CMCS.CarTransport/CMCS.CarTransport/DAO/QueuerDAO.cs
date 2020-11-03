using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Entities.iEAA;
using CMCS.Common.Entities.Sys;
using CMCS.Common.Enums;
using CMCS.Common.Views;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.DapperDber.Util;

namespace CMCS.CarTransport.DAO
{
	/// <summary>
	/// 汽车入厂排队业务
	/// </summary>
	public class QueuerDAO
	{
		private static QueuerDAO instance;

		public static QueuerDAO GetInstance()
		{
			if (instance == null)
			{
				instance = new QueuerDAO();
			}

			return instance;
		}

		private QueuerDAO()
		{ }

		public OracleDapperDber SelfDber
		{
			get { return Dbers.GetInstance().SelfDber; }
		}

		CommonDAO commonDAO = CommonDAO.GetInstance();
		CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();

		#region 入厂煤业务

		/// <summary>
		/// 生成入厂煤运输排队记录，同时生成批次信息以及采制化三级编码
		/// </summary>
		/// <param name="autotruck">车</param>
		/// <param name="supplier">供煤单位</param>
		/// <param name="mine">矿点</param>
		/// <param name="transportCompany">运输单位</param>
		/// <param name="fuelKind">煤种</param>
		/// <param name="ticketWeight">矿发量</param>
		/// <param name="inFactoryTime">入厂时间</param>
		/// <param name="remark">备注</param>
		/// <param name="place">地点</param>
		/// <param name="samplingType">采样方式</param> 
		/// <returns></returns>
		public bool JoinQueueBuyFuelTransport(CmcsAutotruck autotruck, CmcsMine mine, CmcsTransportCompany transportCompany, CmcsFuelKind fuelKind, CmcsInNetTransport inNetTransport, decimal ticketWeight, string samplerCode, string weighterCode, DateTime inFactoryTime, string remark, string place, CmcsSupplier supplier = null)
		{
			CmcsBuyFuelTransport transport = new CmcsBuyFuelTransport
			{
				SerialNumber = carTransportDAO.CreateNewTransportSerialNumber(eCarType.入厂煤, inFactoryTime),
				AutotruckId = autotruck.Id,
				CarNumber = autotruck.CarNumber,
				MineId = mine.Id,
				MineName = mine.Name,
				TransportCompanyId = transportCompany.Id,
				TransportCompanyName = transportCompany.Name,
				FuelKindId = fuelKind.Id,
				FuelKindName = fuelKind.Name,
				TicketWeight = ticketWeight,
				InFactoryTime = inFactoryTime,
				IsFinish = 0,
				IsUse = 1,
				StepName = eTruckInFactoryStep.入厂.ToString(),
				OutNetTransportId = inNetTransport == null ? "" : inNetTransport.Id,
				SamplePlace = samplerCode,
				GrossPlace = weighterCode,
				Remark = remark,
				SamplingType = eSamplingType.机械采样.ToString()
			};
			if (supplier != null)
			{
				transport.SupplierId = supplier.Id;
				transport.SupplierName = supplier.Name;
			}
			// 生成批次以及采制化三级编码数据 
			CmcsInFactoryBatch inFactoryBatch = carTransportDAO.GCQCInFactoryBatchByBuyFuelTransport(transport);
			if (inFactoryBatch != null)
			{
				if (SelfDber.Insert(transport) > 0)
				{
					if (inNetTransport != null)
					{
						//更新内网矿发运输记录节点状态
						inNetTransport.StepName = eTruckInFactoryStep.入厂.ToString();
						inNetTransport.Infactorytime = inFactoryTime;
						SelfDber.Update(inNetTransport);
					}

					// 插入未完成运输记录
					return SelfDber.Insert(new CmcsUnFinishTransport
					{
						TransportId = transport.Id,
						CarType = eCarType.入厂煤.ToString(),
						AutotruckId = autotruck.Id,
						PrevPlace = place,
					}) > 0;
				}
			}

			return false;
		}

		/// <summary>
		/// 保存入厂煤运输记录
		/// </summary>
		/// <param name="transportId"></param> 
		/// <param name="dt"></param>
		/// <returns></returns>
		public bool SaveBuyFuelTransport(string transportId, DateTime dt)
		{
			CmcsBuyFuelTransport transport = SelfDber.Get<CmcsBuyFuelTransport>(transportId);
			if (transport == null) return false;

			transport.StepName = eTruckInFactoryStep.入厂.ToString();
			transport.InFactoryTime = dt;

			return SelfDber.Update(transport) > 0;
		}

		/// <summary>
		/// 获取指定日期已完成的入厂煤运输记录
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public List<View_BuyFuelTransport> GetFinishedBuyFuelTransport(DateTime dtStart, DateTime dtEnd)
		{
			return SelfDber.Entities<View_BuyFuelTransport>("where IsFinish=1 and IsUse=1 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by InFactoryTime desc", new { dtStart = dtStart, dtEnd = dtEnd });
		}

		/// <summary>
		/// 获取未完成的入厂煤运输记录
		/// </summary>
		/// <returns></returns>
		public List<View_BuyFuelTransport> GetUnFinishBuyFuelTransport()
		{
			return SelfDber.Entities<View_BuyFuelTransport>("where IsFinish=0 and IsUse=1 order by InFactoryTime desc");
		}

		/// <summary>
		/// 更改入厂煤运输记录的无效状态
		/// </summary>
		/// <param name="buyFuelTransportId"></param>
		/// <param name="isValid">是否有效</param>
		/// <returns></returns>
		public bool ChangeBuyFuelTransportToInvalid(string buyFuelTransportId, bool isValid)
		{
			if (isValid)
			{
				// 设置为有效
				CmcsBuyFuelTransport buyFuelTransport = SelfDber.Get<CmcsBuyFuelTransport>(buyFuelTransportId);
				if (buyFuelTransport != null)
				{
					if (SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsBuyFuelTransport>() + " set IsUse=1 where Id=:Id", new { Id = buyFuelTransportId }) > 0)
					{
						if (buyFuelTransport.IsFinish == 0)
						{
							SelfDber.Insert(new CmcsUnFinishTransport
							{
								AutotruckId = buyFuelTransport.AutotruckId,
								CarType = eCarType.入厂煤.ToString(),
								TransportId = buyFuelTransport.Id,
								PrevPlace = "未知"
							});
						}

						return true;
					}
				}
			}
			else
			{
				// 设置为无效

				if (SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsBuyFuelTransport>() + " set IsUse=0 where Id=:Id", new { Id = buyFuelTransportId }) > 0)
				{
					SelfDber.DeleteBySQL<CmcsUnFinishTransport>("where TransportId=:TransportId", new { TransportId = buyFuelTransportId });

					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 根据车牌号获取指定到达日期的入厂煤来煤预报
		/// </summary>
		/// <param name="carNumber">车牌号</param>
		/// <param name="inFactoryTime">预计到达日期</param>
		/// <returns></returns>
		public List<CmcsLMYB> GetBuyFuelForecastByCarNumber(string carNumber, DateTime inFactoryTime)
		{
			return SelfDber.Query<CmcsLMYB>("select l.* from " + EntityReflectionUtil.GetTableName<CmcsLMYBDetail>() + " ld left join " + EntityReflectionUtil.GetTableName<CmcsLMYB>() + " l on l.Id=ld.lmybid where ld.CarNumber=:CarNumber and to_char(InFactoryTime,'yyyymmdd')=to_char(:InFactoryTime,'yyyymmdd') order by l.InFactoryTime desc",
				new { CarNumber = carNumber.Trim(), InFactoryTime = inFactoryTime }).ToList();
		}

		/// <summary>
		/// 获取采样通道车数随机指定采样机编号
		/// </summary>
		/// <param name="cyjbm">全过程计划中采样机编号集合“,”隔开</param>
		/// <param name="SampleWayCount">采样机通道最大车数</param>
		/// <param name="isSampleWayCount">是否启用采样机通道数</param>
		/// <param name="result">采样机编号</param>
		/// <param name="result">返回消息</param>
		/// <returns></returns>
		public bool GetSamplerMachineCode(string cyjbm, int SampleWayCount, bool isSampleWayCount, ref string sampler, ref string result)
		{
			//最终采样机编号
			string samplerMachineCode = string.Empty;
			//当前启用的采样机对应待采车数
			Dictionary<string, int> dicMeetSample = new Dictionary<string, int>();

			if (!string.IsNullOrWhiteSpace(cyjbm))
			{
				string[] strCyj = cyjbm.Split(',');

				//1、获取计划中的采样机所对应的待采车数
				for (int i = 0; i < strCyj.Length; i++)
				{
					string Sampler = strCyj[i];
					//查询采样通道对应待采样的车辆数
					int SampleWayCountSum = commonDAO.SelfDber.Count<CmcsBuyFuelTransport>("where StepName=:StepName and SamplePlace=:SamplePlace and isuse=1", new { StepName = eTruckInFactoryStep.入厂.ToString(), SamplePlace = Sampler });

					if (isSampleWayCount)
					{
						if (SampleWayCountSum < SampleWayCount)
							dicMeetSample.Add(Sampler, SampleWayCountSum);
					}
					else
						dicMeetSample.Add(Sampler, SampleWayCountSum);
				}

				//2、没有可以分配的采样机的，返回false
				if (dicMeetSample.Count == 0)
				{
					result = "待采样车数已满，请等待";
					return false;
				}
				//3、有可以分配的采样机的 
				else
				{
					//选择排队车数最少的采样机
					//var dicMin = from d in dicMeetSample orderby d.Value ascending select d;
					//sampler = dicMin.FirstOrDefault().Key;

					//随机选择采样机号
					sampler = dicMeetSample.ElementAt(new Random().Next(0, dicMeetSample.Count())).Key;
				}
				return true;
			}

			result = "采样机编号未设置";
			return false;
		}

		/// <summary>
		/// 根据矿点获取设置的采样机
		/// </summary>
		/// <param name="mineName"></param>
		/// <returns></returns>
		public string GetSamplerByMineName(string mineName)
		{
			CmcsSetSampler entity = commonDAO.SelfDber.Entity<CmcsSetSampler>("where IsDeleted=0 and MineName=:MineName and StartTime<=sysdate and EndTime >sysdate order by EndTime desc", new { MineName = mineName });
			if (entity != null) return entity.Sampler;
			return string.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="qchbm"></param>
		/// <param name="WeighterWayCount"></param>
		/// <param name="weighter"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public bool GetWeighterCode(string qchbm, ref string weighter, ref string result)
		{
			//最终汽车衡编号
			string weighterMachineCode = string.Empty;
			//当前启用的汽车衡对应待采车数
			Dictionary<string, int> dicMeetWeight = new Dictionary<string, int>();

			if (!string.IsNullOrWhiteSpace(qchbm))
			{
				string[] strQch = qchbm.Split(',');

				//1、获取计划中的汽车衡所对应的待过衡车数
				for (int i = 0; i < strQch.Length; i++)
				{
					string Weighter = strQch[i];
					//查询采样通道对应待采样的车辆数
					int WeightWayCountSum = commonDAO.SelfDber.Count<CmcsBuyFuelTransport>("where StepName=:StepName and GrossPlace=:GrossPlace and isuse=1", new { StepName = eTruckInFactoryStep.采样.ToString(), GrossPlace = Weighter });

					dicMeetWeight.Add(Weighter, WeightWayCountSum);
				}

				//随机选择采样机号
				weighter = dicMeetWeight.ElementAt(new Random().Next(0, dicMeetWeight.Count())).Key;

				return true;
			}

			result = "采样机编号未设置";
			return false;
		}

		/// <summary>
		/// 获取当前指定矿点的来煤量
		/// </summary>
		/// <param name="mineId">矿点id</param>
		/// <returns></returns>
		public decimal GetTodayFuelQtyByMineId(string mineId)
		{
			return SelfDber.Entities<CmcsBuyFuelTransport>("where IsUse=1 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd", new { dtStart = DateTime.Now.Date, dtEnd = DateTime.Now.Date.AddDays(1) }).Sum(a => a.SuttleWeight);
		}
		#endregion

		#region 销售煤业务
		public bool JoinQueueSaleFuelTransport(CmcsAutotruck autotruck, CmcsSupplier supplier, CmcsTransportCompany transportCompany, CmcsFuelKind fuelKind, DateTime inFactoryTime, string remark, string place, string loadarea)
		{
			CmcsSaleFuelTransport transport = new CmcsSaleFuelTransport
			{
				SerialNumber = carTransportDAO.CreateNewTransportSerialNumber(eCarType.销售煤, inFactoryTime),
				AutotruckId = autotruck.Id,
				CarNumber = autotruck.CarNumber,
				TransportCompanyName = transportCompany.Name,
				TransportCompanyId = transportCompany.Id,
				SupplierName = supplier.Name,
				SupplierId = supplier.Id,
				FuelKindName = fuelKind.Name,
				FuelKindId = fuelKind.Id,
				InFactoryTime = inFactoryTime,
				IsFinish = 0,
				IsUse = 1,
				StepName = eTruckInFactoryStep.入厂.ToString(),
				LoadArea = loadarea,
				Remark = remark
			};
			if (SelfDber.Insert(transport) > 0)
			{
				// 插入未完成运输记录
				return SelfDber.Insert(new CmcsUnFinishTransport
				{
					TransportId = transport.Id,
					CarType = eCarType.销售煤.ToString(),
					AutotruckId = autotruck.Id,
					PrevPlace = place,
				}) > 0;
			}
			return false;
		}

		/// <summary>
		/// 获取指定日期已完成的入厂煤运输记录
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public List<View_SaleFuelTransport> GetFinishedSaleFuelTransport(DateTime dtStart, DateTime dtEnd)
		{
			return SelfDber.Entities<View_SaleFuelTransport>("where IsFinish=1 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by InFactoryTime desc", new { dtStart = dtStart, dtEnd = dtEnd });
		}

		/// <summary>
		/// 获取未完成的入厂煤运输记录
		/// </summary>
		/// <returns></returns>
		public List<View_SaleFuelTransport> GetUnFinishSaleFuelTransport()
		{
			return SelfDber.Entities<View_SaleFuelTransport>("where IsFinish=0 and IsUse=1 and UnFinishTransportId is not null order by InFactoryTime desc");
		}

		#endregion

		#region 其他物资业务

		/// <summary>
		/// 生成其他物资运输排队记录
		/// </summary>
		/// <param name="autotruck">车辆</param>
		/// <param name="supply">供货单位</param>
		/// <param name="receive">收货单位</param>
		/// <param name="goodsType">物资类型</param>
		/// <param name="inFactoryTime">入厂时间</param>
		/// <param name="remark">备注</param>
		/// <param name="place">地点</param>
		/// <returns></returns>
		public bool JoinQueueGoodsTransport(CmcsAutotruck autotruck, CmcsSupplyReceive supply, CmcsSupplyReceive receive, CmcsGoodsType goodsType, DateTime inFactoryTime, string remark, string place, ref CmcsGoodsTransport transport)
		{
			transport = new CmcsGoodsTransport
			{
				SerialNumber = carTransportDAO.CreateNewTransportSerialNumber(eCarType.其他物资, inFactoryTime),
				AutotruckId = autotruck.Id,
				CarNumber = autotruck.CarNumber,
				SupplyUnitId = supply.Id,
				SupplyUnitName = supply.UnitName,
				ReceiveUnitId = receive.Id,
				ReceiveUnitName = receive.UnitName,
				GoodsTypeId = goodsType.Id,
				GoodsTypeName = goodsType.GoodsName,
				InFactoryTime = inFactoryTime,
				IsFinish = 0,
				IsUse = 1,
				StepName = eTruckInFactoryStep.入厂.ToString(),
				Remark = remark,
				CarType = autotruck.CarType
			};

			return SelfDber.Insert(transport) > 0;
		}

		/// <summary>
		/// 获取指定日期已完成的其他物资运输记录
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public List<CmcsGoodsTransport> GetFinishedGoodsTransport(DateTime dtStart, DateTime dtEnd)
		{
			return SelfDber.Entities<CmcsGoodsTransport>("where IsFinish=1 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by InFactoryTime desc", new { dtStart = dtStart, dtEnd = dtEnd });
		}

		/// <summary>
		/// 获取未完成的其他物资运输记录
		/// </summary>
		/// <returns></returns>
		public List<CmcsGoodsTransport> GetUnFinishGoodsTransport()
		{
			return SelfDber.Entities<CmcsGoodsTransport>("where IsFinish=0 and IsUse=1 and Id in (select TransportId from " + EntityReflectionUtil.GetTableName<CmcsUnFinishTransport>() + " where CarType=:CarType) order by InFactoryTime desc", new { CarType = eCarType.其他物资.ToString() });
		}

		/// <summary>
		/// 更改其他物资运输记录的无效状态
		/// </summary>
		/// <param name="transportId"></param>
		/// <param name="isValid">是否有效</param>
		/// <returns></returns>
		public bool ChangeGoodsTransportToInvalid(string transportId, bool isValid)
		{
			if (isValid)
			{
				// 设置为有效
				CmcsGoodsTransport buyFuelTransport = SelfDber.Get<CmcsGoodsTransport>(transportId);
				if (buyFuelTransport != null)
				{
					if (SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsGoodsTransport>() + " set IsUse=1 where Id=:Id", new { Id = transportId }) > 0)
					{
						if (buyFuelTransport.IsFinish == 0)
						{
							SelfDber.Insert(new CmcsUnFinishTransport
							{
								AutotruckId = buyFuelTransport.AutotruckId,
								CarType = eCarType.其他物资.ToString(),
								TransportId = buyFuelTransport.Id,
								PrevPlace = "未知"
							});
						}

						return true;
					}
				}
			}
			else
			{
				// 设置为无效

				if (SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsGoodsTransport>() + " set IsUse=0 where Id=:Id", new { Id = transportId }) > 0)
				{
					SelfDber.DeleteBySQL<CmcsUnFinishTransport>("where TransportId=:TransportId", new { TransportId = transportId });

					return true;
				}
			}

			return false;
		}

		#endregion

		#region 系统权限管理

		/// <summary>
		/// 生成模块编码
		/// </summary>
		/// <returns></returns>
		public string CreateModuleno()
		{
			string moduleno = "0000";

			SysModule entity = SelfDber.Entities<SysModule>(" order by moduleno desc").FirstOrDefault();
			if (entity != null && !string.IsNullOrEmpty(entity.Moduleno))
			{
				int Count = Convert.ToInt32(entity.Moduleno) + 1;
				moduleno = Count.ToString().PadLeft(4, '0');
				return moduleno;
			}
			else
			{
				return moduleno;
			}
		}

		/// <summary>
		/// 生成功能编码
		/// </summary>
		/// <returns></returns>
		public string CreateResourceno(SysModule module, out int orderno)
		{
			string moduleNo = module.Moduleno;
			List<SysResource> entitys = SelfDber.Entities<SysResource>("where moduleid='" + module.Id + "' order by orderno desc");
			if (entitys.Count > 0)
			{
				SysResource resourceFirst = entitys[0];
				int Count = Convert.ToInt32(resourceFirst.Resno.Replace(moduleNo, "")) + 1;
				orderno = resourceFirst.OrderNO + 1;
				return moduleNo + Count.ToString().PadLeft(2, '0');
			}
			else
			{
				orderno = 0;
				return moduleNo = moduleNo + "01";
			}
		}

		/// <summary>
		/// 得到模块功能，没有就返回默认
		/// </summary>
		/// <param name="SysModule"></param>
		/// <returns></returns>
		public List<SysResource> GetResources(SysModule module, bool isInit)
		{
			List<SysResource> listResource = new List<SysResource>();
			if (isInit)
			{
				SysResource resource = new SysResource();
				resource.Resno = module.Moduleno + "01";
				resource.ResName = "查看";
				resource.ModuleId = module.Id;
				resource.OrderNO = 0;
				listResource.Add(resource);
				resource = new SysResource();
				resource.Resno = module.Moduleno + "02";
				resource.ResName = "新增";
				resource.ModuleId = module.Id;
				resource.OrderNO = 1;
				listResource.Add(resource);
				resource = new SysResource();
				resource.Resno = module.Moduleno + "03";
				resource.ResName = "修改";
				resource.ModuleId = module.Id;
				resource.OrderNO = 2;
				listResource.Add(resource);
				resource = new SysResource();
				resource.Resno = module.Moduleno + "04";
				resource.ResName = "删除";
				resource.ModuleId = module.Id;
				resource.OrderNO = 3;
				listResource.Add(resource);
			}
			else
				listResource = SelfDber.Entities<SysResource>("where moduleid='" + module.Id + "'");

			return listResource;
		}

		/// <summary>
		/// BS系统用户判断是否有权限
		/// </summary>
		/// <returns></returns>
		public bool CheckPower(string ModuleDll, string ResourceResno, Common.Entities.Sys.SysUsers CurrentLoginUser)
		{
			//超级管理员不需要判断权限
			if (CurrentLoginUser.Name == "admin")
				return true;

			SysModule module = SelfDber.Entity<SysModule>("where ModuleDll=:ModuleDll", new { ModuleDll = ModuleDll });
			if (module != null)
			{
				SysResource resource = SelfDber.Entity<SysResource>("where ModuleId=:ModuleId and Resno=:Resno", new { ModuleId = module.Id, Resno = module.Moduleno + ResourceResno });
				if (resource != null)
					return SelfDber.Entity<SysResourceUser>("where ResourceId=:ResourceId and UserId=:UserId", new { ResourceId = resource.Id, UserId = CurrentLoginUser.Id }) == null ? false : true;
			}
			return false;
		}

		/// <summary>
		/// CS系统用户判断是否有权限
		/// </summary>
		/// <param name="ModuleDll"></param>
		/// <param name="ResourceResno"></param>
		/// <param name="CurrentLoginUser"></param>
		/// <returns></returns>
		public bool CheckPower(string ModuleDll, string ResourceResno, CmcsUser CurrentLoginUser)
		{
			//超级管理员不需要判断权限
			if (CurrentLoginUser.IsSupper == 1)
				return true;

			SysModule module = SelfDber.Entity<SysModule>("where ModuleDll=:ModuleDll", new { ModuleDll = ModuleDll });
			if (module != null)
			{
				SysResource resource = SelfDber.Entity<SysResource>("where ModuleId=:ModuleId and Resno=:Resno", new { ModuleId = module.Id, Resno = module.Moduleno + ResourceResno });
				if (resource != null)
					return SelfDber.Entity<SysResourceUser>("where ResourceId=:ResourceId and UserId=:UserId", new { ResourceId = resource.Id, UserId = CurrentLoginUser.Id }) == null ? false : true;
			}
			return false;
		}
		#endregion
	}
}
