using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.Common.Entities.CarTransport;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.Common;
using CMCS.Common.Entities;
using CMCS.Common.Views;
using CMCS.DapperDber.Util;
using CMCS.Common.Enums;

namespace CMCS.CarTransport.DAO
{
	/// <summary>
	/// 汽车过衡业务
	/// </summary>
	public class WeighterDAO
	{
		private static WeighterDAO instance;

		public static WeighterDAO GetInstance()
		{
			if (instance == null)
			{
				instance = new WeighterDAO();
			}

			return instance;
		}

		private WeighterDAO()
		{ }

		public OracleDapperDber SelfDber
		{
			get { return Dbers.GetInstance().SelfDber; }
		}

		CommonDAO commonDAO = CommonDAO.GetInstance();
		CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();

		#region 入厂煤业务

		/// <summary>
		/// 获取指定日期已完成的入厂煤运输记录
		/// </summary>
		/// <param name="dtStart"></param>
		/// <param name="dtEnd"></param>
		/// <returns></returns>
		public List<View_BuyFuelTransport> GetFinishedBuyFuelTransport(DateTime dtStart, DateTime dtEnd)
		{
			return SelfDber.Entities<View_BuyFuelTransport>("where SuttleWeight!=0 and IsUse=1 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by InFactoryTime desc", new { dtStart = dtStart, dtEnd = dtEnd });
		}

		/// <summary>
		/// 获取未完成的入厂煤运输记录
		/// </summary>
		/// <returns></returns>
		public List<View_BuyFuelTransport> GetUnFinishBuyFuelTransport()
		{
			return SelfDber.Entities<View_BuyFuelTransport>("where SuttleWeight=0 and IsUse=1 order by InFactoryTime desc");
		}

		/// <summary>
		/// 保存入厂煤运输记录
		/// </summary>
		/// <param name="transportId"></param>
		/// <param name="weight">重量</param>
		/// <param name="place"></param>
		/// <returns></returns>
		public bool SaveBuyFuelTransport(string transportId, decimal weight, DateTime dt, string place)
		{
			CmcsBuyFuelTransport transport = SelfDber.Get<CmcsBuyFuelTransport>(transportId);
			if (transport == null) return false;

			//根据当前流程节点名称判断
			if (transport.StepName != eTruckInFactoryStep.重车.ToString())
			{
				transport.StepName = eTruckInFactoryStep.重车.ToString();
				transport.GrossWeight = weight;
				transport.GrossPlace = place;
				transport.GrossTime = dt;
				CmcsInNetTransport inNetTransport = SelfDber.Get<CmcsInNetTransport>(transport.OutNetTransportId);
				if (inNetTransport != null)
				{
					inNetTransport.StepName = eTruckInFactoryStep.重车.ToString();
					SelfDber.Update(inNetTransport);
				}
			}
			else if (transport.StepName == eTruckInFactoryStep.重车.ToString())
			{
				transport.StepName = eTruckInFactoryStep.轻车.ToString();
				transport.TareWeight = weight;
				transport.TarePlace = place;
				transport.TareTime = dt;
				transport.OutFactoryTime = dt;
				CmcsInNetTransport inNetTransport = SelfDber.Get<CmcsInNetTransport>(transport.OutNetTransportId);
				if (inNetTransport != null)
				{
					inNetTransport.StepName = eTruckInFactoryStep.轻车.ToString();
					inNetTransport.IsFinish = 1;
					SelfDber.Update(inNetTransport);
				}
				//扣吨量
				transport.DeductWeight = GetDeductWeightWithOutAuto(transport.Id);
				transport.SuttleWeight = transport.GrossWeight - transport.TareWeight - transport.DeductWeight;
				if (transport.GrossWeight > 0 && transport.TareWeight > 0)
				{
					if (transport.TheMine.PurcHaseType == "地方煤")
					{
						transport.TicketWeight = transport.SuttleWeight;
					}
					else if (transport.TheMine.PurcHaseType == "厂矿直供" && transport.TicketWeight > 0 && transport.TicketWeight <= (transport.GrossWeight - transport.TareWeight))
					{
						CmcsBuyFuelTransportDeduct deduct = commonDAO.SelfDber.Entity<CmcsBuyFuelTransportDeduct>("where TransportId=:TransportId and DeductType = '磅差'", new { TransportId = transport.Id });
						decimal KgWeight = transport.GrossWeight - transport.TareWeight - transport.TicketWeight + 0.1m;
						transport.SuttleWeight = transport.TicketWeight - 0.1m - transport.DeductWeight;
						if (deduct == null)
						{
							deduct = new CmcsBuyFuelTransportDeduct();
							deduct.TransportId = transport.Id;
							deduct.DeductType = "磅差";
							deduct.DeductWeight = KgWeight;
							Dbers.GetInstance().SelfDber.Insert(deduct);
						}
						else if (deduct != null && deduct.DeductWeight != KgWeight)
						{
							deduct.DeductWeight = KgWeight;
							Dbers.GetInstance().SelfDber.Update(deduct);
						}
						transport.DeductWeight += KgWeight;
					}
				}
				// 回皮即完结
				transport.IsFinish = 1;

				//流程结束时删除临时运输记录
				CmcsUnFinishTransport unFinishTransport = SelfDber.Entity<CmcsUnFinishTransport>("where TransportId=:TransportId", new { TransportId = transportId });
				if (unFinishTransport != null)
					SelfDber.Delete<CmcsUnFinishTransport>(unFinishTransport.Id);
			}
			else
				return false;

			return SelfDber.Update(transport) > 0;
		}

