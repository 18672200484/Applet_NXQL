using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.Common.DapperDber_etc;
using CMCS.Common;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Utilities;
using CMCS.DumblyConcealer.Tasks.UploadData.Entities;
using CMCS.DapperDber.Dbs.OracleDb;
using System.Data;
using CMCS.Common.Utilities;

namespace CMCS.DumblyConcealer.Tasks.UploadData
{
	public class UploadDataDAO
	{
		private static UploadDataDAO instance;

		public static UploadDataDAO GetInstance()
		{
			if (instance == null)
			{
				instance = new UploadDataDAO();
			}
			return instance;
		}

		private UploadDataDAO()
		{

		}

		CommonDAO commonDAO = CommonDAO.GetInstance();
		OracleDapperDber_iEAA SelfDber = Dbers.GetInstance().SelfDber;



		/// <summary>
		/// 同步数据
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		public void TransferData(List<TableOrView> list, Action<string, eOutputType> output)
		{
			string interfaceUrl = commonDAO.GetAppletConfigString("数据同步智仁接口", "接口地址");
			if (string.IsNullOrWhiteSpace(interfaceUrl))
			{
				output("未在【小程序参数配置】模块中添加配置“接口地址”", eOutputType.Error);
				return;
			}
			
			OracleDapperDber thirdDber = new OracleDapperDber(interfaceUrl);

			foreach (var item in list)
			{
				if (string.IsNullOrWhiteSpace(item.Source) || string.IsNullOrWhiteSpace(item.Destination) || string.IsNullOrWhiteSpace(item.Type)) continue;
				if (item.PropertySetDetails == null || item.PropertySetDetails.Count <= 0) continue;
				if (item.Type.ToLower() == "download")
				{
					DownLoadData(item, thirdDber, output);
				}
				else if (item.Type.ToLower() == "upload")
				{
					UpLoadData(item, thirdDber, output);
				}
				else
				{
					output(string.Format("节点<TableOrView source='{0}' destination={1}>中的类型配置错误，支持类型：upload、download", item.Source, item.Destination), eOutputType.Error);
					return;
				}
			}
		}

