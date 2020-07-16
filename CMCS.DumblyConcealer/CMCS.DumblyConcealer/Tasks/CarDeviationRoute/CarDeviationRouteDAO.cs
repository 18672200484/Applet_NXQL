using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.DumblyConcealer.Enums;
using CMCS.Common;
using CMCS.Common.DapperDber_etc;
using CMCS.DumblyConcealer.Tasks.CarDeviationRoute.Entities;
using CMCS.DumblyConcealer.Utilities;
using System.Data;

namespace CMCS.DumblyConcealer.Tasks.CarDeviationRoute
{
    public class CarDeviationRouteDAO
    {
        private static CarDeviationRouteDAO instance;

        public static CarDeviationRouteDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new CarDeviationRouteDAO();
            }
            return instance;
        }

        private CarDeviationRouteDAO()
        {

        }

        CommonDAO commonDAO = CommonDAO.GetInstance();
        OracleDapperDber_iEAA SelfDber = Dbers.GetInstance().SelfDber;

        #region 生成路线偏离数据
        /// <summary>
        /// 生成路线偏离数据
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SaveToCarDeciationRoute(Action<string, eOutputType> output)
        {
            DeviateWarning warning = SelfDber.Entity<DeviateWarning>("where IsDeleted=0");
            if (warning == null)
            {
                output("未设置路线偏离预警距离！", eOutputType.Error);
                return;
            }
            if (warning.Distance <= 0)
            {
                output("设置路线偏离预警距离小于等于0，请正确设置！", eOutputType.Error);
                return;
            }

            //查询所有在途的车辆，然后根据运输记录中的实时位置去判断路线的偏离
            List<ShowEntity> list = GetInRouteCars();
            if (list == null || list.Count <= 0)
            {
                output("暂无在途车辆！", eOutputType.Warn);
                return;
            }
            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.ROUTEPOINTS))
                {
                    output(string.Format("车号：{0}，运输记录ID：{1}对应的发车未设置线路！", item.CARNUMBER, item.ID), eOutputType.Error);
                    continue;
                }

                //如果车辆已报修并且未处理就不管了偏不偏移了
                DataTable dtIsRepair = SelfDber.ExecuteDataTable(String.Format("select count(1) from CMCSTBCARREPAIRINFO t where t.isdeleted=0 and t.repairstatus=0 and t.carid='{0}'", item.CarId));
                if (dtIsRepair != null && dtIsRepair.Rows.Count > 0 && dtIsRepair.Rows[0][0].ToString() != "0")//车辆去修理了就不算路线偏移
                    continue;

                double sslng = 0;
                double sslat = 0;
                double.TryParse(item.LONGITUDE,out sslng);
                double.TryParse(item.LATITUDE,out sslat);
                if (sslng <= 0 || sslat <= 0)
                {
                    output(string.Format("车号：{0}，运输记录ID：{1}暂无实时位置！", item.CARNUMBER, item.ID), eOutputType.Error);
                    continue;
                }
                string[] lnglats = item.ROUTEPOINTS.Trim('|').Split('|');
                //循环点一个一个的计算
                List<double> minDistance = new List<double>();
                foreach (var lnglat in lnglats)
                {
                    minDistance.Add(GaodeHelper.GetTwoPointDistance(sslat, sslng, double.Parse(lnglat.Split(',')[1]), double.Parse(lnglat.Split(',')[0])));
                }
                //最近的距离偏离了就算偏离
                decimal minDis = (decimal)Math.Round(minDistance.Min(), 2, MidpointRounding.AwayFromZero);
                DeviateErrorInfo entity = SelfDber.Entity<DeviateErrorInfo>(string.Format(" where TransportRecordId='{0}' and StartTime is not null and EndTime is null order by StartTime desc", item.ID));
                if (minDis >= warning.Distance)
                {
                    //偏离了
                    //首先判断存不存在有开始时间但是没有结束时间的数据，有就不管，没有则新增一条结束时间为空，开始时间为当前时间的数据，偏离距离取最小值
                    if (entity == null)
                    {
                        entity = new DeviateErrorInfo();
                        entity.TransportRecordId = item.ID;
                        entity.Distance = minDis;
                        entity.StartTime = DateTime.Now;
                        entity.Remark = string.Format("货车：{0}，在{1}偏离计划路线，偏离距离{2}米", item.CARNUMBER, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), entity.Distance);
                        if (SelfDber.Insert(entity) > 0)
                        {
                            output(string.Format("车号：{0}，运输记录ID：{1}发现路线偏离并记录，偏离距离：{2}！", item.CARNUMBER, item.ID, entity.Distance), eOutputType.Normal);
                            string updateSql = string.Format("update cmcstbbuyfueltransport t set t.ISDEVIATEEERR=1 where t.id='{0}'",item.ID);
                            SelfDber.Execute(updateSql);
                        }
                    }
                }
                else
                { 
                    //没有偏离
                    //首先判断存不存在有开始时间但是没有结束时间的数据，有就将结束时间改为当前时间，没有就不管
                    if (entity != null)
                    {
                        entity.Distance = entity.Distance > minDis ? entity.Distance : minDis;
                        entity.EndTime = DateTime.Now;
                        entity.Remark = string.Format("货车：{0}，在{1}偏离计划路线,于{2}回归计划路线，最大偏离距离{3}米", item.CARNUMBER, entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), entity.Distance);
                        if (SelfDber.Update(entity) > 0)
                        {
                            output(string.Format("车号：{0}，运输记录ID：{1}回归计划路线！", item.CARNUMBER, item.ID), eOutputType.Normal);
                            string updateSql = string.Format("update cmcstbbuyfueltransport t set t.ISDEVIATEEERR=1 where t.id='{0}'", item.ID);
                            SelfDber.Execute(updateSql);
                        }
                    }
                }
            }

        }

        private List<ShowEntity> GetInRouteCars()
        {
            var list = Dbers.GetInstance().SelfDber.Query<ShowEntity>(@"select a.id,
       a.creationtime,
       h.departtime,
       b.id as CarId,
       b.carnumber,
       c.name         as fuelkindname,
       d.name         as minename,
       e.name         as transportcompanyname,
       f.name         as supppliername,
       h.routepoints,
       a.longitude,
       a.latitude,
       a.speed,
       a.locationtime,
       a.currentlocation,
       a.isspeederr,
       a.isstoperr,
       a.isdeviateeerr
  from cmcstboutnettransport a
  left join cmcstbautotruck b on a.autotruckid = b.id
  left join fultbfuelkind c on a.fuelkindid = c.id
  left join fultbmine d on a.mineid = d.id
  left join fultbtransportcompany e on a.transportcompanyid = e.id
  left join fultbsupplier f on a.supplierid = f.id
  left join cmcstbdepartmanagedetail g on a.departmanagedetailid=g.id
  left join cmcstbdepartmanage h on g.mainid=h.id
 where a.stepname = '在途' and a.isdeleted=0 order by a.creationtime desc
").ToList();
            return list;
        }
        #endregion
    }

    public class ShowEntity
    {
        public string ID { get; set; }
        public DateTime CREATIONTIME { get; set; }
        public DateTime DEPARTTIME { get; set; }
        public String CarId { get; set; }
        public string CARNUMBER { get; set; }
        public string FUELKINDNAME { get; set; }
        public string MINENAME { get; set; }
        public string TRANSPORTCOMPANYNAME { get; set; }
        public string SUPPPLIERNAME { get; set; }
        public string ROUTEPOINTS { get; set; }
        public string LONGITUDE { get; set; }
        public string LATITUDE { get; set; }
        public string SPEED { get; set; }
        public string LOCATIONTIME { get; set; }
        public string CURRENTLOCATION { get; set; }
        public string ISSPEEDERR { get; set; }
        public string ISSTOPERR { get; set; }
        public string ISDEVIATEEERR { get; set; }

    }
}
