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
using CMCS.DumblyConcealer.Tasks.CarSpeedRoute.Entities;

namespace CMCS.DumblyConcealer.Tasks.CarDeviationRoute
{
    public class CarSpeedRouteDAO
    {
        private static CarSpeedRouteDAO instance;

        public static CarSpeedRouteDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new CarSpeedRouteDAO();
            }
            return instance;
        }

        private CarSpeedRouteDAO()
        {

        }

        CommonDAO commonDAO = CommonDAO.GetInstance();
        OracleDapperDber_iEAA SelfDber = Dbers.GetInstance().SelfDber;

        #region 生成车速异常数据
        /// <summary>
        /// 生成路线偏离数据
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SaveToCarSpeedRoute(Action<string, eOutputType> output)
        {
            int num = 0;

            //查询全部在途车辆
            List<CMCSTBBUYFUELTRANSPORT> list = this.SelfDber.Entities<CMCSTBBUYFUELTRANSPORT>("where ISFINISH = 0 and STEPNAME = '在途' and CURRENTLOCATION is not null order by STARTTIME desc", null);
            List<string> CURRENTLOCATIONS = list.Select(T => T.CURRENTLOCATION).Distinct().ToList();
            foreach (string item in CURRENTLOCATIONS)
            {
                //查询同一个路段的全部车辆
                List<CMCSTBBUYFUELTRANSPORT> cs = list.Where(t => t.CURRENTLOCATION == item).OrderBy(t => t.SPEED).ToList();
                //cs = cs.Take(0).Take(cs.Count).ToList();
                decimal AvgSpeed = cs.Average(t => t.SPEED);
         
                foreach (CMCSTBBUYFUELTRANSPORT car in cs)
                {
                    CMCSTBSPEEDERRORINFO entity = SelfDber.Entity<CMCSTBSPEEDERRORINFO>(string.Format(" where TRANSPORTRECORDID='{0}' and StartTime is not null and EndTime is null order by StartTime desc", car.Id));
                    
                    bool flag = false;
                    //循环全部车辆
                    List<CMCSTBSPEEDWARNING> SpeedWarnings = this.SelfDber.Entities<CMCSTBSPEEDWARNING>("where 1=1", null);
                    foreach (CMCSTBSPEEDWARNING cmcstbspeedwarning in SpeedWarnings)
                    {
                      
                        //循环全部车速预警设置，匹配是否有满足的路线
                        var SpeedWarningPoint = cmcstbspeedwarning.POINTS.TrimEnd('|').Split('|');
                        foreach (var Point in SpeedWarningPoint)
                        {
                            //根据车速异常设置，返回全部途径点
                            var PointArray = Point.Split(',');
                            if (PointArray.Length == 2)
                            {
                                string origin1 = PointArray[0];
                                string destination1 = PointArray[1];

                                //两个点之间少于100米，则表示是当前路线，则取出当前的车速异常预警设置zh
                                //然后检测全部同一个路段的车辆的车速

                                Double Distance = GetDistance(ToDouble(car.LONGITUDE.ToString()),
                                ToDouble(car.LATITUDE.ToString()),
                                ToDouble(origin1),
                                ToDouble(destination1));

                                //如果途径点匹配到，则检测速度是否异常，
                                if (Distance < 1000)
                                {
                                    if(!flag)
                                    {
                                        //如果小于设置的最小车速的异常
                                        if (car.SPEED < cmcstbspeedwarning.MINSPEED || ((100 - (car.SPEED / AvgSpeed) * 100.00m) > cmcstbspeedwarning.SPEEDRANGE))
                                        {
                                            //不存在，则新增
                                            if(entity == null)
                                            {
                                                CMCSTBSPEEDERRORINFO cMCSTBSPEEDERRORINFO = new CMCSTBSPEEDERRORINFO();
                                                cMCSTBSPEEDERRORINFO.TRANSPORTRECORDID = car.Id;
                                                cMCSTBSPEEDERRORINFO.SPEED = car.SPEED;
                                                cMCSTBSPEEDERRORINFO.STARTTIME = DateTime.Now;
                                                cMCSTBSPEEDERRORINFO.HIGHWAYNAME = car.CURRENTLOCATION;
                                                this.SelfDber.Insert(cMCSTBSPEEDERRORINFO);
                                            }
                                          
                                            num++;
                                            flag = true;

                                            car.ISSPEEDERR = 1;
                                            this.SelfDber.Update(car);
                                        }
                                    }
                                }
                            }
                        }

                        if(!flag)
                        {
                            //没有异常，并且存在未结束的数据，则更新结束时间， 否则不做任何处理
                            if (entity != null)
                            {
                                entity.ENDTIME = DateTime.Now;
                                this.SelfDber.Update(entity);
                            }
                        }
                    } 
                }
            }
        }

        public Double ToDouble(string str)
        {
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception d)
            {
                return 0;
            }
        }

        // 方法定义 lat,lng 
        public double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = lat1 * Math.PI / 180.0;
            double radLat2 = lat2 * Math.PI / 180.0;
            double a = radLat1 - radLat2;
            double b = lng1 * Math.PI / 180.0 - lng2 * Math.PI / 180.0;
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
            Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * 6378.137;// EARTH_RADIUS;
            s = Math.Round(s * 10000 / 10000 * 1000, 2);
            return s;
        }
        #endregion
    } 
}