		#region 同步集团数据
		/// <summary>
		/// 同步集团数据
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="thirdDber"></param>
		/// <param name="output"></param>
		private void DownLoadData(TableOrView entity, OracleDapperDber thirdDber, Action<string, eOutputType> output)
		{
			//同步集团数据
			string sbSearch = "select ";

			foreach (var item in entity.PropertySetDetails)
			{
				sbSearch += string.Format("{0},", item.Source);
			}

			sbSearch = sbSearch.Trim(',');
			sbSearch += string.Format(" from {0} ", entity.Source);

			if (!string.IsNullOrWhiteSpace(entity.TimeIntervalProperty))
			{
				int intervalValue = 7;
				string configValue = commonDAO.GetAppletConfigString("数据同步智仁接口", "获取集团数据时间间隔（天）");
				if (!string.IsNullOrWhiteSpace(configValue))
					Int32.TryParse(configValue, out intervalValue);
				DateTime startTime = DateTime.Now.AddDays(-intervalValue);

				sbSearch += string.Format(" where {0}>=to_date('{1}','yyyy-MM-dd hh24:mi:ss')", entity.TimeIntervalProperty, startTime.ToString("yyyy-MM-dd HH:mm:ss"));
			}

			DataTable dt = thirdDber.ExecuteDataTable(sbSearch);
			if (dt == null || dt.Rows.Count <= 0)
				return;

			Boolean ishavepk = entity.PropertySetDetails.Count(a => a.DesPrimaryKey != null && a.DesPrimaryKey.ToLower() == "true") > 0;

			foreach (DataRow item in dt.Rows)
			{
				//只有更新和新增操作
				string strChaXun = string.Format("select * from {0} where 1=1 ", entity.Destination);
				if (ishavepk)
				{
					foreach (var pk in entity.PropertySetDetails.Where(a => a.DesPrimaryKey != null && a.DesPrimaryKey.ToLower() == "true"))
					{
						strChaXun += string.Format("and {0}='{1}' ", pk.Destination, item[pk.Source] == null ? "" : item[pk.Source].ToString());
					}
				}
				else
				{
					foreach (var pk in entity.PropertySetDetails)
					{
						strChaXun += string.Format("and {0}='{1}' ", pk.Destination, item[pk.Source] == null ? "" : item[pk.Source].ToString());
					}
				}

				DataTable dtHaveData = SelfDber.ExecuteDataTable(strChaXun);
				if (dtHaveData == null || dtHaveData.Rows.Count <= 0)
				{
					//新增
					string insertSql = string.Format(@"insert into {0} (", entity.Destination);
					string names = "ID, CREATIONTIME, CREATORUSERID,LASTMODIFICATIONTIME,";
					string values = string.Format("'{0}', sysdate,1,sysdate,", Guid.NewGuid().ToString());

					if (entity.Description == "矿点同步")
					{
						string code = commonDAO.GetMineNewChildCode("000");
						if (!string.IsNullOrEmpty(code))
						{
							names += "Code,";
							values += "'" + code + "',";
						}
						names += "Sort,";
						values += commonDAO.GetMineSort() + ",";
					}
					else if (entity.Description == "煤种同步")
					{
						string code = commonDAO.GetFuelKindNewChildCode("000");
						if (!string.IsNullOrEmpty(code))
						{
							names += "Code,";
							values += "'" + code + "',";
						}
						names += "Sort,";
						values += commonDAO.GetFuelKindSort() + ",";
					}

					if (!string.IsNullOrWhiteSpace(entity.IsSoftDelete) && entity.IsSoftDelete.ToLower() == "true")
					{
						names += "ISDELETED,";
						values += "0,";
					}

					if (!string.IsNullOrWhiteSpace(entity.TreeParentId))
					{
						names += "parentid,";
						values += string.Format("'{0}',", entity.TreeParentId);
					}

					foreach (var detail in entity.PropertySetDetails)
					{
						names += string.Format("{0},", detail.Destination);
						values += string.Format("'{0}',", item[detail.Source] == null ? "" : item[detail.Source].ToString());
					}

					if (!string.IsNullOrWhiteSpace(entity.IsHaveSyncTime) && entity.IsHaveSyncTime.ToLower() == "true")
					{
						names += "SYNCTIME,";
						values += "sysdate,";
					}

					insertSql += names.Trim(',') + ") values (" + values.Trim(',') + ")";
					if (SelfDber.Execute(insertSql) > 0)
						output(string.Format("接口取数【{0}】已同步，操作：新增", entity.Description), eOutputType.Normal);
				}
				else
				{
					//更新
					string updateSql = string.Format("update {0} set ", entity.Destination);

					foreach (var detail in entity.PropertySetDetails)
					{
						updateSql += string.Format("{0}='{1}',", detail.Destination, item[detail.Source] == null ? "" : item[detail.Source].ToString());
					}

					if (!string.IsNullOrWhiteSpace(entity.IsHaveSyncTime) && entity.IsHaveSyncTime.ToLower() == "true")
					{
						updateSql += "SYNCTIME=sysdate,";
					}

					updateSql = updateSql.Trim(',') + string.Format(" where id='{0}'", dtHaveData.Rows[0]["ID"].ToString());
					if (SelfDber.Execute(updateSql) > 0)
						output(string.Format("接口取数【{0}】已同步，操作：更新", entity.Description), eOutputType.Normal);
				}
			}

		}
		#endregion

