using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.DumblyConcealer.Win.Core;
using CMCS.Common.Utilities;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.SyncNetData;

namespace CMCS.DumblyConcealer.Win.DumblyTasks
{
    public partial class FrmSyncNetData : TaskForm
    {
        RTxtOutputer rTxtOutputer;
        TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();

        string OutsideAddress = "";

        #region 方法执行完毕标识
        Boolean isMineExeFinish = true;
        Boolean isFuelKindExeFinish = true;
        Boolean isSupplierExeFinish = true;
        Boolean isTransportCompanyExeFinish = true;
        Boolean isCarFinish = true;
        Boolean isTransportPlanFinish = true;
        Boolean isCarSendFinish = true;
        #endregion

        public FrmSyncNetData()
        {
            InitializeComponent();
        }

        private void FrmSyncNetData_Load(object sender, EventArgs e)
        {
            this.rTxtOutputer = new RTxtOutputer(rtxtOutput);
            ExecuteAllTask();
        }

        /// <summary>
        /// 执行所有任务
        /// </summary>
        void ExecuteAllTask()
        {
            SyncNetDataDAO syncNetDataDAO = SyncNetDataDAO.GetInstance();

            taskSimpleScheduler.StartNewTask("获取外网地址", () =>
            {
                this.rTxtOutputer.Output("查询小程序参数配置：【外网Api请求地址】");
                OutsideAddress = syncNetDataDAO.GetOutSideAddress();
                this.rTxtOutputer.Output("外网Api请求地址：" + OutsideAddress);
            }, 10 * 60 * 1000, OutputError);

            #region 矿点
            taskSimpleScheduler.StartNewTask("矿点信息同步（单向：内网-->外网）", () =>
            {
                if (isMineExeFinish && !String.IsNullOrWhiteSpace(OutsideAddress))
                {
                    isMineExeFinish = false;
                    syncNetDataDAO.SyncMineData(OutsideAddress, this.rTxtOutputer.Output);
                    isMineExeFinish = true;
                }
            }, 10 * 1000, MineOutputError);
            #endregion

            #region 煤种
            taskSimpleScheduler.StartNewTask("煤种信息同步（单向：内网-->外网）", () =>
            {
                if (isFuelKindExeFinish && !String.IsNullOrWhiteSpace(OutsideAddress))
                {
                    isFuelKindExeFinish = false;
                    syncNetDataDAO.SyncFuelKindData(OutsideAddress, this.rTxtOutputer.Output);
                    isFuelKindExeFinish = true;
                }
            }, 10 * 1000, FuelKindOutputError);
            #endregion

            #region 供应商
            taskSimpleScheduler.StartNewTask("供应商信息同步（单向：内网-->外网）", () =>
            {
                if (isSupplierExeFinish && !String.IsNullOrWhiteSpace(OutsideAddress))
                {
                    isSupplierExeFinish = false;
                    syncNetDataDAO.SyncSupplierData(OutsideAddress, this.rTxtOutputer.Output);
                    isSupplierExeFinish = true;
                }
            }, 10 * 1000, SupplierOutputError);
            #endregion

            #region 运输单位
            taskSimpleScheduler.StartNewTask("运输单位信息同步（单向：内网-->外网）", () =>
            {
                if (isTransportCompanyExeFinish && !String.IsNullOrWhiteSpace(OutsideAddress))
                {
                    isTransportCompanyExeFinish = false;
                    syncNetDataDAO.SyncTransportCompanyData(OutsideAddress, this.rTxtOutputer.Output);
                    isTransportCompanyExeFinish = true;
                }
            }, 10 * 1000, TransportCompanyOutputError);
            #endregion

            #region 车辆管理
            taskSimpleScheduler.StartNewTask("车辆管理信息同步（单向：外网-->内网）", () =>
            {
                if (isCarFinish && !String.IsNullOrWhiteSpace(OutsideAddress))
                {
                    isCarFinish = false;
                    syncNetDataDAO.SyncCarData(OutsideAddress, this.rTxtOutputer.Output);
                    isCarFinish = true;
                }
            }, 10 * 1000, CarOutputError);
            #endregion

            #region 调运计划
            taskSimpleScheduler.StartNewTask("调运计划信息同步（单向：外网-->内网）", () =>
            {
                if (isTransportPlanFinish && !String.IsNullOrWhiteSpace(OutsideAddress))
                {
                    isTransportPlanFinish = false;
                    syncNetDataDAO.SyncTransportPlanData(OutsideAddress, this.rTxtOutputer.Output);
                    isTransportPlanFinish = true;
                }
            }, 10 * 1000, TransportPlanOutputError);
            #endregion

            #region 发车管理
            taskSimpleScheduler.StartNewTask("发车管理信息同步（单向：外网-->内网）", () =>
            {
                if (isCarSendFinish && !String.IsNullOrWhiteSpace(OutsideAddress))
                {
                    isCarSendFinish = false;
                    syncNetDataDAO.SyncCarSendData(OutsideAddress, this.rTxtOutputer.Output);
                    isCarSendFinish = true;
                }
            }, 10 * 1000, CarSendOutputError);
            #endregion
        }

        #region 异常信息输出
        /// <summary>
        /// 输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void OutputError(string text, Exception ex)
        {
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }

        /// <summary>
        /// 矿点同步输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void MineOutputError(string text, Exception ex)
        {
            this.isMineExeFinish = true;
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }

        /// <summary>
        /// 煤种同步输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void FuelKindOutputError(string text, Exception ex)
        {
            this.isFuelKindExeFinish = true;
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }

        /// <summary>
        /// 供应商同步输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void SupplierOutputError(string text, Exception ex)
        {
            this.isSupplierExeFinish = true;
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }

        /// <summary>
        /// 运输单位同步输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void TransportCompanyOutputError(string text, Exception ex)
        {
            this.isTransportCompanyExeFinish = true;
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }

        /// <summary>
        /// 车辆信息同步输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void CarOutputError(string text, Exception ex)
        {
            this.isCarFinish = true;
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }

        /// <summary>
        /// 调运计划同步输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void TransportPlanOutputError(string text, Exception ex)
        {
            this.isTransportPlanFinish = true;
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }

        /// <summary>
        /// 发车管理同步输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void CarSendOutputError(string text, Exception ex)
        {
            this.isCarSendFinish = true;
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }
        #endregion

        /// <summary>
        /// 窗体关闭后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSyncNetData_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 注意：必须取消任务
            this.taskSimpleScheduler.Cancal();
        }
    }
}

