using System;
using System.Windows.Forms;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.Queue.Enums;
//
using CMCS.CarTransport.Queue.Frms.BaseInfo.Mine;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.SetSampler
{
	public partial class FrmSetSampler_Oper : DevComponents.DotNetBar.Metro.MetroForm
	{
		String id = String.Empty;
		eEditMode EditMode = eEditMode.Ĭ��;
		CmcsSetSampler noSampler;

		CmcsMine cmcsMine;
		/// <summary>
		/// ���
		/// </summary>
		public CmcsMine CmcsMine
		{
			get { return cmcsMine; }
			set
			{
				cmcsMine = value;

				if (value != null)
				{
					txt_MineName.Text = value.Name;
				}
				else
				{
					txt_MineName.ResetText();
				}
			}
		}

		public FrmSetSampler_Oper()
		{
			InitializeComponent();
		}

		public FrmSetSampler_Oper(CmcsSetSampler pId, eEditMode editMode)
		{
			InitializeComponent();
			noSampler = pId;
			this.EditMode = editMode;
		}


		private void FrmAppletLog_Oper_Load(object sender, EventArgs e)
		{
			BindSampler();

			if (this.noSampler != null)
			{
				this.CmcsMine = CommonDAO.GetInstance().SelfDber.Get<CmcsMine>(noSampler.MineId);
				dtpStartTime.Value = noSampler.StartTime;
				dtpEndTime.Value = noSampler.EndTime;
				cmbSampler.Text = noSampler.Sampler;
			}

			if (this.EditMode == eEditMode.�鿴)
			{
				btnSubmit.Enabled = false;
				HelperUtil.ControlReadOnly(panelEx2, true);
			}
		}

		private void BindSampler()
		{
			cmbSampler.Items.Add(new DataItem("#1��е������"));
			cmbSampler.Items.Add(new DataItem("#2��е������"));
			cmbSampler.Items.Add(new DataItem("#3��е������"));
			cmbSampler.SelectedIndex = 0;
		}

		private void BtnMine_Click(object sender, EventArgs e)
		{
			FrmMine_Select Frm = new FrmMine_Select("where Id!='-1' and IsStop=0");
			Frm.ShowDialog();
			if (Frm.DialogResult == DialogResult.OK)
			{
				this.CmcsMine = Frm.Output;
			}
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txt_MineName.Text))
			{
				MessageBoxEx.Show("��ѡ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (string.IsNullOrEmpty(this.dtpStartTime.Text))
			{
				MessageBoxEx.Show("��ѡ��ʼʱ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (string.IsNullOrEmpty(this.dtpEndTime.Text))
			{
				MessageBoxEx.Show("��ѡ�����ʱ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (string.IsNullOrEmpty(this.cmbSampler.Text))
			{
				MessageBoxEx.Show("��ѡ�������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (this.EditMode == eEditMode.�޸�)
			{
				noSampler.MineId = this.CmcsMine.Id;
				noSampler.MineName = this.CmcsMine.Name;
				noSampler.StartTime = this.dtpStartTime.Value;
				noSampler.EndTime = this.dtpEndTime.Value;
				noSampler.Sampler = ((DataItem)this.cmbSampler.SelectedItem).Text;
				//Dbers.GetInstance().SelfDber.Execute(string.Format("update cmcstbnosampler set mineid='{0}',minename='{1}',starttime='{2}',endtime='{3}' where id='{4}'", noSampler.MineId, noSampler.MineName, noSampler.StartTime, noSampler.EndTime, noSampler.Id));
				Dbers.GetInstance().SelfDber.Update(noSampler);
			}
			else if (this.EditMode == eEditMode.����)
			{
				noSampler = new CmcsSetSampler();
				noSampler.MineId = this.CmcsMine.Id;
				noSampler.MineName = this.CmcsMine.Name;
				noSampler.StartTime = this.dtpStartTime.Value;
				noSampler.EndTime = this.dtpEndTime.Value;
				noSampler.Sampler = ((DataItem)this.cmbSampler.SelectedItem).Text;
				CommonDAO.GetInstance().SelfDber.Insert(noSampler);
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			this.Close();
		}
	}
}