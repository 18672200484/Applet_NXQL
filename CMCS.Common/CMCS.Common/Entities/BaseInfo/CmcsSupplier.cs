using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys; 

namespace CMCS.Common.Entities.BaseInfo
{
    /// <summary>
    /// 基础信息-供应商
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("fultbsupplier")]
    public class CmcsSupplier : EntityBase1
    {
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 信誉等级
        /// </summary>
        public virtual string CreditRank { get; set; }


        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string Name { get; set; }


        /// <summary>
        /// 供应商简称
        /// </summary>
        public virtual string ShortName { get; set; }

        /// <summary>
        /// 供应商类型
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// 是否启用   0 启用 1 停用
        /// </summary>
        public virtual int IsStop { get; set; }

        /// <summary>
        /// 是否审核 未审核0 已审核1 默认未审核
        /// </summary>
        public virtual Int32 IsCheck { get; set; }

        /// <summary>
        /// 法人代表
        /// </summary>
        public virtual string RepIdentity { get; set; }

        /// <summary>
        /// 组织机构代码
        /// </summary>
        public virtual string OrganizationCode { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        public virtual decimal RegisterFund { get; set; }

        /// <summary>
        /// 营业执照编号
        /// </summary>
        public virtual string LicenceNum { get; set; }


        /// <summary>
        /// 经营许可证编号
        /// </summary>
        public virtual string Operallver { get; set; }

        /// <summary>
        /// 税务登记证代码
        /// </summary>
        public virtual string TaxregCode { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public virtual string LinkMan { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public virtual string LinkTel { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public virtual string Facsimile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public virtual string PostalCode { get; set; }

        /// <summary>
        /// 单位地址
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public virtual string DataFrom { get; set; }

        /// <summary>
        ///  工作流当前节点
        /// </summary>
        public virtual string WfName { get; set; }

        /// <summary>
        ///  客户端登录账号
        /// </summary>
        public virtual string LoginAccount { get; set; }

        /// <summary>
        /// 数据同步标识 0未同步 1已同步
        /// </summary>
        public int SyncFlag { get; set; }
    }
}
