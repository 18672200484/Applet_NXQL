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
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.AppletConfig
{
    public partial class FrmAppletConfig_Oper : MetroForm
    {
        //业务id
        string PId = String.Empty;

        //编辑模式
        eEditMode EditMode = eEditMode.默认;

        CmcsAppletConfig cmcsAppletConfig;

        public FrmAppletConfig_Oper(string pId, eEditMode editMode)
        {
            InitializeComponent();

            this.PId = pId;
            this.EditMode = editMode;
        }

        private void FrmAppletConfig_Oper_Load(object sender, EventArgs e)
        {
            this.cmcsAppletConfig = Dbers.GetInstance().SelfDber.Get<CmcsAppletConfig>(this.PId);
            if (this.cmcsAppletConfig != null)
            {
                txt_AppIdentifier.Text = cmcsAppletConfig.AppIdentifier;
                txt_ConfigName.Text = cmcsAppletConfig.ConfigName;
                txt_ConfigValue.Text = cmcsAppletConfig.ConfigValue;
            }

            if (this.EditMode == eEditMode.查看)
            {
                btnSubmit.Enabled = false;
                HelperUtil.ControlReadOnly(panelEx2, true);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_AppIdentifier.Text))
            {
                MessageBoxEx.Show("该程序唯一标识不能为空！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_ConfigName.Text))
            {
                MessageBoxEx.Show("该配置名称不能为空！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((cmcsAppletConfig == null || cmcsAppletConfig.AppIdentifier != txt_AppIdentifier.Text || cmcsAppletConfig.ConfigName != txt_ConfigName.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsAppletConfig>(" where appIdentifier=:appIdentifier and ConfigName=:ConfigName", new { appIdentifier = txt_AppIdentifier.Text, ConfigName = txt_ConfigName.Text }).Count > 0)
            {
                MessageBoxEx.Show("该程序唯一标识中配置名称不可重复！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.EditMode == eEditMode.修改)
            {
                cmcsAppletConfig.AppIdentifier = txt_AppIdentifier.Text;
                cmcsAppletConfig.ConfigName = txt_ConfigName.Text;
                cmcsAppletConfig.ConfigValue = txt_ConfigValue.Text;
                Dbers.GetInstance().SelfDber.Update(cmcsAppletConfig);
            }
            else if (this.EditMode == eEditMode.新增)
            {
                cmcsAppletConfig = new CmcsAppletConfig();
                cmcsAppletConfig.AppIdentifier = txt_AppIdentifier.Text;
                cmcsAppletConfig.ConfigName = txt_ConfigName.Text;
                cmcsAppletConfig.ConfigValue = txt_ConfigValue.Text;
                Dbers.GetInstance().SelfDber.Insert(cmcsAppletConfig);
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
