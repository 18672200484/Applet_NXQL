using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CMCS.CarTransport.DAO;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.Queue.Enums;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;

namespace CMCS.CarTransport.Queue.Frms.BaseInfo.FuelKind
{
    public partial class FrmFuelKind_List : DevComponents.DotNetBar.Metro.MetroForm
    {
        #region Var

        /// <summary>
        /// ����Ψһ��ʶ��
        /// </summary>
        public static string UniqueKey = "FrmFuelKind_List";

        /// <summary>
        /// ѡ�е�ʵ��
        /// </summary>
        public CmcsFuelKind SelFuelKind;

        /// <summary>
        /// ��ǰ�������ģʽ
        /// </summary>
        private eEditMode CurrEditMode = eEditMode.Ĭ��;

        CommonDAO commonDAO = CommonDAO.GetInstance();

        #endregion

        public FrmFuelKind_List()
        {
            InitializeComponent();
        }

        private void FrmFuelKind_List_Shown(object sender, EventArgs e)
        {
            InitTree();

            //01�鿴 02���� 03�޸� 04ɾ��
            btnAdd.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "02", GlobalVars.LoginUser);
            btnUpdate.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "03", GlobalVars.LoginUser);
            btnDelete.Visible = QueuerDAO.GetInstance().CheckPower(this.GetType().ToString(), "04", GlobalVars.LoginUser);
        }

        private void InitTree()
        {
            IList<CmcsFuelKind> rootList = Dbers.GetInstance().SelfDber.Entities<CmcsFuelKind>();

            if (rootList.Count == 0)
            {
                //��ʼ�����ڵ�
                CmcsFuelKind rootFuelKind = new CmcsFuelKind();
                rootFuelKind.Id = "-1";
                rootFuelKind.Name = "ú�ֹ���";
                rootFuelKind.Code = "00";
                rootFuelKind.IsStop = 0;
                rootFuelKind.Sort = 0;
                Dbers.GetInstance().SelfDber.Insert<CmcsFuelKind>(rootFuelKind);
            }

            advTree1.Nodes.Clear();

            CmcsFuelKind rootEntity = Dbers.GetInstance().SelfDber.Get<CmcsFuelKind>("-1");
            DevComponents.AdvTree.Node rootNode = CreateNode(rootEntity);

            LoadData(rootEntity, rootNode);

            advTree1.Nodes.Add(rootNode);

            this.SelFuelKind = rootEntity;

            ProcessFromRequest(eEditMode.�鿴);
        }

        void LoadData(CmcsFuelKind entity, DevComponents.AdvTree.Node node)
        {
            if (entity == null || node == null) return;

            foreach (CmcsFuelKind item in Dbers.GetInstance().SelfDber.Entities<CmcsFuelKind>("where ParentId=:ParentId order by Sort asc", new { ParentId = entity.Id }))
            {
                DevComponents.AdvTree.Node newNode = CreateNode(item);
                node.Nodes.Add(newNode);
                LoadData(item, newNode);
            }
        }

        DevComponents.AdvTree.Node CreateNode(CmcsFuelKind entity)
        {
            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node(entity.Name + ((entity.IsStop == 0) ? "" : "(��Ч)"));
            node.Tag = entity;
            node.Expanded = true;
            return node;
        }

        private void advTree1_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            SelFuelNode();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProcessFromRequest(eEditMode.����);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.SelFuelKind == null)
            {
                MessageBoxEx.Show("����ѡ��һ��ú��!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ProcessFromRequest(eEditMode.�޸�);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.SelFuelKind == null)
            {
                MessageBoxEx.Show("����ѡ��һ��ú��!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ProcessFromRequest(eEditMode.ɾ��);
        }

        private void SelFuelNode()
        {
            this.SelFuelKind = (advTree1.SelectedNode.Tag as CmcsFuelKind);
            ProcessFromRequest(eEditMode.�鿴);
        }

        private void ProcessFromRequest(eEditMode editMode)
        {
            switch (editMode)
            {
                case eEditMode.����:
                    CurrEditMode = editMode;
                    ClearFromControls();
                    HelperUtil.ControlReadOnly(pnlMain, false);

                    chb_IsUse.Checked = true;
                    dbi_Sequence.Value = commonDAO.GetFuelKindSort();
                    break;
                case eEditMode.�޸�:
                    CurrEditMode = editMode;
                    InitObjectInfo();
                    HelperUtil.ControlReadOnly(pnlMain, false);
                    break;
                case eEditMode.�鿴:
                    CurrEditMode = editMode;
                    InitObjectInfo();
                    HelperUtil.ControlReadOnly(pnlMain, true);
                    break;
                case eEditMode.ɾ��:
                    CurrEditMode = editMode;
                    DelTreeNode();
                    ClearFromControls();
                    HelperUtil.ControlReadOnly(pnlMain, true);
                    break;
            }
        }

        private void DelTreeNode()
        {
            if (this.SelFuelKind.Id == "-1") { MessageBoxEx.Show("���ڵ㲻����ɾ��!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBoxEx.Show("ȷ��ɾ���ýڵ㼰�ӽڵ���", "������ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    Dbers.GetInstance().SelfDber.DeleteBySQL<CmcsFuelKind>("where Id=:Id or parentId=:Id", new { Id = SelFuelKind.Id });
                }
                catch (Exception)
                {
                    MessageBoxEx.Show("��ú������ʹ���У���ֹɾ����", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            InitTree();
        }

        private void InitObjectInfo()
        {
            if (this.SelFuelKind == null) return;
            txt_FuelName.Text = this.SelFuelKind.Name;
            txtFuelCode.Text = this.SelFuelKind.Code;
            txt_ReMark.Text = this.SelFuelKind.ReMark;
            dbi_Sequence.Text = this.SelFuelKind.Sort.ToString();
            chb_IsUse.Checked = (this.SelFuelKind.IsStop == 0);
        }

        private void ClearFromControls()
        {
            txt_FuelName.Text = string.Empty;
            txtFuelCode.Text = string.Empty;
            txt_ReMark.Text = string.Empty;
            dbi_Sequence.Value = 0;
            chb_IsUse.Checked = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidatePage()) return;

            if (CurrEditMode == eEditMode.����)
            {
                if (this.SelFuelKind == null) return;
                CmcsFuelKind entity = new CmcsFuelKind();
                entity.Code = commonDAO.GetFuelKindNewChildCode(this.SelFuelKind.Code);
                entity.Name = txt_FuelName.Text;
                entity.Sort = dbi_Sequence.Value;
                entity.ParentId = this.SelFuelKind.Id;
                entity.IsStop = chb_IsUse.Checked ? 0 : 1;
                Dbers.GetInstance().SelfDber.Insert<CmcsFuelKind>(entity);
            }
            else if (CurrEditMode == eEditMode.�޸�)
            {
                if (this.SelFuelKind == null) return;

                //�Ƿ�����ӽڵ�״̬
                if (this.SelFuelKind.IsStop != (chb_IsUse.Checked ? 0 : 1))
                {
                    if (MessageBoxEx.Show("�Ƿ�����״̬Ӧ�õ��ӽڵ�", "������ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        commonDAO.UpdateFuelKindChildsIsUse(this.SelFuelKind.Id, chb_IsUse.Checked ? 0 : 1);
                }

                this.SelFuelKind.Name = txt_FuelName.Text;
                this.SelFuelKind.Code = txtFuelCode.Text;
                this.SelFuelKind.Sort = dbi_Sequence.Value;
                this.SelFuelKind.IsStop = chb_IsUse.Checked ? 0 : 1;
                this.SelFuelKind.ReMark = txt_ReMark.Text;

                Dbers.GetInstance().SelfDber.Update<CmcsFuelKind>(this.SelFuelKind);
            }

            InitTree();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            InitTree();
        }

        /// <summary>
        /// ��֤ҳ��ؼ�ֵ����Ч�Ϸ���
        /// </summary>
        /// <returns></returns>
        private bool ValidatePage()
        {
            if (string.IsNullOrEmpty(txt_FuelName.Text))
            {
                MessageBoxEx.Show("ú�����Ʋ���Ϊ��!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (commonDAO.IsExistFuelKindName(txt_FuelName.Text, SelFuelKind.Id))
            {
                MessageBoxEx.Show("������ͬú������!", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}