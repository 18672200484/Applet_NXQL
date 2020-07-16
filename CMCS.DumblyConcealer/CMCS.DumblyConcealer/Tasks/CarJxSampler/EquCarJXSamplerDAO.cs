using System;
using System.Collections.Generic;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Entities.Inf;
using CMCS.Common.Enums;
using CMCS.DapperDber.Dbs.SqlServerDb;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.CarJXSampler.Entities;

namespace CMCS.DumblyConcealer.Tasks.CarJXSampler
{
    /// <summary>
    /// 汽车机械采样机接口业务
    /// </summary>
    public class EquCarJXSamplerDAO
    {
        /// <summary>
        /// EquCarJXSamplerDAO
        /// </summary>
        /// <param name="machineCode">设备编码</param>
        /// <param name="equDber">第三方数据库访问对象</param>
        public EquCarJXSamplerDAO(string machineCode, SqlServerDapperDber equDber)
        {
            this.MachineCode = machineCode;
            this.EquDber = equDber;
        }

        CommonDAO commonDAO = CommonDAO.GetInstance();

        /// <summary>
        /// 第三方数据库访问对象
        /// </summary>
        SqlServerDapperDber EquDber;
        /// <summary>
        /// 设备编码
        /// </summary>
        string MachineCode;
        /// <summary>
        /// 是否处于故障状态
        /// </summary>
        bool IsHitch = false;
        /// <summary>
        /// 上一次上位机心跳值
        /// </summary>
        string PrevHeartbeat = string.Empty;

        #region 数据转换方法（此处有点麻烦，后期调整接口方案）

        #endregion

        /// <summary>
        /// 同步实时信号到集中管控
        /// </summary>
        /// <param name="output"></param>
        /// <param name="MachineCode">设备编码</param>
        /// <returns></returns>
        public int SyncSignal(Action<string, eOutputType> output)
        {
            int res = 0;

            foreach (EquQCJXCYJSignal entity in this.EquDber.Entities<EquQCJXCYJSignal>())
            {
                if (entity.TagName == GlobalVars.EquHeartbeatName) continue;

                // 当心跳检测为故障时，则不更新系统状态，保持 eSampleSystemStatus.发生故障
                if (entity.TagName == eSignalDataName.系统.ToString() && IsHitch) continue;

                res += commonDAO.SetSignalDataValue(this.MachineCode, entity.TagName, entity.TagValue) ? 1 : 0;
            }
            output(string.Format("同步实时信号 {0} 条", res), eOutputType.Normal);

            return res;
        }

        /// <summary>
        /// 获取上位机运行状态表 - 心跳值
        /// 每隔30s读取该值，如果数值不变化则表示设备上位机出现故障
        /// </summary>
        /// <param name="MachineCode">设备编码</param>
        public void SyncHeartbeatSignal()
        {
            EquQCJXCYJSignal pDCYSignal = this.EquDber.Entity<EquQCJXCYJSignal>("where TagName=@TagName", new { TagName = GlobalVars.EquHeartbeatName });
            ChangeSystemHitchStatus((pDCYSignal != null && pDCYSignal.TagValue == this.PrevHeartbeat));

            this.PrevHeartbeat = pDCYSignal != null ? pDCYSignal.TagValue : string.Empty;
        }

        /// <summary>
        /// 改变系统状态值
        /// </summary>
        /// <param name="isHitch">是否故障</param> 
        public void ChangeSystemHitchStatus(bool isHitch)
        {
            IsHitch = isHitch;

            if (IsHitch) commonDAO.SetSignalDataValue(this.MachineCode, eSignalDataName.系统.ToString(), eEquInfSamplerSystemStatus.发生故障.ToString());
        }

