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
    /// 汽车出厂业务
    /// </summary>
    public class OuterDAO
    {
        private static OuterDAO instance;

        public static OuterDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new OuterDAO();
            }

            return instance;
        }

        private OuterDAO()
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
            return SelfDber.Entities<View_BuyFuelTransport>("where IsFinish=1 and IsUse=1 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by InFactoryTime desc", new { dtStart = dtStart, dtEnd = dtEnd });
        }

        /// <summary>
        /// 获取未完成的入厂煤运输记录
        /// </summary>
        /// <returns></returns>
        public List<View_BuyFuelTransport> GetUnFinishBuyFuelTransport()
        {
            return SelfDber.Entities<View_BuyFuelTransport>("where IsFinish=0 and IsUse=1 order by InFactoryTime desc", new { OutFactoryTime = new DateTime(2000, 1, 1) });
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

            transport.StepName = eTruckInFactoryStep.出厂.ToString();
            transport.OutFactoryTime = dt;
            transport.IsFinish = 1;

            return SelfDber.Update(transport) > 0;
        }

        #endregion

        #region 销售煤业务
        /// <summary>
        /// 获取指定日期已完成的入厂煤运输记录
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public List<View_SaleFuelTransport> GetFinishedSaleFuelTransport(DateTime dtStart, DateTime dtEnd)
        {
            return SelfDber.Entities<View_SaleFuelTransport>("where OutFactoryTime>:OutFactoryTime and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by InFactoryTime desc", new { OutFactoryTime = new DateTime(2000, 1, 1), dtStart = dtStart, dtEnd = dtEnd });
        }

        /// <summary>
        /// 获取未完成的入厂煤运输记录
        /// </summary>
        /// <returns></returns>
        public List<View_SaleFuelTransport> GetUnFinishSaleFuelTransport()
        {
            return SelfDber.Entities<View_SaleFuelTransport>("where OutFactoryTime<:OutFactoryTime and IsUse=1 and UnFinishTransportId is not null order by InFactoryTime desc", new { OutFactoryTime = new DateTime(2000, 1, 1) });
        }

        /// <summary>
        /// 保存入厂煤运输记录
        /// </summary>
        /// <param name="transportId"></param> 
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SaveSaleFuelTransport(string transportId, DateTime dt)
        {
            CmcsSaleFuelTransport transport = SelfDber.Get<CmcsSaleFuelTransport>(transportId);
            if (transport == null) return false;

            transport.StepName = eTruckInFactoryStep.出厂.ToString();
            transport.OutFactoryTime = dt;
            transport.IsFinish = 1;

            return SelfDber.Update(transport) > 0;
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
            return SelfDber.Entities<CmcsGoodsTransport>("where OutFactoryTime>:OutFactoryTime and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by InFactoryTime desc", new { OutFactoryTime = new DateTime(2000, 1, 1), dtStart = dtStart, dtEnd = dtEnd });
        }

        /// <summary>
        /// 获取未完成的其他物资运输记录
        /// </summary>
        /// <returns></returns>
        public List<CmcsGoodsTransport> GetUnFinishGoodsTransport()
        {
            return SelfDber.Entities<CmcsGoodsTransport>("where OutFactoryTime<:OutFactoryTime and IsUse=1 and Id in (select TransportId from " + EntityReflectionUtil.GetTableName<CmcsUnFinishTransport>() + " where CarType=:CarType) order by InFactoryTime desc", new { OutFactoryTime = new DateTime(2000, 1, 1), CarType = eCarType.其他物资.ToString() });
        }

        /// <summary>
        /// 保存其他物资运输记录
        /// </summary>
        /// <param name="transportId"></param> 
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SaveGoodsTransport(string transportId, DateTime dt)
        {
            CmcsGoodsTransport transport = SelfDber.Get<CmcsGoodsTransport>(transportId);
            if (transport == null) return false;

            transport.StepName = eTruckInFactoryStep.出厂.ToString();
            transport.OutFactoryTime = dt;
            transport.IsFinish = 1;

            return SelfDber.Update(transport) > 0;
        }

        #endregion

    }
}
