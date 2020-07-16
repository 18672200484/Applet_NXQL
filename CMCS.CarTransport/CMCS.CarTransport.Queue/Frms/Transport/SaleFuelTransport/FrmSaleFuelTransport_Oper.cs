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
using CMCS.CarTransport.Queue.Frms.BaseInfo.Supplier;
using CMCS.CarTransport.Queue.Frms.BaseInfo.TransportCompany;
using CMCS.Common;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Enums;
using DevComponents.DotNetBar;

namespace CMCS.CarTransport.Queue.Frms.Transport.SaleFuelTransport
{
    public partial class FrmSaleFuelTransport_Oper : DevComponents.DotNetBar.Metro.MetroForm
    {
        #region Var

        //业务id
        string PId = String.Empty;

        //编辑模式
        eEditMode EditMode = eEditMode.默认;

        CmcsSaleFuelTransport CmcsSaleFuelTransport;

        CmcsTransportSales cmcsTransportSales;

        CmcsSupplier cmcsSupplier;
        /// <summary>
        /// 供应商
        /// </summary>
        public CmcsSupplier CmcsSupplier
        {
            get { return cmcsSupplier; }
            set
            {
                cmcsSupplier = value;

                if (value != null)
                {
                    txt_SupplierName.Text = value.Name;
                }
                else
                {
                    txt_SupplierName.ResetText();
                }
            }
        }

        CmcsFuelKind cmcsFuelKind;
        /// <summary>
        /// 煤种
        /// </summary>
        public CmcsFuelKind CmcsFuelKind
        {
            get { return cmcsFuelKind; }
            set { cmcsFuelKind = value; }
        }

        CmcsTransportCompany cmcsTransportCompany;
        /// <summary>
        /// 运输单位
        /// </summary>
        public CmcsTransportCompany CmcsTransportCompany
        {
            get { return cmcsTransportCompany; }
            set
            {
                cmcsTransportCompany = value;

                if (value != null)
                {
                    txt_TransportCompanyName.Text = value.Name;
                }
                else
                {
                    txt_TransportCompanyName.ResetText();
                }
            }
        }

        #endregion

        public FrmSaleFuelTransport_Oper()
        {
            InitializeComponent();
        }

        public FrmSaleFuelTransport_Oper(String pId, eEditMode editMode)
        {
            InitializeComponent();

            this.PId = pId;
            this.EditMode = editMode;
        }