        /// <summary>
        /// 同步集样罐信息到集中管控
        /// </summary>
        /// <param name="output"></param> 
        /// <returns></returns>
        public void SyncBarrel(Action<string, eOutputType> output)
        {
            int res = 0;

            List<EquQCJXCYJBarrel> infpdcybarrels = this.EquDber.Entities<EquQCJXCYJBarrel>();
            foreach (EquQCJXCYJBarrel entity in infpdcybarrels)
            {
                if (commonDAO.SaveEquInfSampleBarrel(new InfEquInfSampleBarrel
                {
                    BarrelNumber = entity.BarrelNumber,
                    BarrelStatus = entity.BarrelStatus.ToString(),
                    MachineCode = this.MachineCode,
                    InFactoryBatchId = entity.InFactoryBatchId,
                    InterfaceType = commonDAO.GetMachineInterfaceTypeByCode(this.MachineCode),
                    IsCurrent = entity.IsCurrent,
                    SampleCode = entity.SampleCode,
                    SampleCount = entity.SampleCount,
                    UpdateTime = entity.UpdateTime,
                    BarrelType = entity.BarrelType,
                }))
                {

                    entity.DataFlag = 1;
                    this.EquDber.Update(entity);

                    res++;
                }
            }

            output(string.Format("同步集样罐记录 {0} 条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步故障信息到集中管控
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncQCJXCYJError(Action<string, eOutputType> output)
        {
            int res = 0;

            foreach (EquQCJXCYJError entity in this.EquDber.Entities<EquQCJXCYJError>("where DataFlag=0"))
            {
                if (commonDAO.SaveEquInfHitch(this.MachineCode, entity.ErrorTime, "故障代码 " + entity.ErrorCode + "，" + entity.ErrorDescribe))
                {
                    entity.DataFlag = 1;
                    this.EquDber.Update(entity);

                    res++;
                }
            }

            output(string.Format("同步故障信息记录 {0} 条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步采样命令
        /// </summary>
        /// <param name="output"></param>
        /// <param name="MachineCode">设备编码</param>
        public void SyncSampleCmd(Action<string, eOutputType> output)
        {
            int res = 0;

            // 集中管控 > 第三方 
            foreach (InfQCJXCYSampleCMD entity in CarSamplerDAO.GetInstance().GetWaitForSyncSampleCMD(this.MachineCode))
            {
                bool isSuccess = false;
                // 需调整：命令中的水分等信息视接口而定
                EquQCJXCYJSampleCmd samplecmdEqu = this.EquDber.Get<EquQCJXCYJSampleCmd>(entity.Id);
                if (samplecmdEqu == null)
                {
                    isSuccess = this.EquDber.Insert(new EquQCJXCYJSampleCmd
                    {
                        // 保持相同的Id
                        Id = entity.Id,
                        CarNumber = entity.CarNumber,
                        InFactoryBatchId = entity.InFactoryBatchId,
                        SampleCode = entity.SampleCode,
                        //Mt = 0,
                        TicketWeight = entity.TicketWeight,
                        CarCount = entity.CarCount,
                        PointCount = entity.PointCount,
                        Point1 = entity.Point1,
                        Point2 = entity.Point2,
                        Point3 = entity.Point3,
                        Point4 = entity.Point4,
                        Point5 = entity.Point5,
                        Point6 = entity.Point6,
                        CarriageLength = entity.CarriageLength,
                        CarriageWidth = entity.CarriageWidth,
                        CarriageBottomToFloor = entity.CarriageBottomToFloor,
                        Obstacle1 = entity.Obstacle1,
                        Obstacle2 = entity.Obstacle2,
                        Obstacle3 = entity.Obstacle3,
                        Obstacle4 = entity.Obstacle4,
                        Obstacle5 = entity.Obstacle5,
                        Obstacle6 = entity.Obstacle6,
                        StartTime = entity.StartTime,
                        EndTime = entity.EndTime,
                        SampleUser = entity.SampleUser,
                        ResultCode = entity.ResultCode,
                        DataFlag = 0
                    }) > 0;
                }
                else
                {
                    samplecmdEqu.CarNumber = entity.CarNumber;
                    samplecmdEqu.InFactoryBatchId = entity.InFactoryBatchId;
                    samplecmdEqu.SampleCode = entity.SampleCode;
                    //samplecmdEqu.Mt = 0;
                    samplecmdEqu.TicketWeight = entity.TicketWeight;
                    samplecmdEqu.CarCount = entity.CarCount;
                    samplecmdEqu.PointCount = entity.PointCount;
                    samplecmdEqu.Point1 = entity.Point1;
                    samplecmdEqu.Point2 = entity.Point2;
                    samplecmdEqu.Point3 = entity.Point3;
                    samplecmdEqu.Point4 = entity.Point4;
                    samplecmdEqu.Point5 = entity.Point5;
                    samplecmdEqu.Point6 = entity.Point6;
                    samplecmdEqu.CarriageLength = entity.CarriageLength;
                    samplecmdEqu.CarriageWidth = entity.CarriageWidth;
                    samplecmdEqu.CarriageBottomToFloor = entity.CarriageBottomToFloor;
                    samplecmdEqu.Obstacle1 = entity.Obstacle1;
                    samplecmdEqu.Obstacle2 = entity.Obstacle2;
                    samplecmdEqu.Obstacle3 = entity.Obstacle3;
                    samplecmdEqu.Obstacle4 = entity.Obstacle4;
                    samplecmdEqu.Obstacle5 = entity.Obstacle5;
                    samplecmdEqu.Obstacle6 = entity.Obstacle6;
                    samplecmdEqu.StartTime = entity.StartTime;
                    samplecmdEqu.EndTime = entity.EndTime;
                    samplecmdEqu.SampleUser = entity.SampleUser;
                    samplecmdEqu.ResultCode = entity.ResultCode;
                    samplecmdEqu.DataFlag = 0;
                    isSuccess = this.EquDber.Update(samplecmdEqu) > 0;
                }

                if (isSuccess)
                {
                    entity.SyncFlag = 1;
                    Dbers.GetInstance().SelfDber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步采样计划 {0} 条（集中管控 > 第三方）", res), eOutputType.Normal);


            res = 0;
            // 第三方 > 集中管控
            foreach (EquQCJXCYJSampleCmd entity in this.EquDber.Entities<EquQCJXCYJSampleCmd>("where DataFlag=2 and datediff(dd,CreationTime,getdate())=0"))
            {
                InfQCJXCYSampleCMD samplecmdInf = Dbers.GetInstance().SelfDber.Get<InfQCJXCYSampleCMD>(entity.Id);
                if (samplecmdInf == null) continue;

                samplecmdInf.Point1 = entity.Point1;
                samplecmdInf.Point2 = entity.Point2;
                samplecmdInf.Point3 = entity.Point3;
                samplecmdInf.Point4 = entity.Point4;
                samplecmdInf.Point5 = entity.Point5;
                samplecmdInf.Point6 = entity.Point6;
                samplecmdInf.StartTime = entity.StartTime;
                samplecmdInf.EndTime = entity.EndTime;
                samplecmdInf.SampleUser = entity.SampleUser;
                samplecmdInf.ResultCode = entity.ResultCode;

                if (Dbers.GetInstance().SelfDber.Update(samplecmdInf) > 0)
                {
                    // 我方已读
                    entity.DataFlag = 3;
                    this.EquDber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步采样计划 {0} 条（第三方 > 集中管控）", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步卸样命令
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncJXCYControlUnloadCMD(Action<string, eOutputType> output)
        {
            int res = 0;

            // 集中管控 > 第三方
            foreach (InfQCJXCYUnLoadCMD entity in CarSamplerDAO.GetInstance().GetWaitForSyncJXCYSampleUnloadCmd(MachineCode))
            {
                bool isSuccess = false;

                EquQCJXCYJUnloadCmd pJXCYCMD = this.EquDber.Get<EquQCJXCYJUnloadCmd>(entity.Id);
                if (pJXCYCMD == null)
                {
                    isSuccess = this.EquDber.Insert(new EquQCJXCYJUnloadCmd
                    {
                        // 保持相同的Id
                        Id = entity.Id,
                        DataFlag = 0,
                        CreationTime = entity.CreationTime,
                        SampleCode = entity.SampleCode,
                        ResultCode = eEquInfCmdResultCode.默认.ToString(),
                        UnLoadType = entity.UnLoadType.ToString(),
                        SamplingId = entity.SamplingId
                    }) > 0;
                }
                else isSuccess = true;

                if (isSuccess)
                {
                    entity.SyncFlag = 1;
                    Dbers.GetInstance().SelfDber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步卸样命令 {0} 条（集中管控 > 第三方）", res), eOutputType.Normal);

            res = 0;
            // 第三方 > 集中管控
            foreach (EquQCJXCYJUnloadCmd entity in this.EquDber.Entities<EquQCJXCYJUnloadCmd>("where DataFlag=2 and datediff(dd,CreationTime,getdate())=0"))
            {
                InfQCJXCYUnLoadCMD JXCYCmd = Dbers.GetInstance().SelfDber.Get<InfQCJXCYUnLoadCMD>(entity.Id);
                if (JXCYCmd == null) continue;

                // 更新执行结果等
                JXCYCmd.ResultCode = entity.ResultCode;

                if (Dbers.GetInstance().SelfDber.Update(JXCYCmd) > 0)
                {
                    // 我方已读
                    entity.DataFlag = 3;
                    this.EquDber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步卸样命令 {0} 条（第三方 > 集中管控）", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步历史卸样结果
        /// </summary>
        /// <param name="output"></param>
        /// <param name="MachineCode"></param>
        public void SyncUnloadResult(Action<string, eOutputType> output)
        {
            int res = 0;

            res = 0;
            // 第三方 > 集中管控
            foreach (EquQCJXCYJUnloadResult entity in this.EquDber.Entities<EquQCJXCYJUnloadResult>("where DataFlag=0"))
            {
                InfQCJXCYJUnloadResult oldUnloadResult = commonDAO.SelfDber.Get<InfQCJXCYJUnloadResult>(entity.Id);
                if (oldUnloadResult == null)
                {
                    // 查找采样命令
                    EquQCJXCYJSampleCmd qCJXCYJSampleCmd = this.EquDber.Entity<EquQCJXCYJSampleCmd>("where SampleCode=@SampleCode", new { SampleCode = entity.SampleCode });
                    if (qCJXCYJSampleCmd != null)
                    {
                        // 生成采样桶记录
                        CmcsRCSampleBarrel rCSampleBarrel = new CmcsRCSampleBarrel()
                            {
                                BarrelCode = entity.BarrelCode,
                                BarrellingTime = entity.UnloadTime,
                                BarrelNumber = entity.BarrelNumber,
                                InFactoryBatchId = qCJXCYJSampleCmd.InFactoryBatchId,
                                SamplerName = commonDAO.GetMachineNameByCode(this.MachineCode),
                                SampleType = eSamplingType.机械采样.ToString(),
                                SamplingId = entity.SamplingId
                            };

                        if (commonDAO.SelfDber.Insert(rCSampleBarrel) > 0)
                        {
                            if (commonDAO.SelfDber.Insert(new InfQCJXCYJUnloadResult
                            {
                                SampleCode = entity.SampleCode,
                                BarrelCode = entity.BarrelCode,
                                UnloadTime = entity.UnloadTime,
                                DataFlag = entity.DataFlag
                            }) > 0)
                            {
                                entity.DataFlag = 1;
                                this.EquDber.Update(entity);

                                res++;
                            }
                        }
                    }
                }
            }
            output(string.Format("同步卸样结果 {0} 条（第三方 > 集中管控）", res), eOutputType.Normal);
        }
    }
}
