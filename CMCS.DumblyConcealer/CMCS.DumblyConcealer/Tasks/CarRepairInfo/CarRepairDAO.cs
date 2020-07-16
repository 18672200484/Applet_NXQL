using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.Common.DapperDber_etc;
using CMCS.Common;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.CarSpeedRoute.Entities;
using CMCS.DumblyConcealer.Tasks.CarRepairInfo.Entities;

namespace CMCS.DumblyConcealer.Tasks.CarRepairInfo
{
    public class CarRepairDAO
    {
        private static CarRepairDAO instance;

        public static CarRepairDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new CarRepairDAO();
            }
            return instance;
        }

        private CarRepairDAO()
        {

        }

        CommonDAO commonDAO = CommonDAO.GetInstance();
        OracleDapperDber_iEAA SelfDber = Dbers.GetInstance().SelfDber;

        #region 监测车辆报修数据
        /// <summary>
        /// 监测车辆报修数据
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SaveToCarRepair(Action<string, eOutputType> output)
        {

            //查询全部在途车辆
            List<CMCSTBBUYFUELTRANSPORT> list = this.SelfDber.Entities<CMCSTBBUYFUELTRANSPORT>("where ISFINISH = 0 and STEPNAME = '在途' order by STARTTIME desc", null);
            foreach (var item in list)
            {

                CarRepair entity = SelfDber.Entity<CarRepair>(string.Format(" where CARID='{0}' and REPAIRSTATUS=0", item.AUTOTRUCKID));
                if (entity != null)
                {
                    item.ISREPAIRERR = 1;
                    this.SelfDber.Update(item);
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

        #endregion
    } 
}
