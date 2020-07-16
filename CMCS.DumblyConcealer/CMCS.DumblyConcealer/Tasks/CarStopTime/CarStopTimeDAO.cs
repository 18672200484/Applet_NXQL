using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.Common;
using CMCS.Common.DapperDber_etc;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.CarStopTime.Entities;
using System.Data;
using CMCS.DumblyConcealer.Utilities;

namespace CMCS.DumblyConcealer.Tasks.CarStopTime
{
    public class CarStopTimeDAO
    {
          private static CarStopTimeDAO instance;

        public static CarStopTimeDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new CarStopTimeDAO();
            }
            return instance;
        }

        private CarStopTimeDAO()
        {

        }

        CommonDAO commonDAO = CommonDAO.GetInstance();
        OracleDapperDber_iEAA SelfDber = Dbers.GetInstance().SelfDber;

        #region 获取异常停留设置的时间
        public AbnormalStayWarning GetSettingTime(Action<string, eOutputType> output)
        {
            AbnormalStayWarning warning = SelfDber.Entity<AbnormalStayWarning>("where IsDeleted=0");
            if (warning == null)
            {
                output("未设置异常停留位置预警信息！", eOutputType.Error);
                return null;
            }
            if (warning.StopTime <= 0)
            {
                output("设置停车时间限制小于等于0，请正确设置！", eOutputType.Error);
                return null;
            }
            if (warning.StopNumber <= 0)
            {
                output("设置停车数量限制小于等于0，请正确设置！", eOutputType.Error);
                return null;
            }
            return warning;
        }
        #endregion

        #region 生成车辆异常停留位置数据
        /// <summary>
        /// 生成车辆异常停留位置数据
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SaveToCarStopTime(Action<string, eOutputType> output, AbnormalStayWarning warning)
        {
            //查询所有在途的车辆，然后判断该车辆最近的两次经纬度是否在同一点
            List<ShowEntity> list = GetInRouteCars();
            if (list == null || list.Count <= 0)
            {
                output("暂无在途车辆！", eOutputType.Warn);
                return;
            }
            foreach (var item in list)
            {
                item.isStop = false;
                //如果车辆已报修并且未处理就不管了
                DataTable dtIsRepair = SelfDber.ExecuteDataTable(String.Format("select count(1) from CMCSTBCARREPAIRINFO t where t.isdeleted=0 and t.repairstatus=0 and t.carid='{0}'", item.CarId));
                if (dtIsRepair != null && dtIsRepair.Rows.Count > 0 && dtIsRepair.Rows[0][0].ToString() != "0")//车辆去修理了就不管
                    continue;

                //List<CarLongLatHistory> historyList = SelfDber.Entities<CarLongLatHistory>(string.Format(" where TransportRecordId='{0}' and CreationTime>=to_date('{1}','yyyy-mm-dd hh24:mi:ss') order by CreationTime desc", item.ID, DateTime.Now.AddMinutes(-(double)warning.StopTime)));
                List<CarLongLatHistory> historyList = SelfDber.Entities<CarLongLatHistory>(string.Format(" where TransportRecordId='{0}' and CreationTime>=to_date('{1}','yyyy-mm-dd hh24:mi:ss') order by CreationTime desc", item.ID, DateTime.Parse("2020-04-10 11:05:58").AddMinutes(-(double)warning.StopTime)));
                if (historyList.Count <= 1) continue;

                historyList = historyList.OrderByDescending(a => a.CreationTime).ToList();
                var distance = GaodeHelper.GetTwoPointDistance((double)historyList.First().Latitude, (double)historyList.First().Longitude, (double)historyList.Last().Latitude, (double)historyList.Last().Longitude);
                if (distance <= 5)//距离在5米内默认没移动
                {
                    item.StopMintes = (historyList.First().CreationTime - historyList.Last().CreationTime).TotalMinutes;
                    item.LONGITUDE = historyList.First().Longitude.ToString();
                    item.LATITUDE = historyList.First().Latitude.ToString();
                    item.isStop = true;
                }
            }

            foreach (var item in list)
            {
                StopErrorInfo entity = SelfDber.Entity<StopErrorInfo>(string.Format(" where TransportRecordId='{0}' and StartTime is not null and EndTime is null order by StartTime desc", item.ID));

                //同一批次其他的车没停，百分之几的车停了那停的车就算异常

                bool isPercent=(list.Count(a => a.MineSendId == item.MineSendId && a.isStop) < (list.Count(a => a.MineSendId == item.MineSendId) * warning.StopNumber) / 100m);

                if (item.isStop && entity == null && isPercent)
                {
                    entity = new StopErrorInfo();
                    entity.TransportRecordId = item.ID;
                    entity.StopPoint = item.LONGITUDE + "," + item.LATITUDE;
                    string place = GaodeHelper.GetLocation(entity.StopPoint);
                    if (string.IsNullOrWhiteSpace(place)) continue;
                    entity.StopPlace = place;
                    entity.StopTime = decimal.Parse(item.StopMintes.ToString("F0"));
                    entity.StopTime = entity.StopTime > warning.StopTime ? entity.StopTime : warning.StopTime;
                    entity.StartTime = DateTime.Now;
                    entity.StopNum = Math.Round((list.Count(a => a.MineSendId == item.MineSendId && a.isStop) / list.Count(a => a.MineSendId == item.MineSendId)) * 100m, 2, MidpointRounding.AwayFromZero);
                    entity.Remark = string.Format("货车：{0}，于{1}开始在{2}异常停留时间超过{3}分钟！", item.CARNUMBER, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), entity.StopPlace, entity.StopTime);
                    if (SelfDber.Insert(entity) > 0)
                    {
                        output(entity.Remark, eOutputType.Normal);
                        string updateSql = string.Format("update cmcstbbuyfueltransport t set t.isstoperr=1 where t.id='{0}'", item.ID);
                        SelfDber.Execute(updateSql);
                    }
                }
                else if (entity != null)//如果没有异常停留需要将异常停留信息结束
                {
                    entity.EndTime = DateTime.Now;
                    entity.StopTime = decimal.Parse((entity.StartTime - entity.EndTime).TotalMinutes.ToString("F0"));
                    entity.Remark = string.Format("货车：{0}，于{1}至{2}在{3}异常停留{4}分钟！", item.CARNUMBER, entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss"), entity.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), entity.StopPlace, entity.StopTime);
                    if (SelfDber.Update(entity) > 0)
                    {
                        output(entity.Remark, eOutputType.Normal);
                        string updateSql = string.Format("update cmcstbbuyfueltransport t set t.isstoperr=1 where t.id='{0}'", item.ID);
                        SelfDber.Execute(updateSql);
                    }
                }

            }
        }

        private List<ShowEntity> GetInRouteCars()
        {
            var list = Dbers.GetInstance().SelfDber.Query<ShowEntity>(@"select a.id,
       a.creationtime,
       h.id as MineSendId,
       h.departtime,
       b.id as CarId,
       b.carnumber,
       c.name         as fuelkindname,
       d.name         as minename,
       e.name         as transportcompanyname,
       f.name         as supppliername,
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
 where a.stepname = '在途' and a.isdeleted=0 and h.id is not null order by a.creationtime desc
").ToList();
            return list;
        }
        #endregion
    }

    public class ShowEntity
    {
        public string ID { get; set; }
        public DateTime CREATIONTIME { get; set; }
        public string MineSendId { get; set; }
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
        public double StopMintes{get;set;}
        public bool isStop { get; set; }
    }
}
