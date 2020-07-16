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
using CMCS.DumblyConcealer.Tasks.CarStopTime;
using CMCS.DumblyConcealer.Tasks.CarStopTime.Entities;

namespace CMCS.DumblyConcealer.Win.DumblyTasks
{
    public partial class FrmCarStopTime : TaskForm
    {
        RTxtOutputer rTxtOutputer;
        TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();
        Boolean isExeFinish = true;
        AbnormalStayWarning warning = null;

        public FrmCarStopTime()
        {
            InitializeComponent();
        }

        private void FrmCarStopTime_Load(object sender, EventArgs e)
        {
            this.rTxtOutputer = new RTxtOutputer(rtxtOutput);
            ExecuteAllTask();
        }

        /// <summary>
        /// 执行所有任务
        /// </summary>
        void ExecuteAllTask()
        {
            //停留位置判断：设置的停留多长时间就多长时间循环一次查询，然后比较那个车的最近两个点是不是在同一个地点，如果是的话记录下来，然后如果在途的车都在那个点停留的车数是不是超过设定值

            CarStopTimeDAO carStopTimeDAO = CarStopTimeDAO.GetInstance();

            taskSimpleScheduler.StartNewTask("获取设定值", () =>
            {
                if (isExeFinish)
                {
                    isExeFinish = false;
                    var entity = carStopTimeDAO.GetSettingTime(this.rTxtOutputer.Output);
                    if (this.warning == null)
                        this.warning = entity;
                    else if (warning.StopNumber != entity.StopNumber || warning.StopTime != entity.StopTime)
                        this.warning = entity;
                    isExeFinish = true;
                }
            }, 10 * 1000, OutputError);

            while (true)
            {
                if (warning != null)
                {
                    taskSimpleScheduler.StartNewTask("车辆异常停留位置监测", () =>
                    {
                        if (isExeFinish && warning != null)
                        {
                            isExeFinish = false;
                            carStopTimeDAO.SaveToCarStopTime(this.rTxtOutputer.Output, warning);
                            isExeFinish = true;
                        }
                    }, (int)(warning.StopTime * 60 * 1000), OutputError);
                    break;
                }
            }
        }

        /// <summary>
        /// 输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void OutputError(string text, Exception ex)
        {
            this.isExeFinish = true;
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }

        /// <summary>
        /// 窗体关闭后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCarStopTime_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 注意：必须取消任务
            this.taskSimpleScheduler.Cancal();
        }
    }
}