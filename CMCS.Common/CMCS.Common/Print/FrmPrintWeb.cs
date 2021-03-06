﻿using System;
using System.Drawing;
using System.Windows.Forms;
//
using DevComponents.DotNetBar.Metro;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Enums;
using System.Collections;
using CMCS.Common.DAO;
using System.Collections.Generic;

namespace CMCS.Common.Print
{
	/// <summary>
	/// 打印预览
	/// </summary>
	public partial class FrmPrintWeb : DevComponents.DotNetBar.Metro.MetroForm
	{
		WagonPrinter wagonPrinter = null;
		CmcsBuyFuelTransport _BuyFuelTransport = null;
		CmcsGoodsTransport _GoodsTransport = null;
		Font TitleFont = new Font("宋体", 18, FontStyle.Bold, GraphicsUnit.Pixel);
		Font ContentFont = new Font("宋体", 14, FontStyle.Regular, GraphicsUnit.Pixel);

		int PageIndex = 1;

		public FrmPrintWeb(CmcsBuyFuelTransport buyfueltransport, CmcsGoodsTransport goodstransport = null)
		{
			_BuyFuelTransport = buyfueltransport;
			_GoodsTransport = goodstransport;
			InitializeComponent();
		}

		/// <summary>
		/// 打印磅单
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsmiPrint_Click(object sender, EventArgs e)
		{
			this.wagonPrinter.Print(this._BuyFuelTransport, this._GoodsTransport);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			string SerialNumber = string.Empty,
					CarNumber = string.Empty,
					MineName = string.Empty,
					FuelKindName = string.Empty,
					GrossTime = string.Empty,
					TareTime = string.Empty,
					TicketWeight = string.Empty,
					GrossWeight = string.Empty,
					TareWeight = string.Empty,
					SuttleWeight = string.Empty,
					DeductWeight = string.Empty,
					CheckWeight = string.Empty,
					UserName = string.Empty;
			if (this._BuyFuelTransport != null)
			{
				SerialNumber = this._BuyFuelTransport.SerialNumber;
				CarNumber = this._BuyFuelTransport.CarNumber;
				MineName = this._BuyFuelTransport.MineName;
				FuelKindName = this._BuyFuelTransport.FuelKindName;
				GrossTime = DisposeTime(this._BuyFuelTransport.GrossTime.ToString(), "yyyy-MM-dd HH:mm");
				TareTime = DisposeTime(this._BuyFuelTransport.TareTime.ToString(), "yyyy-MM-dd HH:mm");
				TicketWeight = this._BuyFuelTransport.TicketWeight.ToString("F2").PadLeft(6, ' ');
				GrossWeight = this._BuyFuelTransport.GrossWeight.ToString("F2").PadLeft(6, ' ');
				TareWeight = this._BuyFuelTransport.TareWeight.ToString("F2").PadLeft(6, ' ');
				SuttleWeight = this._BuyFuelTransport.SuttleWeight.ToString("F2").PadLeft(6, ' ');
				DeductWeight = this._BuyFuelTransport.DeductWeight.ToString("F2").PadLeft(6, ' ');
				List<CmcsBuyFuelTransportDeduct> deductlist = CommonDAO.GetInstance().SelfDber.Entities<CmcsBuyFuelTransportDeduct>("where TransportId=:Transportid order by CreationTime desc", new { Transportid = this._BuyFuelTransport.Id });
				#region 入厂煤
				// 行间距 24 
				float TopValue = 53;
				string printValue = "";
				g.DrawString("国电投青铝发电有限公司", TitleFont, Brushes.White, 30, TopValue);
				TopValue += 34;

				g.DrawString("打印时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawLine(new Pen(Color.White, 2), 0, TopValue, 300 - 10, TopValue);
				TopValue += 15;

				g.DrawString("车 牌 号：" + CarNumber, ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				printValue = printValue = "矿    点：" + MineName;
				if (printValue.Length > 18)
				{
					g.DrawString(printValue.Substring(0, 18), ContentFont, Brushes.White, 30, TopValue);
					TopValue += 24;
					g.DrawString(printValue.Substring(18, printValue.Length - 18), ContentFont, Brushes.White, 105, TopValue);
					TopValue += 24;
				}
				else
				{
					g.DrawString(printValue, ContentFont, Brushes.White, 30, TopValue);
					TopValue += 24;
				}

				g.DrawString("煤    种：" + FuelKindName, ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString(string.Format("矿 发 量：{0} 吨", TicketWeight), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString(string.Format("毛    重：{0} 吨", GrossWeight), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString("毛重时间：" + this._BuyFuelTransport.GrossTime.ToString("yyyy-MM-dd HH:mm"), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString(string.Format("皮    重：{0} 吨", TareWeight), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString("皮重时间：" + this._BuyFuelTransport.TareTime.ToString("yyyy-MM-dd HH:mm"), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				int deductindex = 1;
				foreach (CmcsBuyFuelTransportDeduct item in deductlist)
				{
					g.DrawString(string.Format("扣   吨{0}：{1} 吨 ({2})", deductindex, item.DeductWeight, item.DeductType), ContentFont, Brushes.White, 30, TopValue);
					TopValue += 24;
					deductindex++;
				}

				g.DrawString(string.Format("净    重：{0} 吨", SuttleWeight), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString(string.Format("计量员签字："), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 34;

				g.DrawString(string.Format("监磅员签字："), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 34;

				g.DrawString(string.Format("司机签字："), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 34;
				g.DrawString("", ContentFont, Brushes.Black, 30, TopValue);
				#endregion
			}
			else if (this._GoodsTransport != null)
			{
				SerialNumber = this._GoodsTransport.SerialNumber;
				CarNumber = this._GoodsTransport.CarNumber;
				MineName = this._GoodsTransport.SupplyUnitName;
				FuelKindName = this._GoodsTransport.GoodsTypeName;
				GrossTime = DisposeTime(this._GoodsTransport.FirstTime.ToString(), "yyyy-MM-dd HH:mm");
				TareTime = DisposeTime(this._GoodsTransport.SecondTime.ToString(), "yyyy-MM-dd HH:mm");
				GrossWeight = this._GoodsTransport.FirstWeight.ToString("F2").PadLeft(6, ' ');
				TareWeight = this._GoodsTransport.SecondWeight.ToString("F2").PadLeft(6, ' ');
				SuttleWeight = this._GoodsTransport.SuttleWeight.ToString("F2").PadLeft(6, ' ');

				#region 其他物资
				// 行间距 24 
				float TopValue = 53;
				string printValue = "";
				g.DrawString(PrintAppConfig.GetInstance().TitleContent, TitleFont, Brushes.Black, 30, TopValue);
				TopValue += 34;

				g.DrawString("打印时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawLine(new Pen(Color.Black, 2), 0, TopValue, 300 - 10, TopValue);
				TopValue += 15;

				g.DrawString("车 牌 号：" + CarNumber, ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				printValue = "单    位：" + MineName;
				if (printValue.Length > 18)
				{
					g.DrawString(printValue.Substring(0, 18), ContentFont, Brushes.White, 30, TopValue);
					TopValue += 24;
					g.DrawString(printValue.Substring(18, printValue.Length - 18), ContentFont, Brushes.White, 105, TopValue);
					TopValue += 24;
				}
				else
				{
					g.DrawString(printValue, ContentFont, Brushes.White, 30, TopValue);
					TopValue += 24;
				}

				g.DrawString("物资类型：" + FuelKindName, ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString(string.Format("一次称重：{0} 吨", GrossWeight), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString("一次时间：" + GrossTime, ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString(string.Format("二次称重：{0} 吨", TareWeight), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString("二次时间：" + TareTime, ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString(string.Format("净    重：{0} 吨", SuttleWeight), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 24;

				g.DrawString(string.Format("计量员签字："), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 34;

				g.DrawString(string.Format("监磅员签字："), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 34;

				g.DrawString(string.Format("司机签字："), ContentFont, Brushes.White, 30, TopValue);
				TopValue += 34;
				g.DrawString("", ContentFont, Brushes.Black, 30, TopValue);
				#endregion
			}
		}
		public static string DisposeTime(string dt, string format)
		{
			if (!string.IsNullOrEmpty(dt))
			{
				DateTime dti = DateTime.Parse(dt);
				if (dti > new DateTime(2000, 1, 1))
					return dti.ToString(format);
			}
			return string.Empty;
		}


		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			this.Close();
		}

		private void FrmPrintWeb_Load(object sender, EventArgs e)
		{
			this.wagonPrinter = new WagonPrinter(printDocument1);
		}
	}
}