		#region 上报数据
		/// <summary>
		/// 上报数据
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="thirdDber"></param>
		/// <param name="output"></param>
		private void UpLoadData(TableOrView entity, OracleDapperDber thirdDber, Action<string, eOutputType> output)
		{
			//查询本地是否有待上报数据
			string sbSearch = "select ";

			foreach (var item in entity.PropertySetDetails)
			{
				sbSearch += string.Format("{0},", item.Source);
			}

			sbSearch = sbSearch.Trim(',');
			sbSearch += string.Format(" from {0} ", entity.Source);


			if (!string.IsNullOrWhiteSpace(entity.TimeIntervalProperty))
			{
				int intervalValue = 7;
				string configValue = commonDAO.GetAppletConfigString("数据同步智仁接口", "数据上报时间间隔（天）");
				if (!string.IsNullOrWhiteSpace(configValue))
					Int32.TryParse(configValue, out intervalValue);
				DateTime startTime = DateTime.Now.AddDays(-intervalValue);

				sbSearch += string.Format(" where {0}>=to_date('{1}','yyyy-MM-dd hh24:mi:ss')", entity.TimeIntervalProperty, startTime);
			}

			DataTable dt = SelfDber.ExecuteDataTable(sbSearch);
			if (dt == null || dt.Rows.Count <= 0)
				return;

			Boolean ishavepk = entity.PropertySetDetails.Count(a => a.DesPrimaryKey != null && a.DesPrimaryKey.ToLower() == "true") > 0;

			foreach (DataRow item in dt.Rows)
			{
				//只有更新和新增操作
				string strChaXun = string.Format("select * from {0} where 1=1 ", entity.Destination);
				if (ishavepk)
				{
					foreach (var pk in entity.PropertySetDetails.Where(a => a.DesPrimaryKey != null && a.DesPrimaryKey.ToLower() == "true"))
					{
						if (!string.IsNullOrWhiteSpace(pk.DesType) && pk.DesType.ToLower() == "datetime")
						{
							pk.Format = "yyyy-MM-dd hh24:mi:ss";
							strChaXun += string.Format("and {0}=to_date('{1}','{2}') ", pk.Destination, item[pk.Source] == DBNull.Value ? "" : item[pk.Source].ToString(), pk.Format);
						}
						else
							strChaXun += string.Format("and {0}='{1}' ", pk.Destination, item[pk.Source] == DBNull.Value ? "" : item[pk.Source].ToString());
					}
				}
				else
				{
					foreach (var pk in entity.PropertySetDetails)
					{
						if (!string.IsNullOrWhiteSpace(pk.DesType) && pk.DesType.ToLower() == "datetime")
						{
							pk.Format = "yyyy-MM-dd hh24:mi:ss";
							strChaXun += string.Format("and {0}=to_date('{1}','{2}') ", pk.Destination, item[pk.Source] == DBNull.Value ? "" : item[pk.Source].ToString(), pk.Format);
						}
						else
							strChaXun += string.Format("and {0}='{1}' ", pk.Destination, item[pk.Source] == DBNull.Value ? "" : item[pk.Source].ToString());
					}
				}

				DataTable dtHaveData = thirdDber.ExecuteDataTable(strChaXun);
				if (dtHaveData == null || dtHaveData.Rows.Count <= 0)
				{
					//新增
					string insertSql = string.Format(@"insert into {0} (", entity.Destination);
					string names = "";
					string values = "";


					foreach (var detail in entity.PropertySetDetails)
					{
						names += string.Format("{0},", detail.Destination);

						if (detail.DesType != null && detail.DesType.ToLower() == "datetime" && !string.IsNullOrWhiteSpace(detail.Format))
							values += string.Format("{0},", item[detail.Source] == DBNull.Value ? "''" : ("'" + DateTime.Parse(item[detail.Source].ToString()).ToString(detail.Format) + "'"));
						else
							values += string.Format("'{0}',", item[detail.Source] == DBNull.Value ? "" : item[detail.Source].ToString());
					}

					insertSql += names.Trim(',') + ") values (" + values.Trim(',') + ")";
					if (thirdDber.Execute(insertSql) > 0)
						output(string.Format("接口上报数据【{0}】已同步，操作：新增", entity.Description), eOutputType.Normal);
				}
				else
				{
					//更新
					string updateSql = string.Format("update {0} set ", entity.Destination);

					foreach (var detail in entity.PropertySetDetails)
					{
						if (detail.DesType != null && detail.DesType.ToLower() == "datetime" && !string.IsNullOrWhiteSpace(detail.Format))
							updateSql += string.Format("{0}={1},", detail.Destination, item[detail.Source] == DBNull.Value ? "''" : ("'" + DateTime.Parse(item[detail.Source].ToString()).ToString(detail.Format) + "'"));
						else
							updateSql += string.Format("{0}='{1}',", detail.Destination, item[detail.Source] == DBNull.Value ? "" : item[detail.Source].ToString());
					}
					//if (entity.Description == "编码接口表")
					//    updateSql = updateSql.Trim(',') + string.Format(" where id='{0}'", dtHaveData.Rows[0]["TRUCKENTERID"].ToString());
					//else

					updateSql = updateSql.Trim(',');
					string updateWhere = "";
					if (ishavepk)
					{
						int index = 0;
						foreach (var pk in entity.PropertySetDetails.Where(a => a.DesPrimaryKey != null && a.DesPrimaryKey.ToLower() == "true"))
						{
							if (!string.IsNullOrWhiteSpace(pk.DesType) && pk.DesType.ToLower() == "datetime")
							{
								pk.Format = "yyyy-MM-dd hh24:mi:ss";
								updateWhere += string.Format("{3} {0}=to_date('{1}','{2}') ", pk.Destination, item[pk.Source] == DBNull.Value ? "" : item[pk.Source].ToString(), pk.Format, (index == 0 ? "" : "and"));
							}
							else
								updateWhere += string.Format("{2} {0}='{1}' ", pk.Destination, item[pk.Source] == DBNull.Value ? "" : item[pk.Source].ToString(), (index == 0 ? "" : "and"));
							index++;
						}

						if (!string.IsNullOrWhiteSpace(updateWhere))
							updateWhere = " where " + updateWhere;
					}

					if (string.IsNullOrWhiteSpace(updateWhere))
						updateWhere = string.Format(" where {0}='{1}'", entity.PropertySetDetails.First().Destination, dtHaveData.Rows[0][entity.PropertySetDetails.First().Destination].ToString());

					updateSql = updateSql + " " + updateWhere;
					//updateSql = updateSql.Trim(',') + string.Format(" where {0}='{1}'", entity.PropertySetDetails.First().Destination, dtHaveData.Rows[0][entity.PropertySetDetails.First().Destination].ToString());
					if (thirdDber.Execute(updateSql) > 0)
						output(string.Format("接口取数【{0}】已同步，操作：更新", entity.Description), eOutputType.Normal);
				}
			}

		}
		#endregion

