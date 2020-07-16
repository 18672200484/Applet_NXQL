using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.Common;
using CMCS.Common.Utilities;
using CMCS.DataTester.Core;
using CMCS.DumblyConcealer.Enums;

namespace CMCS.DataTester.Frms
{
    public partial class FrmCarLocation : Form
    {
        public FrmCarLocation()
        {
            InitializeComponent();
        }

        Boolean isInsertData = false;
        RTxtOutputer rTxtOutputer;
        TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();

        /// <summary>
        /// 窗体加载的时候获取所有状态为在途的车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCarLocation_Load(object sender, EventArgs e)
        {
            this.rTxtOutputer = new RTxtOutputer(this.errorMsg);
            var list = Dbers.GetInstance().SelfDber.Query<ShowEntity>(@"select a.id,
       a.creationtime,
       h.departtime,
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
            this.dataGridView1.DataSource = list;
        }

        /// <summary>
        /// 开始模拟数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            int interval = 30;
            if (string.IsNullOrWhiteSpace(txtInterval.Text.Trim()))
            {
                PrintError("请输入数据生成时间间隔！");
                return;
            }
            else {
                if (!int.TryParse(txtInterval.Text.Trim(), out interval))
                {
                    PrintError("时间间隔数字格式错误！");
                    return;
                }
                else if (interval < 3)
                {
                    PrintError("时间间隔最少3秒！");
                    return;
                }
            }
            List<ShowEntity> list = new List<ShowEntity>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value))
                {
                    var entity = dataGridView1.Rows[i].DataBoundItem as ShowEntity;
                    list.Add(entity);
                }
            }
            if (list.Count <= 0)
            {
                PrintError("请至少选择一条数据进行生成！");
                return;
            }
            isInsertData = true;
            taskSimpleScheduler.StartNewTask("生成实时数据", () =>
            {
                if (isInsertData)
                {
                    foreach (var item in list)
                    {
                        if (string.IsNullOrWhiteSpace(item.ROUTEPOINTS))
                        {
                            this.rTxtOutputer.Output(string.Format("车号：{0}，运输记录ID：{1}对应的发车未设置线路！", item.CARNUMBER, item.ID),eOutputType.Error);
                            continue;
                        }
                        string[] lnglats = item.ROUTEPOINTS.Trim('|').Split('|');
                        DataTable dt = Dbers.GetInstance().SelfDber.ExecuteDataTable("select count(1) from CMCSTBLONGITUDEANDLATITUDE t where t.transportrecordid='" + item.ID + "'");
                        int index = 0;
                        if (dt != null && dt.Rows.Count > 0)
                            index = int.Parse(dt.Rows[0][0].ToString());
                        if(index>=lnglats.Length) continue;
                        string insertSql = string.Format(@"insert into CMCSTBLONGITUDEANDLATITUDE (ID, CREATIONTIME, CREATORUSERID, ISDELETED, TRANSPORTRECORDID, LONGITUDE, LATITUDE)
values ('{0}', to_date('{1}','yyyy-mm-dd hh24:mi:ss'), 1, 0, '{2}', {3}, {4})", Guid.NewGuid().ToString(), item.DEPARTTIME.AddSeconds(index * 10).ToString("yyyy-MM-dd HH:mm:ss"), item.ID, lnglats[index].Split(',')[0], lnglats[index].Split(',')[1]);
                        if (Dbers.GetInstance().SelfDber.Execute(insertSql) > 0)
                        {
                            this.rTxtOutputer.Output(string.Format("车号：{0}插入历史记录！", item.CARNUMBER));
                            string updateSql = string.Format(@" update cmcstboutnettransport a set a.longitude='{0}', a.latitude='{1}',a.locationtime=to_date('{2}','yyyy-mm-dd hh24:mi:ss') where a.id='{3}'", lnglats[index].Split(',')[0], lnglats[index].Split(',')[1], DateTime.Now, item.ID);
                            if (Dbers.GetInstance().SelfDber.Execute(updateSql) > 0)
                            {
                                this.rTxtOutputer.Output(string.Format("车号：{0}更新实时经纬度！", item.CARNUMBER));
                            //根据经纬度查询位置
                                string result=GaodeHelper.GetLocation(lnglats[index]);
                                if (!String.IsNullOrWhiteSpace(result))
                                {
                                    updateSql = string.Format(@" update cmcstboutnettransport a set a.currentlocation='{0}' where a.id='{1}'", result, item.ID);
                                    Dbers.GetInstance().SelfDber.Execute(updateSql);
                                    this.rTxtOutputer.Output(string.Format("车号：{0}根据接口查询更新实时位置！", item.CARNUMBER));
                                }
                            }
                        }
                    }
                }

            }, interval*1000, OutputError);
        }

        /// <summary>
        /// 停止模拟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnd_Click(object sender, EventArgs e)
        {
            isInsertData = false;
            taskSimpleScheduler.Cancal();
        }
        /// <summary>
        /// 选定车辆偏离路线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDfrit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("将车辆经纬度历史记录表中的随便选一条数据调整下经纬度即可！");
        }
        /// <summary>
        /// 选定车辆车速异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSpeed_Click(object sender, EventArgs e)
        {
            //车速异常（将3分钟内的数据中的时间间隔改大）
            //List<ShowEntity> list = new List<ShowEntity>();
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value))
            //    {
            //        var entity = dataGridView1.Rows[i].DataBoundItem as ShowEntity;
            //        list.Add(entity);
            //    }
            //}
            //if (list.Count <= 0)
            //{
            //    PrintError("请至少选择一条数据进行模拟！");
            //    return;
            //}
            //try
            //{
            //}
            //catch (Exception ex)
            //{
            //    OutputError("", ex);
            //}
            MessageBox.Show("将车辆经纬度历史记录表中的时间间隔改下即可！");
        }
        /// <summary>
        /// 选定车辆异常停留
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            //将最新的10分钟内的该运输记录的经纬度改成一样的
            List<ShowEntity> list = new List<ShowEntity>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value))
                {
                    var entity = dataGridView1.Rows[i].DataBoundItem as ShowEntity;
                    list.Add(entity);
                }
            }
            if (list.Count <= 0)
            {
                PrintError("请至少选择一条数据进行模拟！");
                return;
            }
            try
            {
                foreach (var item in list)
                {
                    DataTable dt = Dbers.GetInstance().SelfDber.ExecuteDataTable("select a.* from (select t.* from CMCSTBLONGITUDEANDLATITUDE t where t.transportrecordid='" + item.ID + "' order by t.creationtime desc) a where rownum=1");
                    if (dt == null || dt.Rows.Count <= 0) continue;
                    string updateSql = string.Format("update CMCSTBLONGITUDEANDLATITUDE t set t.LONGITUDE='{0}',t.LATITUDE='{1}' where t.transportrecordid='{2}' and t.CREATIONTIME>=to_date('{3}','yyyy-mm-dd hh24:mi:ss')", dt.Rows[0]["LONGITUDE"], dt.Rows[0]["LATITUDE"], item.ID, Convert.ToDateTime(dt.Rows[0]["CREATIONTIME"]).AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss"));
                    if (Dbers.GetInstance().SelfDber.Execute(updateSql) > 0)
                        PrintError(string.Format("车号：{0}，运输记录ID：{1}异常停留设置成功！", item.CARNUMBER, item.ID));
                }
            }
            catch (Exception ex)
            {
                OutputError("",ex);
            }
            
        }

        private void PrintError(String error)
        {
            if (this.errorMsg.TextLength > 100000)
                this.errorMsg.Text = error + "\n";
            else
                this.errorMsg.Text += error + "\n";
        }

        /// <summary>
        /// 输出异常信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        void OutputError(string text, Exception ex)
        {
            this.rTxtOutputer.Output(text + Environment.NewLine + ex.Message, eOutputType.Error);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.errorMsg.Text = "";
        }
    }

    public class ShowEntity
    {
        public string ID { get; set; }
        public DateTime CREATIONTIME { get; set; }
        public DateTime DEPARTTIME { get; set; }
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
