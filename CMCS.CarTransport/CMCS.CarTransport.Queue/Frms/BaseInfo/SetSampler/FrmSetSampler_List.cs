using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using CMCS.Common;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar.SuperGrid;
using CMCS.Common.DAO;
using CMCS.CarTransport.Queue.Core;
using CMCS.Common.Enums;
using CMCS.Common.Entities.Sys;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.SetSampler
{
	public partial class FrmSetSampler_List : MetroAppForm
	{
		/// <summary>
		/// ����Ψһ��ʶ��
		/// </summary>
		public static string UniqueKey = "FrmSetSampler_List";

		/// <summary>
		/// ÿҳ��ʾ����
		/// </summary>
		int PageSize = 18;

		/// <summary>
		/// ��ҳ��
		/// </summary>
		int PageCount = 0;

		/// <summary>
		/// �ܼ�¼��
		/// </summary>
		int TotalCount = 0;

		/// <summary>
		/// ��ǰҳ����
		/// </summary>
		int CurrentIndex = 0;

		string SqlWhere = string.Empty;

		public FrmSetSampler_List()
		{
			InitializeComponent();
		}

		private void FrmAppletLog_List_Load(object sender, EventArgs e)
		{
			superGridControl1.PrimaryGrid.AutoGenerateColumns = false;
			dtpStartTime.Value = DateTime.Now.Date;
			dtpEndTime.Value = DateTime.Now.Date;
			btnSearch_Click(null, null);
		}

		public void BindData()
		{
			object param = new { StartTime = dtpStartTime.Value, EndTime = dtpEndTime.Value.AddDays(1) };
			string tempSqlWhere = this.SqlWhere;

			List<CmcsSetSampler> list = Dbers.GetInstance().SelfDber.ExecutePager<CmcsSetSampler>(PageSize, CurrentIndex, tempSqlWhere + " order by CreationTime desc", param);
			//List<CmcsAppletLog> list = Dbers.GetInstance().SelfDber.Entities<CmcsAppletLog>(tempSqlWhere, param);
			superGridControl1.PrimaryGrid.DataSource = list;

			GetTotalCount(tempSqlWhere, param);
			PagerControlStatue();

			lblPagerInfo.Text = string.Format("�� {0} ����¼��ÿҳ {1} ������ {2} ҳ����ǰ�� {3} ҳ", TotalCount, PageSize, PageCount, CurrentIndex + 1);
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			this.SqlWhere = " where 1=1";
			if (dtpStartTime.Value != DateTime.MinValue) this.SqlWhere += " and CreationTime >=:StartTime";
			if (dtpEndTime.Value != DateTime.MinValue) this.SqlWhere += " and CreationTime <=:EndTime";
			if (!string.IsNullOrEmpty(txtContent.Text)) this.SqlWhere += " and MineName like '%" + txtContent.Text + "%'";

			CurrentIndex = 0;
			BindData();
		}

		private void btnAll_Click(object sender, EventArgs e)
		{
			this.SqlWhere = string.Empty;
			txtContent.Text = string.Empty;

			CurrentIndex = 0;
			BindData();
		}

		#region Pager

		private void btnPagerCommand_Click(object sender, EventArgs e)
		{
			ButtonX btn = sender as ButtonX;
			switch (btn.CommandParameter.ToString())
			{
				case "First":
					CurrentIndex = 0;
					break;
				case "Previous":
					CurrentIndex = CurrentIndex - 1;
					break;
				case "Next":
					CurrentIndex = CurrentIndex + 1;
					break;
				case "Last":
					CurrentIndex = PageCount - 1;
					break;
			}

			BindData();
		}

		public void PagerControlStatue()
		{
			if (PageCount <= 1)
			{
				btnFirst.Enabled = false;
				btnPrevious.Enabled = false;
				btnLast.Enabled = false;
				btnNext.Enabled = false;

				return;
			}

			if (CurrentIndex == 0)
			{
				// ��ҳ
				btnFirst.Enabled = false;
				btnPrevious.Enabled = false;
				btnLast.Enabled = true;
				btnNext.Enabled = true;
			}

			if (CurrentIndex > 0 && CurrentIndex < PageCount - 1)
			{
				// ��һҳ/��һҳ
				btnFirst.Enabled = true;
				btnPrevious.Enabled = true;
				btnLast.Enabled = true;
				btnNext.Enabled = true;
			}

			if (CurrentIndex == PageCount - 1)
			{
				// ĩҳ
				btnFirst.Enabled = true;
				btnPrevious.Enabled = true;
				btnLast.Enabled = false;
				btnNext.Enabled = false;
			}
		}

		private void GetTotalCount(string sqlWhere, object param)
		{
			TotalCount = Dbers.GetInstance().SelfDber.Count<CmcsSetSampler>(sqlWhere, param);
			if (TotalCount % PageSize != 0)
				PageCount = TotalCount / PageSize + 1;
			else
				PageCount = TotalCount / PageSize;
		}
		#endregion

		#region DataGridView

		private void superGridControl1_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
		{
			// ȡ���༭
			e.Cancel = true;
		}

		private void superGridControl1_CellMouseDown(object sender, DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs e)
		{
			CmcsSetSampler entity = Dbers.GetInstance().SelfDber.Get<CmcsSetSampler>(superGridControl1.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, superGridControl1.PrimaryGrid.Columns["clmId"].ColumnIndex).Value.ToString());
			switch (superGridControl1.PrimaryGrid.Columns[e.GridCell.ColumnIndex].Name)
			{
				case "clmShow":
					FrmSetSampler_Oper frmShow = new FrmSetSampler_Oper(entity, Enums.eEditMode.�鿴);
					if (frmShow.ShowDialog() == DialogResult.OK)
					{
						BindData();
					}
					break;
				case "clmEdit":
					FrmSetSampler_Oper frmShow2 = new FrmSetSampler_Oper(entity, Enums.eEditMode.�޸�);
					if (frmShow2.ShowDialog() == DialogResult.OK)
					{
						BindData();
					}
					break;
				case "clmDelete":
					if ((MessageBoxEx.Show("ȷ��Ҫɾ���ü�¼��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
					{
						CommonDAO.GetInstance().SelfDber.Delete<CmcsSetSampler>(entity.Id);
						BindData();
					}
					break;
			}
		}

		/// <summary>
		/// �����к�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void superGridControl_GetRowHeaderText(object sender, DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs e)
		{
			e.Text = (e.GridRow.RowIndex + 1).ToString();
		}

		private void superGridControl1_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
		{
			foreach (GridRow gridRow in e.GridPanel.Rows)
			{
				CmcsSetSampler entity = gridRow.DataItem as CmcsSetSampler;
			}
		}
		#endregion

		private void btnAdd_Click(object sender, EventArgs e)
		{
			FrmSetSampler_Oper frmShow = new FrmSetSampler_Oper(null, Enums.eEditMode.����);
			if (frmShow.ShowDialog() == DialogResult.OK)
			{
				BindData();
			}
		}
	}
}