		#region 处理集团基础信息操作日志表
		public void TransferBaseOperLog(Action<string, eOutputType> output)
		{
			string interfaceUrl = commonDAO.GetAppletConfigString("数据同步智仁接口", "接口地址");
			if (string.IsNullOrWhiteSpace(interfaceUrl))
			{
				output("未在【小程序参数配置】模块中添加配置“接口地址”", eOutputType.Error);
				return;
			}

			OracleDapperDber thirdDber = new OracleDapperDber(interfaceUrl);

			int intervalValue = 7;
			string configValue = commonDAO.GetAppletConfigString("数据同步智仁接口", "获取集团数据时间间隔（天）");
			if (!string.IsNullOrWhiteSpace(configValue))
				Int32.TryParse(configValue, out intervalValue);
			DateTime startTime = DateTime.Now.AddDays(-intervalValue);

			string searchSql = string.Format("select * from V_JK_COALLOG where RIQ>=to_date('{0}','yyyy-MM-dd hh24:mi:ss') order by RIQ desc", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
			DataTable dt = thirdDber.ExecuteDataTable(searchSql);
			if (dt == null || dt.Rows.Count <= 0)
				return;

			foreach (DataRow item in dt.Rows)
			{
				string sql = "";
				if (item["LEIX"].ToString() == "煤矿单位")
				{
					#region 煤矿
					if (item["DONGZ"].ToString() == "增加")
					{
						//增加的不管，增加的在基础信息里面有，直接同步过来就行
						continue;

//						//先查有没有，没有就新增，有就更新
//						DataTable dtTemp = SelfDber.ExecuteDataTable(string.Format("select * from fultbmine where name='{0}' or CompanyCode='{1}'", item["NEWNAME"], item["NEWCODE"]));
//						if (dtTemp != null && dtTemp.Rows.Count > 0)
//						{
//							if (dtTemp.Rows.Count > 1)
//							{
//								output(string.Format("矿点：名称【{0}】，编码【{1}】，查询到多条记录，更新失败！请在系统中检查数据合理性！", item["NEWNAME"], item["NEWCODE"]), eOutputType.Error);
//								continue;
//							}
//							else
//								sql = string.Format("update fultbmine set name='{0}',CompanyCode='{1}',SYNCTIME=sysdate where name='{2}' and CompanyCode='{3}'", item["NEWNAME"], item["NEWCODE"], item["OLDNAME"], item["OLDCODE"]);

//						}
//						else //新增
//						{
//							sql = string.Format(@"insert into fultbmine (ID, CREATIONTIME, CREATORUSERID, ISDELETED, CODE, SORT, NAME, ISSTOP, DATAFROM, PARENTID, SYNCFLAG, COMPANYCODE, SHORTNAME, DATAFLAG, SYNCTIME)
//values ('{0}', sysdate, 1, 0, '{1}', 1, '{2}', 0, '集团接口', '-1', 0, '{1}', '{2}', 0, sysdate)", Guid.NewGuid().ToString(), item["NEWCODE"], item["NEWNAME"]);
//							Log4Neter.Info(sql);
//						}
					}
					else if (item["DONGZ"].ToString() == "更新")
						sql = string.Format("update fultbmine set shortname='{0}',CompanyCode='{1}',SYNCTIME=sysdate where shortname='{2}' and CompanyCode='{3}'", item["NEWNAME"], item["NEWCODE"], item["OLDNAME"], item["OLDCODE"]);
					else if (item["DONGZ"].ToString() == "删除")
						sql = string.Format("update fultbmine set isstop=1,SYNCTIME=sysdate where shortname='{0}' and CompanyCode='{1}'", item["OLDNAME"], item["OLDCODE"]);
					#endregion
				}
				else if (item["LEIX"].ToString() == "运输单位")
				{
					#region 运输单位
					if (item["DONGZ"].ToString() == "增加")
					{
						//增加的不管，增加的在基础信息里面有，直接同步过来就行
						continue;

//						//先查有没有，没有就新增，有就更新
//						DataTable dtTemp = SelfDber.ExecuteDataTable(string.Format("select * from fultbtransportcompany where name='{0}' or code='{1}'", item["NEWNAME"], item["NEWCODE"]));
//						if (dtTemp != null && dtTemp.Rows.Count > 0)
//						{
//							if (dtTemp.Rows.Count > 1)
//							{
//								output(string.Format("运输单位：名称【{0}】，编码【{1}】，查询到多条记录，更新失败！请在系统中检查数据合理性！", item["OLDNAME"], item["OLDCODE"]), eOutputType.Error);
//								continue;
//							}
//							else
//								sql = string.Format("update fultbtransportcompany set name='{0}',code='{1}',SYNCTIME=sysdate where name='{2}' and code='{3}'", item["NEWNAME"], item["NEWCODE"], item["OLDNAME"], item["OLDCODE"]);

//						}
//						else //新增
//							sql = string.Format(@"insert into fultbtransportcompany (ID, CREATIONTIME, CREATORUSERID, ISDELETED, CODE, NAME, ISSTOP, DATAFROM, SYNCFLAG, SYNCTIME)
//values ('{0}', sysdate, 1, 0, '{1}', '{2}', 0, '集团接口', 0, sysdate)", Guid.NewGuid().ToString(), item["NEWCODE"], item["NEWNAME"]);
					}
					else if (item["DONGZ"].ToString() == "更新")
						sql = string.Format("update fultbtransportcompany set shortname='{0}',code='{1}',SYNCTIME=sysdate where shortname='{2}' and code='{3}'", item["NEWNAME"], item["NEWCODE"], item["OLDNAME"], item["OLDCODE"]);
					else if (item["DONGZ"].ToString() == "删除")
						sql = string.Format("update fultbtransportcompany set isstop=1,SYNCTIME=sysdate where shortname='{0}' and code='{1}'", item["OLDNAME"], item["OLDCODE"]);
					#endregion
				}
				else if (item["LEIX"].ToString() == "煤种")
				{
					#region 煤种
					if (item["DONGZ"].ToString() == "增加")
					{
						//增加的不管，增加的在基础信息里面有，直接同步过来就行
						continue;


//						//先查有没有，没有就新增，有就更新
//						DataTable dtTemp = SelfDber.ExecuteDataTable(string.Format("select * from fultbfuelkind where name='{0}' or CompanyCode='{1}'", item["NEWNAME"], item["NEWCODE"]));
//						if (dtTemp != null && dtTemp.Rows.Count > 0)
//						{
//							if (dtTemp.Rows.Count > 1)
//							{
//								output(string.Format("煤种：名称【{0}】，编码【{1}】，查询到多条记录，更新失败！请在系统中检查数据合理性！", item["OLDNAME"], item["OLDCODE"]), eOutputType.Error);
//								continue;
//							}
//							else
//								sql = string.Format("update fultbfuelkind set name='{0}',CompanyCode='{1}',SYNCTIME=sysdate where name='{2}' and CompanyCode='{3}'", item["NEWNAME"], item["NEWCODE"], item["OLDNAME"], item["OLDCODE"]);

//						}
//						else //新增
//							sql = string.Format(@"insert into fultbfuelkind (ID, CREATIONTIME, CREATORUSERID, ISDELETED, CODE, SORT, NAME, ISSTOP, DATAFROM, PARENTID, SYNCFLAG, COMPANYCODE, SHORTNAME, DATAFLAG, SYNCTIME)
//values ('{0}', sysdate, 1, 0, '{1}', 1, '{2}', 0, '集团接口', '-1', 0, '{1}', '{2}', 0, sysdate)", Guid.NewGuid().ToString(), item["NEWCODE"], item["NEWNAME"]);
					}
					else if (item["DONGZ"].ToString() == "更新")
						sql = string.Format("update fultbfuelkind set name='{0}',CompanyCode='{1}',SYNCTIME=sysdate where name='{2}' and CompanyCode='{3}'", item["NEWNAME"], item["NEWCODE"], item["OLDNAME"], item["OLDCODE"]);
					else if (item["DONGZ"].ToString() == "删除")
						sql = string.Format("update fultbfuelkind set isstop=1,SYNCTIME=sysdate where name='{0}' and CompanyCode='{1}'", item["OLDNAME"], item["OLDCODE"]);
					#endregion
				}
				if (string.IsNullOrWhiteSpace(sql)) continue;
				if (SelfDber.Execute(sql) > 0)
					output(string.Format("基础信息操作记录处理完成，操作内容：{0}，操作动作：{1}，修改前编码：{2}，修改后编码：{3}，修改前名称：{4}，修改后名称：{5}", item["LEIX"], item["DONGZ"], item["OLDCODE"], item["NEWCODE"], item["OLDNAME"], item["NEWNAME"]), eOutputType.Normal);
			}

		}
		#endregion

		#region 同步化验结果
		public void TransferAssayQulity(Action<string, eOutputType> output)
		{
			string interfaceUrl = commonDAO.GetAppletConfigString("数据同步智仁接口", "接口地址");
			if (string.IsNullOrWhiteSpace(interfaceUrl))
			{
				output("未在【小程序参数配置】模块中添加配置“接口地址”", eOutputType.Error);
				return;
			}

			OracleDapperDber thirdDber = new OracleDapperDber(interfaceUrl);

			int intervalValue = 7;
			string configValue = commonDAO.GetAppletConfigString("数据同步智仁接口", "获取集团数据时间间隔（天）");
			if (!string.IsNullOrWhiteSpace(configValue))
				Int32.TryParse(configValue, out intervalValue);
			DateTime startTime = DateTime.Now.AddDays(-intervalValue);

			string searchSql = string.Format("select * from V_JK_HUAY where assaycode like 'Z%' and RIQ>=to_date('{0}','yyyy-MM-dd hh24:mi:ss')", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
			DataTable dt = thirdDber.ExecuteDataTable(searchSql);
			if (dt == null || dt.Rows.Count <= 0)
				return;

			foreach (DataRow item in dt.Rows)
			{
				string assaycode = string.Empty;
				DataTable assaydata = SelfDber.ExecuteDataTable(string.Format("select b.assaycode from cmcstbmake a inner join cmcstbassay b on a.id=b.makeid where a.makecode='{0}'", item["ASSAYCODE"]));
				if (assaydata != null && assaydata.Rows.Count > 0)
				{
					assaycode = assaydata.Rows[0][0].ToString();
				}
				if (string.IsNullOrEmpty(assaycode)) continue;
				string assaySql = string.Format("update cmcstbassay set ASSAYDATE=to_date('{0}','yyyy-MM-dd hh24:mi:ss'),ASSAYPLE='{1}' where ASSAYCODE='{2}'", item["RIQ"], item["HUYY"], assaycode);
				string qualitySql = string.Format(@"update fultbfuelquality t set t.mt={0},t.mad={1},t.ad={2},t.aar={3},t.aad={4},t.vad={5},t.vdaf={6},t.std={7},t.stad={8},t.had={9},t.hdaf={10},t.qnetarmj={11},t.qbad={12},t.qgrd={13},t.qgrad={14} where t.id in(select FUELQUALITYID from cmcstbassay where ASSAYCODE='{15}')", item["MT"], item["MAD"], item["AD"], item["AAR"], item["AAD"], item["VAD"], item["VDAF"], item["STD"], item["STAD"], item["HAD"], item["HDAF"], item["QNETAR"], item["QBAD"], item["QGRD"], item["QGRAD"], assaycode);

				if (SelfDber.Execute(assaySql) + SelfDber.Execute(qualitySql) > 0)
					output(string.Format("化验编码：{0}，煤质信息同步成功！", assaycode), eOutputType.Normal);
			}

		}
		#endregion
	}
}