        private void FrmSaleFuelTransport_Oper_Load(object sender, EventArgs e)
        {
            //绑定煤种信息
            cmbFuelName_SaleFuel.DisplayMember = "Name";
            cmbFuelName_SaleFuel.ValueMember = "Id";
            cmbFuelName_SaleFuel.DataSource = Dbers.GetInstance().SelfDber.Entities<CmcsFuelKind>("where IsStop=0 and ParentId is not null");
            cmbFuelName_SaleFuel.SelectedIndex = 0;

            //绑定流程节点
            BindStepName();

            this.CmcsSaleFuelTransport = Dbers.GetInstance().SelfDber.Get<CmcsSaleFuelTransport>(this.PId);
            if (this.CmcsSaleFuelTransport != null)
            {
                if (!String.IsNullOrEmpty(this.CmcsSaleFuelTransport.TransportSalesId))
                {
                    cmcsTransportSales = Dbers.GetInstance().SelfDber.Get<CmcsTransportSales>(this.CmcsSaleFuelTransport.TransportSalesId);
                }
                txt_SerialNumber.Text = CmcsSaleFuelTransport.SerialNumber;
                txt_CarNumber.Text = CmcsSaleFuelTransport.CarNumber;
                txt_SupplierName.Text = CmcsSaleFuelTransport.SupplierName;
                txt_LoadArea.Text = CmcsSaleFuelTransport.LoadArea;
                txt_TransportCompanyName.Text = CmcsSaleFuelTransport.TransportCompanyName;
                dbi_GrossWeight.Value = (double)CmcsSaleFuelTransport.GrossWeight;
                dbi_TareWeight.Value = (double)CmcsSaleFuelTransport.TareWeight;
                dbi_SuttleWeight.Value = (double)CmcsSaleFuelTransport.SuttleWeight;
                txt_InFactoryTime.Text = CmcsSaleFuelTransport.InFactoryTime.Year == 1 ? "" : CmcsSaleFuelTransport.InFactoryTime.ToString();
                txt_GrossTime.Text = CmcsSaleFuelTransport.GrossTime.Year == 1 ? "" : CmcsSaleFuelTransport.GrossTime.ToString();
                txt_TareTime.Text = CmcsSaleFuelTransport.TareTime.Year == 1 ? "" : CmcsSaleFuelTransport.TareTime.ToString();
                txt_OutFactoryTime.Text = CmcsSaleFuelTransport.OutFactoryTime.Year == 1 ? "" : CmcsSaleFuelTransport.OutFactoryTime.ToString();
                txt_LoadTime.Text = CmcsSaleFuelTransport.LoadTime.Year == 1 ? "" : CmcsSaleFuelTransport.LoadTime.ToString();
                txt_Remark.Text = CmcsSaleFuelTransport.Remark;
                chb_IsFinish.Checked = (CmcsSaleFuelTransport.IsFinish == 1);
                chb_IsUse.Checked = (CmcsSaleFuelTransport.IsUse == 1);
                cmbStepName.Text = CmcsSaleFuelTransport.StepName;
            }

            if (this.EditMode == eEditMode.查看)
            {
                btnSubmit.Enabled = false;
                HelperUtil.ControlReadOnly(panelEx2, true);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txt_SerialNumber.Text.Length == 0)
            {
                MessageBoxEx.Show("该标车牌号不能为空！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((CmcsSaleFuelTransport == null || CmcsSaleFuelTransport.CarNumber != txt_SerialNumber.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsSaleFuelTransport>(" where CarNumber=:CarNumber", new { CarNumber = txt_SerialNumber.Text }).Count > 0)
            {
                MessageBoxEx.Show("该标车牌号不可重复！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.EditMode == eEditMode.修改)
            {
                CmcsSaleFuelTransport.SerialNumber = txt_SerialNumber.Text;
                CmcsSaleFuelTransport.CarNumber = txt_CarNumber.Text;
                if (cmcsSupplier != null)
                {
                    CmcsSaleFuelTransport.SupplierId = cmcsSupplier.Id;
                    CmcsSaleFuelTransport.SupplierName = cmcsSupplier.Name;
                }
                if (cmcsTransportCompany != null)
                {
                    CmcsSaleFuelTransport.TransportCompanyId = cmcsTransportCompany.Id;
                    CmcsSaleFuelTransport.TransportCompanyName = cmcsTransportCompany.Name;
                }
                if (cmcsFuelKind != null)
                {
                    CmcsSaleFuelTransport.FuelKindId = cmcsFuelKind.Id;
                    CmcsSaleFuelTransport.FuelKindName = cmcsFuelKind.Name;
                }
                CmcsSaleFuelTransport.GrossWeight = (decimal)dbi_GrossWeight.Value;
                CmcsSaleFuelTransport.TareWeight = (decimal)dbi_TareWeight.Value;
                CmcsSaleFuelTransport.SuttleWeight = (decimal)dbi_SuttleWeight.Value;
                CmcsSaleFuelTransport.Remark = txt_Remark.Text;
                CmcsSaleFuelTransport.IsFinish = (chb_IsFinish.Checked ? 1 : 0);
                CmcsSaleFuelTransport.IsUse = (chb_IsUse.Checked ? 1 : 0);
                CmcsSaleFuelTransport.StepName = cmbStepName.Text;
                if (cmcsTransportSales != null)
                {
                    CmcsSaleFuelTransport.SupplierId = cmcsTransportSales.SaleSorderId;
                    CmcsSaleFuelTransport.SupplierName = cmcsTransportSales.Consignee;
                    CmcsSaleFuelTransport.TransportCompanyId = cmcsTransportSales.TransportCompayId;
                    CmcsSaleFuelTransport.TransportCompanyName = cmcsTransportSales.TransportCompayName;
                }

                CmcsUnFinishTransport unfinishTransport = Dbers.GetInstance().SelfDber.Entity<CmcsUnFinishTransport>(" where TransportId= '" + CmcsSaleFuelTransport.Id + "'");

                //有效并且未完成时需要存在[未完成运输记录]
                if (chb_IsUse.Checked && !chb_IsFinish.Checked)
                {
                    if (unfinishTransport == null)
                    {
                        unfinishTransport = new CmcsUnFinishTransport()
                        {
                            TransportId = CmcsSaleFuelTransport.Id,
                            CarType = eCarType.销售煤.ToString(),
                            AutotruckId = CmcsSaleFuelTransport.AutotruckId,
                            PrevPlace = CommonAppConfig.GetInstance().AppIdentifier
                        };
                        Dbers.GetInstance().SelfDber.Insert(unfinishTransport);
                    }
                }
                //无效或者是完成时需要删除[未完成运输记录]
                if (!chb_IsUse.Checked || chb_IsFinish.Checked)
                {
                    if (unfinishTransport != null)
                        Dbers.GetInstance().SelfDber.Delete<CmcsUnFinishTransport>(unfinishTransport.Id);
                }

                Dbers.GetInstance().SelfDber.Update(CmcsSaleFuelTransport);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnMine_Click(object sender, EventArgs e)
        {
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select Frm = new FrmSupplier_Select(string.Empty);
            Frm.ShowDialog();
            if (Frm.DialogResult == DialogResult.OK)
            {
                this.CmcsSupplier = Frm.Output;
            }
        }

        private void btnTransportCompany_Click(object sender, EventArgs e)
        {
            FrmTransportCompany_Select Frm = new FrmTransportCompany_Select(string.Empty);
            Frm.ShowDialog();
            if (Frm.DialogResult == DialogResult.OK)
            {
                this.CmcsTransportCompany = Frm.Output;
            }
        }

        private void btnSelectForecast_SaleFuel_Click(object sender, EventArgs e)
        {

            FrmSaleFuelForecast_Select frm = new FrmSaleFuelForecast_Select();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                cmcsTransportSales = frm.Output;
                txt_SupplierName.Text = frm.Output.Consignee;
                txt_TransportCompanyName.Text = frm.Output.TransportCompayName;
            }
        }

        private void cmbFuelName_BuyFuel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmcsFuelKind = cmbFuelName_SaleFuel.SelectedItem as CmcsFuelKind;
        }

        /// <summary>
        /// 绑定流程节点
        /// </summary>
        private void BindStepName()
        {
            cmbStepName.Items.Add(eTruckInFactoryStep.入厂.ToString());
            cmbStepName.Items.Add(eTruckInFactoryStep.轻车.ToString());
            cmbStepName.Items.Add(eTruckInFactoryStep.重车.ToString());
            cmbStepName.Items.Add(eTruckInFactoryStep.出厂.ToString());
            cmbStepName.SelectedIndex = 0;
        }
    }
}