		/// <summary>
		/// 获取扣吨量
		/// </summary>
		/// <param name="transportId"></param>
		/// <returns></returns>
		public decimal GetDeductWeight(string transportId)
		{
			decimal DeductWeight = 0;
			List<CmcsBuyFuelTransportDeduct> listDeducts = SelfDber.Entities<CmcsBuyFuelTransportDeduct>("where TransportId=:TransportId ", new { TransportId = transportId });
			if (listDeducts.Count > 0)
				DeductWeight = listDeducts.Sum(a => a.DeductWeight);

			return DeductWeight;
		}

		/// <summary>
		/// 获取扣吨量(不包括自动扣磅差)
		/// </summary>
		/// <param name="transportId"></param>
		/// <returns></returns>
		public decimal GetDeductWeightWithOutAuto(string transportId)
		{
			decimal DeductWeight = 0;
			List<CmcsBuyFuelTransportDeduct> listDeducts = SelfDber.Entities<CmcsBuyFuelTransportDeduct>("where TransportId=:TransportId and DeductType!='磅差'", new { TransportId = transportId });
			if (listDeducts.Count > 0)
				DeductWeight = listDeducts.Sum(a => a.DeductWeight);

			return DeductWeight;
		}

		#endregion

		#region 其他物资业务

		/// <summary>
		/// 获取指定日期已完成的其他物资运输记录
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public List<CmcsGoodsTransport> GetFinishedGoodsTransport(DateTime dtStart, DateTime dtEnd)
		{
			return SelfDber.Entities<CmcsGoodsTransport>("where SuttleWeight>0 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by InFactoryTime desc", new { dtStart = dtStart, dtEnd = dtEnd });
		}

		/// <summary>
		/// 获取未完成的其他物资运输记录
		/// </summary>
		/// <returns></returns>
		public List<CmcsGoodsTransport> GetUnFinishGoodsTransport()
		{
			return SelfDber.Entities<CmcsGoodsTransport>("where SuttleWeight=0 and IsUse=1 order by InFactoryTime desc");
		}

		/// <summary>
		/// 保存其他物资运输记录
		/// </summary>
		/// <param name="transportId"></param>
		/// <param name="weight">重量</param>
		/// <param name="place"></param>
		/// <returns></returns>
		public bool SaveGoodsTransport(string transportId, decimal weight, DateTime dt, string place)
		{
			CmcsGoodsTransport transport = SelfDber.Get<CmcsGoodsTransport>(transportId);
			if (transport == null) return false;

			if (transport.FirstWeight == 0)
			{
				transport.StepName = eTruckInFactoryStep.第一次称重.ToString();
				transport.FirstWeight = weight;
				transport.FirstPlace = place;
				transport.FirstTime = dt;
			}
			else if (transport.SecondWeight == 0)
			{
				transport.StepName = eTruckInFactoryStep.第二次称重.ToString();
				transport.SecondWeight = weight;
				transport.SecondPlace = place;
				transport.SecondTime = dt;
				transport.SuttleWeight = Math.Abs(transport.FirstWeight - transport.SecondWeight) - transport.DeductWeight;

				// 回皮即完结
				transport.IsFinish = 1;
			}
			else
				return false;

			return SelfDber.Update(transport) > 0;
		}


		#endregion
	}
}
