using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.Queue.Enums;
using CMCS.Common;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.ProvinceAbbreviation
{
    public partial class FrmProvinceAbbreviation_Oper : MetroForm
    {
        //业务id
        string PId = String.Empty;

        //编辑模式
        eEditMode EditMode = eEditMode.默认;

        CmcsProvinceAbbreviation cmcsProvinceAbbreviation;

        public FrmProvinceAbbreviation_Oper(string pId, eEditMode editMode)
        {
            InitializeComponent();

            this.PId = pId;
            this.EditMode = editMode;
        }

        private void FrmProvinceAbbreviation_Oper_Load(object sender, EventArgs e)
        {
            this.cmcsProvinceAbbreviation = Dbers.GetInstance().SelfDber.Get<CmcsProvinceAbbreviation>(this.PId);
            if (this.cmcsProvinceAbbreviation != null)
            {
                txt_PaName.Text = cmcsProvinceAbbreviation.PaName;
            }

            if (this.EditMode == eEditMode.查看)
            {
                btnSubmit.Enabled = false;
                HelperUtil.ControlReadOnly(panelEx2, true);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txt_PaName.Text.Length == 0)
            {
                MessageBoxEx.Show("该简称不能为空！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((cmcsProvinceAbbreviation == null || cmcsProvinceAbbreviation.PaName != txt_PaName.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsProvinceAbbreviation>(" where PaName=:PaName", new { PaName = txt_PaName.Text }).Count > 0)
            {
                MessageBoxEx.Show("该程序唯一标识中配置名称不可重复！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.EditMode == eEditMode.修改)
            {
                cmcsProvinceAbbreviation.PaName = txt_PaName.Text;
                Dbers.GetInstance().SelfDber.Update(cmcsProvinceAbbreviation);
            }
            else if (this.EditMode == eEditMode.新增)
            {
                cmcsProvinceAbbreviation = new CmcsProvinceAbbreviation();
                cmcsProvinceAbbreviation.PaName = txt_PaName.Text;
                Dbers.GetInstance().SelfDber.Insert(cmcsProvinceAbbreviation);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
