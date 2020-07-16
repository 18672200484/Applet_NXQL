using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.Queue.Enums;
using CMCS.Common;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.SupplyReceive
{
    public partial class FrmSupplyReceive_Oper : DevComponents.DotNetBar.Metro.MetroForm
    {
        #region Var

        //业务id
        string PId = String.Empty;

        //编辑模式
        eEditMode EditMode = eEditMode.默认;

        CmcsSupplyReceive CmcsSupplyReceive;

        #endregion

        public FrmSupplyReceive_Oper(string pId, eEditMode editMode)
        {
            InitializeComponent();

            this.PId = pId;
            this.EditMode = editMode;
        }

        /// <summary>
        /// 窗体加载绑定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSupplyReceive_Oper_Load(object sender, EventArgs e)
        {
            this.CmcsSupplyReceive = Dbers.GetInstance().SelfDber.Get<CmcsSupplyReceive>(this.PId);
            if (this.CmcsSupplyReceive != null)
            {
                txt_UnitName.Text = CmcsSupplyReceive.UnitName;
                chb_IsUse.Checked = (CmcsSupplyReceive.IsValid == 1);
                txt_ReMark.Text = CmcsSupplyReceive.Remark;
            }

            if (this.EditMode == eEditMode.查看)
            {
                btnSubmit.Enabled = false;
                HelperUtil.ControlReadOnly(panelEx2, true);
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_UnitName.Text))
            {
                MessageBoxEx.Show("该标单位名称不能为空！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((CmcsSupplyReceive == null || CmcsSupplyReceive.UnitName != txt_UnitName.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsSupplyReceive>(" where CardNumber=:CardNumber", new { CardNumber = txt_UnitName.Text }).Count > 0)
            {
                MessageBoxEx.Show("该标单位名称不可重复！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.EditMode == eEditMode.修改)
            {
                CmcsSupplyReceive.UnitName = txt_UnitName.Text;
                CmcsSupplyReceive.IsValid = (chb_IsUse.Checked ? 1 : 0);
                CmcsSupplyReceive.Remark = txt_ReMark.Text;
                Dbers.GetInstance().SelfDber.Update(CmcsSupplyReceive);
            }
            else if (this.EditMode == eEditMode.新增)
            {
                CmcsSupplyReceive = new CmcsSupplyReceive();
                CmcsSupplyReceive.UnitName = txt_UnitName.Text;
                CmcsSupplyReceive.IsValid = (chb_IsUse.Checked ? 1 : 0);
                CmcsSupplyReceive.Remark = txt_ReMark.Text;
                Dbers.GetInstance().SelfDber.Insert(CmcsSupplyReceive);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}