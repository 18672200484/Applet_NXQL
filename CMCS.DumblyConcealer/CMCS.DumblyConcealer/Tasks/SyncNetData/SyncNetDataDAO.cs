using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.Common.DapperDber_etc;
using CMCS.Common;
using CMCS.DumblyConcealer.Enums;
using CMCS.Common.Entities.BaseInfo;
using CMCS.DumblyConcealer.Utilities;
using Newtonsoft.Json;
using CMCS.Common.Entities.CarTransport;
using CMCS.DumblyConcealer.Tasks.SyncNetData.Entities;

namespace CMCS.DumblyConcealer.Tasks.SyncNetData
{
    public class SyncNetDataDAO
    {
        private static SyncNetDataDAO instance;

        public static SyncNetDataDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new SyncNetDataDAO();
            }
            return instance;
        }

        private SyncNetDataDAO()
        {

        }

        CommonDAO commonDAO = CommonDAO.GetInstance();
        OracleDapperDber_iEAA selfDber = Dbers.GetInstance().SelfDber;
        WebApiHelper apiHelper = new WebApiHelper();

        #region 获取外网地址
        public String GetOutSideAddress()
        {
            return commonDAO.GetAppletConfigString("后台业务处理程序", "外网Api请求地址");
        }
        #endregion

        #region 同步矿点信息
        /// <summary>
        /// 同步矿点信息
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncMineData(string OutsideAddress, Action<string, eOutputType> output)
        {
            int res = 0;
            var syncList = selfDber.EntitiesContainDeleted<CmcsMine>("where SyncFlag=0 order by Code");
            foreach (var item in syncList)
            {
                string str = apiHelper.HttpApi(OutsideAddress + "api/services/baseinfo/FuelTbMine/SyncData", JsonConvert.SerializeObject(item), "post");
                ApiBaseResult result = JsonConvert.DeserializeObject<ApiBaseResult>(str);
                if (result.success)
                {
                    item.SyncFlag = 1;
                    res += selfDber.Update(item);
                }
            }
            if (res > 0)
                output(string.Format("矿点信息同步（单向：内网-->外网） {0} 条", res), eOutputType.Normal);
        }
        #endregion

        #region 同步煤种信息
        /// <summary>
        /// 同步煤种信息
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncFuelKindData(string OutsideAddress, Action<string, eOutputType> output)
        {
            int res = 0;
            var syncList = selfDber.EntitiesContainDeleted<CmcsFuelKind>("where SyncFlag=0 order by Code");
            foreach (var item in syncList)
            {

                string str = apiHelper.HttpApi(OutsideAddress + "api/services/baseinfo/FuelKind/SyncData", JsonConvert.SerializeObject(item), "post");
                ApiBaseResult result = JsonConvert.DeserializeObject<ApiBaseResult>(str);
                if (result.success)
                {
                    item.SyncFlag = 1;
                    res += selfDber.Update(item);
                }
            }
            if (res > 0)
                output(string.Format("煤种信息同步（单向：内网-->外网） {0} 条", res), eOutputType.Normal);
        }
        #endregion

        #region 同步供应商信息
        /// <summary>
        /// 同步供应商信息
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncSupplierData(string OutsideAddress, Action<string, eOutputType> output)
        {
            int res = 0;
            var syncList = selfDber.EntitiesContainDeleted<CmcsSupplier>("where IsCheck=99 and SyncFlag=0");
            foreach (var item in syncList)
            {

                string str = apiHelper.HttpApi(OutsideAddress + "api/services/baseinfo/Supplier/SyncData", JsonConvert.SerializeObject(item), "post");
                ApiBaseResult result = JsonConvert.DeserializeObject<ApiBaseResult>(str);
                if (result.success)
                {
                    item.SyncFlag = 1;
                    res += selfDber.Update(item);
                }
            }
            if (res > 0)
                output(string.Format("供应商信息同步（单向：内网-->外网） {0} 条", res), eOutputType.Normal);
        }
        #endregion

        #region 同步运输单位信息
        /// <summary>
        /// 同步运输单位信息
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncTransportCompanyData(string OutsideAddress, Action<string, eOutputType> output)
        {
            int res = 0;
            var syncList = selfDber.EntitiesContainDeleted<CmcsTransportCompany>("where SyncFlag=0");
            foreach (var item in syncList)
            {
                string str = apiHelper.HttpApi(OutsideAddress + "api/services/baseinfo/TransportCompany/SyncData", JsonConvert.SerializeObject(item), "post");
                ApiBaseResult result = JsonConvert.DeserializeObject<ApiBaseResult>(str);
                if (result.success)
                {
                    item.SyncFlag = 1;
                    res += selfDber.Update(item);
                }
            }
            if (res > 0)
                output(string.Format("运输单位信息同步（单向：内网-->外网） {0} 条", res), eOutputType.Normal);
        }
        #endregion

        #region 同步车辆信息（外网-->内网）
        /// <summary>
        /// 同步车辆信息（外网-->内网）
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncCarData(string OutsideAddress, Action<string, eOutputType> output)
        {
            int res = 0;
            //调用接口获取需要同步到内网的数据
            string str = apiHelper.HttpApi(OutsideAddress + "api/services/baseinfo/CarManager/GetSyncList", "", "post");
            ApiListResult<CmcsAutotruck> result = JsonConvert.DeserializeObject<ApiListResult<CmcsAutotruck>>(str);
            if (result.success && result.result != null && result.result.items.Count > 0)
            {
                foreach (var item in result.result.items)
                {
                    int flag = 0;
                    item.SyncFlag = 1;
                    var entity = selfDber.EntitiesContainDeleted<CmcsAutotruck>(" where id='" + item.Id + "'").FirstOrDefault();
                    if (entity == null)
                        flag += selfDber.Insert(item);
                    else
                        flag += selfDber.Update(item);
                    if (flag > 0)//内网更新成功后调用外网的接口将数据更新为不需要再次同步
                    {
                        apiHelper.HttpApi(OutsideAddress + "api/services/baseinfo/CarManager/SyncDataCallBack", JsonConvert.SerializeObject(item), "post");
                    }
                    res += flag;
                }
            }
            if (res > 0)
                output(string.Format("车辆管理信息同步（外网-->内网） {0} 条", res), eOutputType.Normal);
        }
        #endregion

        #region 同步调运计划（外网-->内网）
        /// <summary>
        /// 同步调运计划（外网-->内网）
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncTransportPlanData(string OutsideAddress, Action<string, eOutputType> output)
        {
            int res = 0;
            //调用接口获取需要同步到内网的数据
            string str = apiHelper.HttpApi(OutsideAddress + "api/services/minesendinfo/TransportPlanInfos/GetSyncList", "", "post");
            ApiListResult<TransportPlan> result = JsonConvert.DeserializeObject<ApiListResult<TransportPlan>>(str);
            if (result.success && result.result != null && result.result.items.Count > 0)
            {
                foreach (var item in result.result.items)
                {
                    int flag = 0;
                    item.SyncFlag = 1;
                    var entity = selfDber.EntitiesContainDeleted<TransportPlan>(" where id='" + item.Id + "'").FirstOrDefault();
                    if (entity == null)
                        flag += selfDber.Insert(item);
                    else
                        flag += selfDber.Update(item);
                    if (flag > 0)//内网更新成功后调用外网的接口将数据更新为不需要再次同步
                    {
                        apiHelper.HttpApi(OutsideAddress + "api/services/minesendinfo/TransportPlanInfos/SyncDataCallBack", JsonConvert.SerializeObject(item), "post");
                    }
                    res += flag;
                }
            }
            if (res > 0)
                output(string.Format("调运计划信息同步（外网-->内网） {0} 条", res), eOutputType.Normal);
        }
        #endregion

        #region 同步发车管理（外网-->内网）
        /// <summary>
        /// 同步发车管理（外网-->内网）
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncCarSendData(string OutsideAddress, Action<string, eOutputType> output)
        {
            int res = 0;
            //调用接口获取需要同步到内网的数据
            string str = apiHelper.HttpApi(OutsideAddress + "api/services/minesendinfo/DepartManages/GetSyncList", "", "post");
            ApiListResult<DepartManage> result = JsonConvert.DeserializeObject<ApiListResult<DepartManage>>(str);
            if (result.success && result.result != null && result.result.items.Count > 0)
            {
                foreach (var item in result.result.items)
                {
                    int flag = 0;
                    item.SyncFlag = 1;
                    var entity = selfDber.EntitiesContainDeleted<DepartManage>(" where id='" + item.Id + "'").FirstOrDefault();
                    if (entity == null)
                        flag += selfDber.Insert(item);
                    else
                        flag += selfDber.UpdateDirectly(item);

                    var details = selfDber.Query<DepartManageDetail>("select * from CMCSTBDEPARTMANAGEDETAIL where MainId='" + item.Id + "'");

                    foreach (var detail in details)
                    {
                        selfDber.Delete<DepartManageDetail>(detail.Id);
                    }

                    foreach (var detail in item.CarDetails)
                    {
                        selfDber.Insert(detail);
                    }

                    if (flag > 0)//内网更新成功后调用外网的接口将数据更新为不需要再次同步
                    {
                        apiHelper.HttpApi(OutsideAddress + "api/services/minesendinfo/DepartManages/SyncDataCallBack", JsonConvert.SerializeObject(item), "post");
                    }
                    res += flag;
                }
            }
            if (res > 0)
                output(string.Format("发车管理信息同步（外网-->内网） {0} 条", res), eOutputType.Normal);
        }
        #endregion
    }
}
